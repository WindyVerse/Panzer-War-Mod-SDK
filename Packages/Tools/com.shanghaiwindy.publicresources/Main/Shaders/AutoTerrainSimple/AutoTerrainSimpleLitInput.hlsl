#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
    // Simple Lit 必带参数
    float4 _BaseMap_ST;
    half4 _BaseColor;
    half _Cutoff;
    half _Surface;
    
    // 自定义参数
    float4 _SandMap_ST;
    float4 _CliffMap_ST;
    float4 _RoadMap_ST;
    float4 _NoiseMask_ST;
    float4 _CliffMask_ST;
    float _RoadMask_ST;
    half _Slope;
CBUFFER_END

TEXTURE2D(_SandMap);       SAMPLER(sampler_SandMap);
TEXTURE2D(_CliffMap);      SAMPLER(sampler_CliffMap);
TEXTURE2D(_RoadMap);      SAMPLER(sampler_RoadMap);

TEXTURE2D(_NoiseMask);     SAMPLER(sampler_NoiseMask);
TEXTURE2D(_CliffMask);      SAMPLER(sampler_CliffMask);
TEXTURE2D(_RoadMask);      SAMPLER(sampler_RoadMask);
