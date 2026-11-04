#ifndef PI
#define PI 3.14159265
#endif

float DistributionGGX(float3 N, float3 H, float roughness)
{
    float a = roughness * roughness;
    float a2 = a * a;
    float NdotH = max(dot(N, H), 0.0);
    float NdotH2 = NdotH * NdotH;

    float denom = NdotH2 * (a2 - 1.0) + 1.0;
    return a2 / (PI * denom * denom);
}

float GeometrySchlickGGX(float NdotV, float roughness)
{
    float r = roughness + 1.0;
    float k = (r * r) / 8.0;
    return NdotV / (NdotV * (1.0 - k) + k);
}

float GeometrySmith(float3 N, float3 V, float3 L, float roughness)
{
    float NdotV = max(dot(N, V), 0.0);
    float NdotL = max(dot(N, L), 0.0);
    float ggx1 = GeometrySchlickGGX(NdotV, roughness);
    float ggx2 = GeometrySchlickGGX(NdotL, roughness);
    return ggx1 * ggx2;
}

float3 FresnelSchlick(float cosTheta, float3 F0)
{
    return F0 + (1.0 - F0) * pow(1.0 - cosTheta, 5.0);
}

void SimplePointLight_float(
    float3 BaseColor,
    float4 MaskMap,
    float3 LightPosition,
    float3 LightColor,
    float LightIntensity,
    float LightMaxDistance,
    float3 ObjectPosition,
    float3 ObjectNormal,
    float3 CameraPosition,
    out float3 Albedo
)
{
    float metallic = MaskMap.r;
    float smoothness = MaskMap.a;
    float ao = MaskMap.g;
    float roughness = 1.0 - smoothness;

    float3 N = normalize(ObjectNormal);
    float3 V = normalize(CameraPosition - ObjectPosition);
    float3 L = normalize(LightPosition - ObjectPosition);
    float3 H = normalize(L + V);

    float NdotL = max(dot(N, L), 0.0);
    float NdotV = max(dot(N, V), 0.0);
    float NdotH = max(dot(N, H), 0.0);
    float VdotH = max(dot(V, H), 0.0);

    float distance = length(LightPosition - ObjectPosition);
    float attenuation = LightIntensity / (distance * distance + 0.1);
    float smoothAtten = attenuation * saturate(1.0 - distance / LightMaxDistance);

    float3 F0 = lerp(float3(0.04, 0.04, 0.04), BaseColor, metallic);

    float D = DistributionGGX(N, H, roughness);
    float G = GeometrySmith(N, V, L, roughness);
    float3 F = FresnelSchlick(VdotH, F0);

    float3 nominator = D * G * F;
    float denominator = 4.0 * NdotV * NdotL + 0.001;
    float3 specular = nominator / denominator;

    float3 kS = F;
    float3 kD = 1.0 - kS;
    kD *= 1.0 - metallic;

    float3 diffuse = BaseColor / PI;

    float3 color = (kD * diffuse + specular) * LightColor * NdotL;
    color *= smoothAtten * ao;

    Albedo = color;
}
