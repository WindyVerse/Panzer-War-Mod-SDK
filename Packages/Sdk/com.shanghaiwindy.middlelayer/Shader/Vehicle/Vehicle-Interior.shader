// Generated from Vehicle-Interior.windyshader by RuntimeSupportExport. Do not edit.
Shader "WindyVerse/Vehicle/Interior"
{
    Properties
    {
        [MainTexture][NoScaleOffset]_BaseMap ("Base Map", 2D) = "white" {}
        [MainColor]_BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_BumpMap ("Normal Map", 2D) = "bump" {}
        [NoScaleOffset]_MaskMap ("Mask Map (R metallic, G occlusion, A smoothness)", 2D) = "white" {}
        _PointLightPosition ("Point Light Position", Vector) = (0, 0, 0, 0)
        _PointLightColor ("Point Light Color", Color) = (1, 1, 1, 0)
        _PointLightIntensity ("Point Light Intensity", Float) = 1
        _MaxLightDistance ("Max Light Distance", Float) = 1
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
#line 28 "Packages/com.shanghaiwindy.middlelayer/Shader/Vehicle/Vehicle-Interior.windyshader"
        #include "Packages/com.shanghaiwindy.middlelayer/Shader/Vehicle/Vehicle-Interior-Lighting.hlsl"

        TEXTURE2D(_BaseMap);    SAMPLER(sampler_BaseMap);
        TEXTURE2D(_BumpMap);    SAMPLER(sampler_BumpMap);
        TEXTURE2D(_MaskMap);    SAMPLER(sampler_MaskMap);

        CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            float4 _PointLightPosition;
            half4 _PointLightColor;
            float _PointLightIntensity;
            float _MaxLightDistance;
        CBUFFER_END

        struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
            float3 positionOS : TEXCOORD1;
            float3 normalOS : TEXCOORD2;
            float4 tangentOS : TEXCOORD3;
            WINDY_FOG_COORDS(4)
        };

        struct DepthVaryings
        {
            float4 positionCS : SV_POSITION;
        };

        Varyings Vert(Attributes input)
        {
            Varyings output;
            output.positionCS = WINDY_ObjectToHClip(input.positionOS);
            output.uv = input.uv;
            output.positionOS = input.positionOS;
            output.normalOS = input.normalOS;
            output.tangentOS = input.tangentOS;
            WINDY_TRANSFER_FOG(output, output.positionCS);
            return output;
        }

        half4 Frag(Varyings input) : SV_Target
        {
            half4 baseTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
            float3 albedo = float3((_BaseColor * baseTex).rgb);

            // The interior rig places the mesh so object space lines up with the
            // lighting inputs: light and fragment positions are object space while
            // the camera is world space, faithfully matching the original graph.
            float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, input.uv));
            float3 normalOS = normalize(input.normalOS);
            float3 tangentOS = normalize(input.tangentOS.xyz);
            float3 bitangentOS = cross(normalOS, tangentOS) * input.tangentOS.w;
            float3 normalObj = normalize(
                normalTS.x * tangentOS + normalTS.y * bitangentOS + normalTS.z * normalOS);

            float4 mask = float4(SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, input.uv));

            float3 lit;
            SimplePointLight_float(albedo, mask, _PointLightPosition.xyz, float3(_PointLightColor.rgb),
                _PointLightIntensity, _MaxLightDistance, input.positionOS, normalObj,
                _WorldSpaceCameraPos.xyz, lit);

            // Artist-tuned interior light, not scene radiance: like the unlit base
            // colour in VehicleLight it rides de-exposure, so the interior keeps its
            // authored brightness instead of sinking under daylight exposure.
            half3 color = half3(lit) * WINDY_DeExposure;
            WINDY_APPLY_FOG(input, color);
            return half4(color, half(1.0));
        }

        DepthVaryings VertDepthOnly(Attributes input)
        {
            DepthVaryings output;
            output.positionCS = WINDY_ObjectToHClip(input.positionOS);
            return output;
        }

        DepthVaryings VertShadowCaster(Attributes input)
        {
            DepthVaryings output;
            output.positionCS = WindyShadowCasterPositionCS(input.positionOS, input.normalOS);
            return output;
        }

        half4 FragDepth(DepthVaryings input) : SV_Target
        {
            return 0;
        }
    ENDHLSL
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }

        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fog
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex VertShadowCaster
            #pragma fragment FragDepth

            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }

            ZWrite On
            ColorMask R
            Cull Back

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex VertDepthOnly
            #pragma fragment FragDepth
            ENDHLSL
        }
    }
    Fallback "Universal Render Pipeline/Unlit"
}
