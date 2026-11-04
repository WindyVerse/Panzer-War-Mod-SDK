// Generated from ScopeLens.windyshader by RuntimeSupportExport. Do not edit.
Shader "WindyVerse/Weapon/ScopeLens"
{
    Properties
    {
        _LensColor ("Lens Tint (A = opacity head-on)", Color) = (0.03, 0.05, 0.06, 0.92)
        _CoatingColor ("Coating Reflection Tint (head-on)", Color) = (0.45, 0.85, 0.65, 1)
        _CoatingColor2 ("Coating Reflection Tint (oblique)", Color) = (0.6, 0.45, 0.85, 1)
        _Reflectance ("Head-on Reflectance", Range(0, 0.5)) = 0.12
        _Roughness ("Perceptual Roughness", Range(0, 1)) = 0.06
        _Curvature ("Element Curvature", Range(0, 1.5)) = 0.7
        _RimDarken ("Interior Darkening at the Cell", Range(0, 1)) = 0.65
        _InnerColorA ("Inner Reflection Tint (outer ring)", Color) = (1.0, 0.62, 0.28, 1)
        _InnerColorB ("Inner Reflection Tint (inner ring)", Color) = (0.4, 0.9, 0.7, 1)
        _InnerStrength ("Inner Reflection Strength", Range(0, 1)) = 0.1
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
#line 28 "Packages/com.shanghaiwindy.middlelayer/Shader/Common/ScopeLens.windyshader"
        CBUFFER_START(UnityPerMaterial)
            half4 _LensColor;
            half4 _CoatingColor;
            half4 _CoatingColor2;
            half _Reflectance;
            half _Roughness;
            half _Curvature;
            half _RimDarken;
            half4 _InnerColorA;
            half4 _InnerColorB;
            half _InnerStrength;
        CBUFFER_END

        struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float2 lensUV : TEXCOORD0;
        };

        // Schlick from the coating's head-on reflectance to a mirror at grazing angles.
        half LensReflectance(half nv)
        {
            half grazing = 1.0h - nv;
            half grazing2 = grazing * grazing;
            return lerp(_Reflectance, 1.0h, grazing2 * grazing2 * grazing);
        }

        // A convex element: the surface normal leans out toward the rim.
        half3 CurvedNormal(half3 normalWS, half3 tangentWS, half3 bitangentWS, float2 lensUV)
        {
            return normalize(normalWS + (tangentWS * lensUV.x + bitangentWS * lensUV.y) * _Curvature);
        }

        // Anti-reflection coatings look green head-on and turn violet toward grazing views.
        half3 CoatingTint(half nv, half radius)
        {
            half shift = saturate((1.0h - nv) * 1.8h + radius * radius * 0.3h);
            return lerp(_CoatingColor.rgb, _CoatingColor2.rgb, shift);
        }

        // The elements behind the front glass each throw back a faint ring of the scene; seen through the
        // front surface they sit at different depths, so they drift against each other as the view turns.
        half3 InnerReflections(float2 lensUV, half3 viewTS)
        {
            float2 drift = viewTS.xy / max(float(viewTS.z), 0.3);
            float2 outer = lensUV + drift * 0.35;
            float2 inner = lensUV - drift * 0.6;
            float2 glint = lensUV + drift * 0.15 + float2(0.3, -0.32);
            float outerRing = (length(outer) - 0.62) / 0.1;
            float innerRing = (length(inner) - 0.3) / 0.08;
            half3 rings = _InnerColorA.rgb * half(exp(-outerRing * outerRing))
                + _InnerColorB.rgb * half(exp(-innerRing * innerRing))
                + half3(1.0h, 1.0h, 1.0h) * half(exp(-dot(glint, glint) * 30.0) * 0.6);
            half inside = 1.0h - half(smoothstep(0.85, 1.0, length(lensUV)));
            return rings * inside;
        }

        // The dark interior behind the glass, falling off toward the cell that holds the element.
        half Vignette(half radius)
        {
            return 1.0h - _RimDarken * half(smoothstep(0.5, 1.0, radius));
        }

        #if defined(WINDY_URP)

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS : TEXCOORD0;
            float3 normalWS : TEXCOORD1;
            float4 tangentWS : TEXCOORD2;
            float2 lensUV : TEXCOORD3;
            WINDY_FOG_COORDS(4)
        };

        Varyings Vert(Attributes input)
        {
            Varyings output;
            output.positionWS = TransformObjectToWorld(input.positionOS);
            output.positionCS = TransformWorldToHClip(output.positionWS);
            output.normalWS = TransformObjectToWorldNormal(input.normalOS);
            output.tangentWS = float4(TransformObjectToWorldDir(input.tangentOS.xyz), input.tangentOS.w * GetOddNegativeScale());
            output.lensUV = input.lensUV;
            WINDY_TRANSFER_FOG(output, output.positionCS);
            return output;
        }

        half4 Frag(Varyings input) : SV_Target
        {
            half3 normalWS = half3(normalize(input.normalWS));
            half3 tangentWS = half3(SafeNormalize(input.tangentWS.xyz));
            half3 bitangentWS = cross(normalWS, tangentWS) * half(input.tangentWS.w);
            half3 viewWS = half3(GetWorldSpaceNormalizeViewDir(input.positionWS));
            half radius = half(saturate(length(input.lensUV)));

            half3 curvedWS = CurvedNormal(normalWS, tangentWS, bitangentWS, input.lensUV);
            half nv = saturate(dot(curvedWS, viewWS));
            half reflectance = LensReflectance(nv);
            half3 coating = lerp(CoatingTint(nv, radius), half3(1.0h, 1.0h, 1.0h), reflectance);

            // Sky / probe plus a sharp sun glint off the polished surface. The probe can be black (realtime
            // reflection gated off, no probe nearby), so the SH sky keeps a sheen.
            // Full precision: a 2048 exponent on a half dot product bands on mobile GPUs.
            half3 reflectWS = reflect(-viewWS, curvedWS);
            half3 sky = max(GlossyEnvironmentReflection(reflectWS, _Roughness, 1.0h), WindyAmbientSH(reflectWS));
            Light mainLight = GetMainLight();
            float3 halfDir = SafeNormalize(float3(mainLight.direction) + float3(viewWS));
            half glint = half(pow(saturate(dot(float3(curvedWS), halfDir)), 2048.0) * 16.0);
            half3 reflection = sky * coating + mainLight.color * (glint * mainLight.distanceAttenuation);

            // Light that crosses the glass: the dark interior lit only by the ambient, and the inner elements'
            // reflections of the scene in front of the glass, as bright as that scene.
            half3 ambient = WindyAmbientSH(normalWS);
            half3 viewTS = half3(dot(viewWS, tangentWS), dot(viewWS, bitangentWS), dot(viewWS, normalWS));
            half3 scene = WindyAmbientSH(viewWS);
            half3 interior = _LensColor.rgb * ambient * Vignette(radius)
                + InnerReflections(input.lensUV, viewTS) * scene * _InnerStrength;

            half transmitted = (1.0h - _LensColor.a) * (1.0h - reflectance);
            half alpha = 1.0h - transmitted;
            half3 color = interior * (_LensColor.a * (1.0h - reflectance)) + reflection * reflectance;

            // Premultiplied: the fog target is the fog colour at this fragment's coverage.
            WINDY_APPLY_FOG_TO(input, color, unity_FogColor.rgb * alpha);
            return half4(color, alpha);
        }

        #else

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS : TEXCOORD0;
            float3 normalWS : TEXCOORD1;
            float4 tangentWS : TEXCOORD2;
            float2 lensUV : TEXCOORD3;
        };

        Varyings Vert(Attributes input)
        {
            Varyings output;
            output.positionWS = WINDY_ObjectToWorld(input.positionOS);
            output.positionCS = WINDY_WorldToHClip(output.positionWS);
            output.normalWS = WINDY_ObjectToWorldNormal(input.normalOS);
            output.tangentWS = float4(mul((float3x3)UNITY_MATRIX_M, input.tangentOS.xyz), input.tangentOS.w);
            output.lensUV = input.lensUV;
            return output;
        }

        // Unlit approximation: no probe, the coating tints stand in for the reflected sky.
        half4 Frag(Varyings input) : SV_Target
        {
            half3 normalWS = half3(normalize(input.normalWS));
            half3 tangentWS = half3(normalize(input.tangentWS.xyz + 1e-6));
            half3 bitangentWS = cross(normalWS, tangentWS) * half(input.tangentWS.w);
            half3 viewWS = half3(normalize(WINDY_ViewVector(input.positionWS)));
            half radius = half(saturate(length(input.lensUV)));
            half3 curvedWS = CurvedNormal(normalWS, tangentWS, bitangentWS, input.lensUV);
            half nv = saturate(dot(curvedWS, viewWS));
            half reflectance = LensReflectance(nv);
            half3 viewTS = half3(dot(viewWS, tangentWS), dot(viewWS, bitangentWS), dot(viewWS, normalWS));
            half3 interior = _LensColor.rgb * Vignette(radius) + InnerReflections(input.lensUV, viewTS) * _InnerStrength * 0.5h;
            half alpha = 1.0h - (1.0h - _LensColor.a) * (1.0h - reflectance);
            half3 color = interior * (_LensColor.a * (1.0h - reflectance)) + CoatingTint(nv, radius) * reflectance;
            return half4(color * WINDY_DeExposure, alpha);
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
