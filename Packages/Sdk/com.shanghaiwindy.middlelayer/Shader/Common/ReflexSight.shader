// Generated from ReflexSight.windyshader by RuntimeSupportExport. Do not edit.
Shader "WindyVerse/Weapon/ReflexSight"
{
    Properties
    {
        _LensColor ("Lens Tint", Color) = (0.12, 0.22, 0.19, 0.025)
        _CoatingColor ("Coating Reflection Tint", Color) = (0.75, 0.85, 1, 1)
        _Reflectance ("Head-on Reflectance", Range(0, 0.2)) = 0.012
        _Roughness ("Perceptual Roughness", Range(0, 1)) = 0.04
        [NoScaleOffset] _DirtMap ("Dirt (R smudge, G dust, B scratches)", 2D) = "black" {}
        _DirtTiling ("Dirt Tiles per Metre", Float) = 20
        _DirtStrength ("Dirt Strength", Range(0, 1)) = 0.45
        _DotColor ("Dot Color", Color) = (1, 0.025, 0.01, 1)
        _DotIntensity ("Dot Intensity (HDR)", Range(1, 8)) = 2.5
        _DotSize ("Dot Diameter (MOA)", Range(1, 20)) = 6
        _DotMinPixels ("Minimum Dot Diameter (pixels)", Range(1, 6)) = 3
        _ReflexShape ("Shape: Dot / Ring Dot / Cross", Float) = 0
        _ReticleSize ("Outer Diameter (MOA)", Range(10, 100)) = 40
        _ReticleStroke ("Stroke Width (MOA)", Range(0.5, 6)) = 2
        [HideInInspector]_ReflexDirection ("View Direction", Vector) = (0, 0, -1, 0)
        [HideInInspector]_ReflexOpacity ("Opacity", Float) = 0
    }
    HLSLINCLUDE
#line 4 "Packages/com.shanghaiwindy.middlelayer/Shader/Common/WindyCommon.windyshaderinc"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

        #define WINDY_URP 1

        #define WINDY_ObjectToWorld(positionOS) TransformObjectToWorld(positionOS)
        #define WINDY_ObjectToWorldNormal(normalOS) TransformObjectToWorldNormal(normalOS)
        #define WINDY_WorldToHClip(positionWS) TransformWorldToHClip(positionWS)
        #define WINDY_ObjectToHClip(positionOS) TransformObjectToHClip(positionOS)
        #define WINDY_ViewVector(positionWS) (_WorldSpaceCameraPos.xyz - (positionWS))
        #define WINDY_AbsolutePositionWS(positionWS) (positionWS)
        #define WINDY_ScreenSize (_ScreenParams.xy)
        #define WINDY_SceneEyeDepth(positionCS) LinearEyeDepth(LoadSceneDepth(uint2((positionCS).xy)), _ZBufferParams)
        // Soft edges against the scene (particle / depth fades): SceneDepth says whether the depth texture exists
        // this frame and the player wants them; off = skip the read and keep the raw alpha.
        float _WindySoftParticles;
        #define WINDY_SOFT_PARTICLES (_WindySoftParticles > 0.5)
        // URP renders in unexposed linear, so both of these fold away.
        #define WINDY_Exposure (half(1.0))
        #define WINDY_DeExposure (half(1.0))

        // Same contract as UNITY_FOG_COORDS/TRANSFER/APPLY; no-ops on HDRP, so
        // callers never touch the fog field or wrap fog in #if themselves.
        #define WINDY_FOG_COORDS(idx) half fogFactor : TEXCOORD##idx;
        #define WINDY_TRANSFER_FOG(output, clipPos) output.fogFactor = half(ComputeFogFactor((clipPos).z))
        #define WINDY_APPLY_FOG(input, color) color = MixFog(color, input.fogFactor)
        // Blend modes other than alpha have their own "invisible" colour, and
        // lerping them toward the fog colour brightens or darkens instead of
        // hiding: additive fades to black, multiply fades to white.
        #define WINDY_APPLY_FOG_TO(input, color, target) color = MixFogColor(color, target, input.fogFactor)
#line 10 "Packages/com.shanghaiwindy.middlelayer/Shader/Common/WindyLit.windyshaderinc"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        // SampleSH9 home; URP 17 no longer pulls it in through Lighting.hlsl.
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SphericalHarmonics.hlsl"

        // Set by ShadowUtils.SetupShadowCasterConstantBuffer, not per material.
        float3 _LightDirection;
        float3 _LightPosition;

        // ShadowCaster vertex math; the pass supplies _CASTING_PUNCTUAL_LIGHT_SHADOW
        // via multi_compile_vertex.
        float4 WindyShadowCasterPositionCS(float3 positionOS, float3 normalOS)
        {
            float3 positionWS = TransformObjectToWorld(positionOS);
            float3 normalWS = TransformObjectToWorldNormal(normalOS);

            #if defined(_CASTING_PUNCTUAL_LIGHT_SHADOW)
            float3 lightDirectionWS = normalize(_LightPosition - positionWS);
            #else
            float3 lightDirectionWS = _LightDirection;
            #endif

            float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
            #if UNITY_REVERSED_Z
            positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
            #else
            positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
            #endif
            return positionCS;
        }

        // URP 17 removed SampleSH(normal); evaluate the object probe directly.
        half3 WindyAmbientSH(half3 normalWS)
        {
            real4 sh[7] = { unity_SHAr, unity_SHAg, unity_SHAb, unity_SHBr, unity_SHBg, unity_SHBb, unity_SHC };
            return half3(max(real3(0.0, 0.0, 0.0), SampleSH9(sh, real3(normalWS))));
        }
#line 67 "Packages/com.shanghaiwindy.middlelayer/Shader/Common/WindyCommon.windyshaderinc"
        // Shadergraph Simple Noise: 3-octave value noise, kept bit-identical so
        // converted effects keep their exact patterns.
        float WindyNoiseRandomValue(float2 uv)
        {
            return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
        }

        float WindyNoiseValue(float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            float r0 = WindyNoiseRandomValue(i);
            float r1 = WindyNoiseRandomValue(i + float2(1.0, 0.0));
            float r2 = WindyNoiseRandomValue(i + float2(0.0, 1.0));
            float r3 = WindyNoiseRandomValue(i + float2(1.0, 1.0));
            return lerp(lerp(r0, r1, f.x), lerp(r2, r3, f.x), f.y);
        }

        float WindySimpleNoise(float2 uv, float scale)
        {
            float t = WindyNoiseValue(uv * scale) * 0.125;
            t += WindyNoiseValue(uv * (scale * 0.5)) * 0.25;
            t += WindyNoiseValue(uv * (scale * 0.25)) * 0.5;
            return t;
        }
#line 32 "Packages/com.shanghaiwindy.middlelayer/Shader/Common/ReflexSight.windyshader"
        CBUFFER_START(UnityPerMaterial)
            half4 _LensColor;
            half4 _CoatingColor;
            half _Reflectance;
            half _Roughness;
            float _DirtTiling;
            half _DirtStrength;
            half4 _DotColor;
            half _DotIntensity;
            float _DotSize;
            float _DotMinPixels;
            float _ReflexShape;
            float _ReticleSize;
            float _ReticleStroke;
            float4 _ReflexDirection;
            half _ReflexOpacity;
        CBUFFER_END

        TEXTURE2D(_DirtMap);
        SAMPLER(sampler_DirtMap);

        struct ReticleSample
        {
            half core;
            half glow;
            // Angular distance from the reticle centre in units of the dot radius: the dirt halo falls off with it.
            half spread;
        };

        // positionVS: the fragment in view space; the reticle sits where the aim ray crosses the pane.
        ReticleSample SampleReticle(float3 positionVS)
        {
            float2 ray = positionVS.xy / max(-positionVS.z, 0.0001);
            float2 target = _ReflexDirection.xy / max(-_ReflexDirection.z, 0.0001);
            float2 delta = ray - target;
            float pixel = max(length(ddx(delta)), length(ddy(delta)));
            float radius = max(tan(_DotSize * (3.14159265 / 21600.0)), pixel * _DotMinPixels * 0.5);
            float distance = length(delta);
            float core = saturate(0.5 + (radius - distance) / max(pixel, 0.000001));
            if (_ReflexShape > 0.5)
            {
                float outer = max(tan(_ReticleSize * (3.14159265 / 21600.0)), radius * 4.0);
                float stroke = max(tan(_ReticleStroke * (3.14159265 / 21600.0)), pixel * 0.5);
                float shapeDistance = abs(distance - outer) - stroke;
                if (_ReflexShape > 1.5)
                {
                    float2 axis = abs(delta);
                    float along = max(axis.x, axis.y);
                    shapeDistance = max(min(axis.x, axis.y) - stroke,
                        max(outer * 0.3 - along, along - outer));
                }
                core = max(core, saturate(0.5 - shapeDistance / max(pixel, 0.000001)));
            }

            ReticleSample result;
            result.core = half(core);
            result.glow = half(pow(saturate(1.0 - distance / (radius * 2.2)), 3.0) * 0.35);
            result.spread = half(distance / radius);
            return result;
        }

        // The pane is flat, so the dirt is projected on whichever object plane faces along its normal; it
        // stays on the glass as the weapon moves. R smudges, G dust, B scratches.
        half3 SampleDirt(float3 positionOS, float3 normalOS)
        {
            float3 axis = abs(normalOS);
            float2 uv = positionOS.xy;
            if (axis.x > axis.y && axis.x > axis.z)
            {
                uv = positionOS.zy;
            }
            else if (axis.y > axis.z)
            {
                uv = positionOS.xz;
            }
            return SAMPLE_TEXTURE2D(_DirtMap, sampler_DirtMap, uv * _DirtTiling).rgb * _DirtStrength;
        }

        // Schlick from the coating's head-on reflectance to a mirror at grazing angles.
        half LensReflectance(half nv)
        {
            half grazing = 1.0h - nv;
            half grazing2 = grazing * grazing;
            return lerp(_Reflectance, 1.0h, grazing2 * grazing2 * grazing);
        }

        struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
        };

        #if defined(WINDY_URP)

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS : TEXCOORD0;
            float3 normalWS : TEXCOORD1;
            float3 positionVS : TEXCOORD2;
            float3 positionOS : TEXCOORD3;
            float3 normalOS : TEXCOORD4;
            WINDY_FOG_COORDS(5)
        };

        Varyings Vert(Attributes input)
        {
            Varyings output;
            output.positionWS = TransformObjectToWorld(input.positionOS);
            output.positionCS = TransformWorldToHClip(output.positionWS);
            output.normalWS = TransformObjectToWorldNormal(input.normalOS);
            output.positionVS = TransformWorldToView(output.positionWS);
            output.positionOS = input.positionOS;
            output.normalOS = input.normalOS;
            WINDY_TRANSFER_FOG(output, output.positionCS);
            return output;
        }

        half4 Frag(Varyings input) : SV_Target
        {
            ReticleSample reticle = SampleReticle(input.positionVS);
            half3 dirt = SampleDirt(input.positionOS, input.normalOS);
            half grime = saturate(dirt.r * 0.6h + dirt.g + dirt.b * 0.8h);

            half3 normalWS = half3(normalize(input.normalWS));
            half3 viewWS = half3(GetWorldSpaceNormalizeViewDir(input.positionWS));
            half nv = saturate(dot(normalWS, viewWS));
            half reflectance = LensReflectance(nv);
            half3 coating = lerp(_CoatingColor.rgb, half3(1.0h, 1.0h, 1.0h), reflectance);

            // Smudges roughen the coating: the sky blurs and the sun spreads into a sheen across them.
            // The probe can be black (realtime reflection gated off), so the SH sky keeps a reflection.
            half roughness = lerp(_Roughness, 0.55h, saturate(dirt.r * 1.5h));
            half3 reflectWS = reflect(-viewWS, normalWS);
            half3 sky = max(GlossyEnvironmentReflection(reflectWS, roughness, 1.0h), WindyAmbientSH(reflectWS));
            Light mainLight = GetMainLight();
            half3 sun = mainLight.color * mainLight.distanceAttenuation;
            // Full precision: a 2048 exponent on a half dot product bands on mobile GPUs.
            float3 halfDir = SafeNormalize(float3(mainLight.direction) + float3(viewWS));
            float nh = saturate(dot(normalize(input.normalWS), halfDir));
            half glint = half(pow(nh, 2048.0) * 12.0);
            half sheen = half(pow(nh, 48.0)) * grime * 1.5h;
            half3 reflection = sky * coating * reflectance + sun * (glint * reflectance + sheen * 0.25h);

            // Grime is lit like a matte film on the glass; near the dot it also scatters some of its light.
            half nl = saturate(dot(normalWS, half3(mainLight.direction)));
            half3 haze = grime * (WindyAmbientSH(normalWS) * 0.25h + sun * nl * 0.08h);
            half scatter = grime * exp(-reticle.spread * 0.12h) * 0.6h;
            half dotAlpha = max(reticle.core, reticle.glow) * _DotColor.a * _ReflexOpacity;
            half3 dotLight = _DotColor.rgb * (_DotIntensity * WINDY_Exposure) * (dotAlpha + scatter * _ReflexOpacity);

            // Premultiplied: what the pane covers of the scene behind, then what it adds on top.
            half coverage = saturate(_LensColor.a + reflectance * 0.5h + grime * 0.12h);
            half alpha = coverage + reticle.core * _DotColor.a * _ReflexOpacity * (1.0h - coverage);
            half3 color = _LensColor.rgb * _LensColor.a + reflection + haze + dotLight;
            WINDY_APPLY_FOG_TO(input, color, unity_FogColor.rgb * alpha);
            return half4(color, alpha);
        }

        #else

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS : TEXCOORD0;
            float3 normalWS : TEXCOORD1;
            float3 positionVS : TEXCOORD2;
            float3 positionOS : TEXCOORD3;
            float3 normalOS : TEXCOORD4;
        };

        Varyings Vert(Attributes input)
        {
            Varyings output;
            output.positionWS = WINDY_ObjectToWorld(input.positionOS);
            output.positionCS = WINDY_WorldToHClip(output.positionWS);
            output.normalWS = WINDY_ObjectToWorldNormal(input.normalOS);
            output.positionVS = mul(UNITY_MATRIX_V, float4(output.positionWS, 1)).xyz;
            output.positionOS = input.positionOS;
            output.normalOS = input.normalOS;
            return output;
        }

        // Unlit approximation: no probe, the coating tint stands in for the reflected sky.
        half4 Frag(Varyings input) : SV_Target
        {
            ReticleSample reticle = SampleReticle(input.positionVS);
            half3 dirt = SampleDirt(input.positionOS, input.normalOS);
            half grime = saturate(dirt.r * 0.6h + dirt.g + dirt.b * 0.8h);
            half3 normalWS = half3(normalize(input.normalWS));
            half3 viewWS = half3(normalize(WINDY_ViewVector(input.positionWS)));
            half reflectance = LensReflectance(saturate(dot(normalWS, viewWS)));
            half dotAlpha = max(reticle.core, reticle.glow) * _DotColor.a * _ReflexOpacity;
            half coverage = saturate(_LensColor.a + reflectance * 0.5h + grime * 0.12h);
            half alpha = coverage + reticle.core * _DotColor.a * _ReflexOpacity * (1.0h - coverage);
            half3 color = (_LensColor.rgb * _LensColor.a + _CoatingColor.rgb * reflectance + grime * 0.05h) * WINDY_DeExposure
                + _DotColor.rgb * _DotIntensity * dotAlpha;
            return half4(color, alpha);
        }

        #endif
    ENDHLSL
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "UniversalForward" }
            Blend One OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Back
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fog
            ENDHLSL
        }
    }
    Fallback Off
}
