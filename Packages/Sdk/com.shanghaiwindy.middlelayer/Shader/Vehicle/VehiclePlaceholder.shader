// Generated from VehiclePlaceholder.windyshader by RuntimeSupportExport. Do not edit.
Shader "WindyVerse/Vehicle/Placeholder"
{
    Properties
    {
        [MainTexture][NoScaleOffset]_BaseMap("Base Map", 2D) = "white" {}
        [MainColor]_BaseColor("Base Color", Color) = (0.8, 0.8, 0.8, 0)
        // Declared, never sampled: they exist so the packed material keeps the
        // references alive for the bundle and for GetTexturePropertyNames at load.
        [NoScaleOffset]_BumpMap("Normal Map", 2D) = "bump" {}
        [NoScaleOffset]_MaskMap("Mask Map (R metallic, G occlusion, A smoothness)", 2D) = "white" {}
    }
    HLSLINCLUDE
#line 25 "Packages/com.shanghaiwindy.middlelayer/Shader/Vehicle/VehiclePlaceholder.windyshader"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #define WINDY_ObjectToHClip(positionOS) TransformObjectToHClip(positionOS)
#line 34 "Packages/com.shanghaiwindy.middlelayer/Shader/Vehicle/VehiclePlaceholder.windyshader"
        TEXTURE2D(_BaseMap);
        SAMPLER(sampler_BaseMap);

        CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
        CBUFFER_END

        struct Attributes
        {
            float3 positionOS : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        Varyings VertPlaceholder(Attributes input)
        {
            Varyings output;
            output.positionCS = WINDY_ObjectToHClip(input.positionOS);
            output.uv = input.uv;
            return output;
        }

        half4 FragPlaceholder(Varyings input) : SV_Target
        {
            return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
        }
    ENDHLSL
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "IgnoreProjector" = "True" }
        LOD 100

        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex VertPlaceholder
            #pragma fragment FragPlaceholder
            ENDHLSL
        }
    }
    Fallback Off
}
