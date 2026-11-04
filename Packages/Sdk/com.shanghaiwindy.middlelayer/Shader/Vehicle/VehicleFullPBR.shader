// Generated from VehicleFullPBR.windyshader by RuntimeSupportExport. Do not edit.
Shader "WindyVerse/Vehicle/FullPBR"
{
    Properties
    {
        [MainTexture][NoScaleOffset]_BaseMap("Base Map", 2D) = "white" {}
        [MainColor]_BaseColor("Base Color", Color) = (0.8, 0.8, 0.8, 0)
        [NoScaleOffset]_BumpMap("Normal Map", 2D) = "bump" {}
        [NoScaleOffset]_MaskMap("Mask Map (R metallic, G occlusion, A smoothness)", 2D) = "white" {}
        _DefaultSmooth("Default Smoothness", Float) = 0.3

        [NoScaleOffset]_CamoPatternMap("Camo Pattern Map", 2D) = "white" {}
        [NoScaleOffset]_CamoColorTex("Camo Color Texture", 2D) = "white" {}
        _CamoPatternTiling("Camo Pattern Tiling", Vector) = (2, 2, 0, 0)
        _CamoBlackTint("Camo Black Tint", Color) = (0.7830189, 0.7830189, 0.7830189, 0)
        _CamoRedTint("Camo Red Tint", Color) = (0.735849, 0, 0, 0)
        _CamoGreenTint("Camo Green Tint", Color) = (0.5559685, 0.7924528, 0, 0)
        _CamoBlueTint("Camo Blue Tint", Color) = (0.3632075, 1, 0.9239099, 0)
        _Lerp("Camo Intensity", Float) = 0.5

        [NoScaleOffset]_DirtTex("Dirt Texture", 2D) = "white" {}
        _DirtRange("Dirt Range", Float) = 0

        _TrackOffset("Track UV Offset", Vector) = (0, 0, 0, 0)

        // Written by CamouflageData / VehicleCamoManager, never read here: the
        // keywords carry the same state. Kept so the C# write path stays valid
        // and so the keywords can later collapse into uniform branches.
        _EnableCamo("Camo Enabled (unused)", Float) = 0
        _BoolEnableCamoColorTex("Camo Color Texture Enabled (unused)", Float) = 0
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
#line 36 "Packages/com.shanghaiwindy.middlelayer/Shader/Vehicle/VehicleSurface.windyshaderinc"
        struct WindyLight
        {
            half3 color;
            half3 direction;
            half attenuation;
        };

        WindyLight WindyGetMainLight()
        {
            Light mainLight = GetMainLight();

            WindyLight light;
            light.color = mainLight.color;
            light.direction = mainLight.direction;
            light.attenuation = mainLight.distanceAttenuation;
            return light;
        }

        half3 WindySampleSH(half3 normalWS)
        {
            return WindyAmbientSH(normalWS);
        }

        // Mid tier reads the probe SH as a low-frequency environment, so roughness
        // has nothing to select and is ignored here.
        half3 WindySampleEnvSpecular(half3 reflectDirWS, half perceptualRoughness)
        {
            return WindyAmbientSH(reflectDirWS);
        }

        half WindyFogFactor(float positionCSZ)
        {
            return half(ComputeFogFactor(positionCSZ));
        }

        half3 WindyMixFog(half3 color, half fogFactor)
        {
            return MixFog(color, fogFactor);
        }

        // Last-resort backstop, same as the HDRP exit: a bad environment sample or
        // an fp16 overflow on a phone GPU must not survive into bloom.
        half3 WindyGuardOutput(half3 color)
        {
            color = min(color, half(HALF_MAX));
            return AnyIsNaN(float3(color)) ? half3(0.0, 0.0, 0.0) : color;
        }

        float4 WindyShadowPositionCS(float3 positionOS, float3 normalOS)
        {
            float3 positionWS = TransformObjectToWorld(positionOS);
            float3 normalWS = TransformObjectToWorldNormal(normalOS);

            #if _CASTING_PUNCTUAL_LIGHT_SHADOW
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
#line 16 "Packages/com.shanghaiwindy.middlelayer/Shader/Vehicle/VehicleFullPBR.windyshader"
        #define VEHICLE_TIER_FULL_PBR 1
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
#line 191 "Packages/com.shanghaiwindy.middlelayer/Shader/Vehicle/VehicleSurface.windyshaderinc"
        // Each shell names its tier in its own %%hlsl.<pipeline> block, which the
        // importer emits ahead of this one. UnlitFast names none and takes the
        // defaults, so its variants never compile the PBR entry points.
        #ifndef VEHICLE_TIER_SIMPLE_PBR
        #define VEHICLE_TIER_SIMPLE_PBR 0
        #endif
        #ifndef VEHICLE_TIER_FULL_PBR
        #define VEHICLE_TIER_FULL_PBR 0
        #endif

        // Both PBR tiers read metallic/occlusion/smoothness every pixel; UnlitFast
        // only needs the mask for the dirt breakup and skips the fetch otherwise.
        #ifndef VEHICLE_SURFACE_USE_MASK
        #define VEHICLE_SURFACE_USE_MASK (VEHICLE_TIER_SIMPLE_PBR || VEHICLE_TIER_FULL_PBR)
        #endif

        // Tokuyoshi/Kaplanyan specular AA tuning, matching HDRP's Lit defaults.
        #define VEHICLE_SPEC_AA_VARIANCE 0.1
        #define VEHICLE_SPEC_AA_THRESHOLD 0.2

        TEXTURE2D(_BaseMap);        SAMPLER(sampler_BaseMap);
        TEXTURE2D(_BumpMap);        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_MaskMap);        SAMPLER(sampler_MaskMap);
        TEXTURE2D(_CamoPatternMap); SAMPLER(sampler_CamoPatternMap);
        TEXTURE2D(_CamoColorTex);   SAMPLER(sampler_CamoColorTex);
        TEXTURE2D(_DirtTex);        SAMPLER(sampler_DirtTex);

        CBUFFER_START(UnityPerMaterial)
            float4 _CamoPatternTiling;
            float4 _TrackOffset;
            half4 _BaseColor;
            half4 _CamoBlackTint;
            half4 _CamoRedTint;
            half4 _CamoGreenTint;
            half4 _CamoBlueTint;
            half _DefaultSmooth;
            half _Lerp;
            half _DirtRange;
            half _EnableCamo;
            half _BoolEnableCamoColorTex;
            #if VEHICLE_TIER_FULL_PBR && !defined(WINDY_URP)
                // HDRP's Material.hlsl reads these two out of the material buffer, so
                // the top tier has to carry them to stay SRP-batcher compatible. Both
                // only matter for transparent blending; zero is the opaque answer.
                float _BlendMode;
                float _EnableBlendModePreserveSpecularLighting;
            #endif
        CBUFFER_END

        // Same value as URP's kDielectricSpec, restated so the shared shading path
        // does not depend on a URP-only symbol.
        static const half4 WINDY_DIELECTRIC_SPEC = half4(0.04, 0.04, 0.04, 1.0 - 0.04);

        // Dirt tiling is a graph constant, not a property.
        static const float2 VEHICLE_DIRT_TILING = float2(5.0, 5.0);

        // ---- Paint and weathering tuning ---------------------------------------
        // Everything below reads the mask map that is already fetched: no extra
        // sample, no extra material property. Values are the knobs; the behaviour
        // they drive is described where each one is used.
        static const half VEHICLE_PAINT_SMOOTHNESS = half(0.45);   // sheen of fresh paint
        static const half VEHICLE_PAINT_DETAIL_PIVOT = half(0.45); // base luminance that maps to 1x
        static const half VEHICLE_PAINT_DETAIL_MIN = half(0.35);   // how dark a seam may push the paint
        static const half VEHICLE_PAINT_DETAIL_MAX = half(1.8);    // stops white decals turning fluorescent
        // Tuned against the shipped default Mask.png, whose occlusion channel only
        // spans 0.86..1.0 (measured). A 0.75 threshold would flake almost the whole
        // hull; starting at 0.85 spends the curve on the contrast that actually exists,
        // and an authored full-range mask then wears only its genuinely exposed edges.
        static const half VEHICLE_WEAR_START = half(0.85);         // AO level where flaking begins
        static const half VEHICLE_WEAR_MAX = half(0.55);           // cap so a flat white mask cannot strip all paint
        static const half VEHICLE_DIRT_SMOOTHNESS_VALUE = half(0.08); // caked dirt is matte
        // Same reason: 1 - occlusion tops out near 0.14 on the default mask, so unit
        // weight would make crevice dirt invisible. Over-weighting only means an
        // authored mask saturates its crevices sooner, which is the intended look.
        static const half VEHICLE_DIRT_CAVITY_WEIGHT = half(1.2);  // crevice packing
        static const half VEHICLE_DIRT_DUST_WEIGHT = half(0.5);    // settling on upward faces
        static const half VEHICLE_DIRT_GROWTH_SOFT = half(0.35);   // softness of the spreading front
        static const half VEHICLE_DIRT_GATE_MIN = half(0.1);       // sweep floor: zero-potential faces stay clean at full range

        struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float2 uv : TEXCOORD0;
            #ifdef _ENABLE_NORMAL
                float4 tangentOS : TANGENT;
            #endif
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
            half3 normalWS : TEXCOORD1;
            half3 vertexSH : TEXCOORD2;
            half fogFactor : TEXCOORD3;
        };

        // Fog rides in normalWSAndFog.w to keep the lit tiers inside the interpolator
        // budget once the tangent frame joins.
        struct PBRVaryings
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
            half4 normalWSAndFog : TEXCOORD1;
            float3 positionWS : TEXCOORD2;
            #ifdef _ENABLE_NORMAL
                half4 tangentWS : TEXCOORD3;
            #endif
            #if VEHICLE_TIER_SIMPLE_PBR
                half3 vertexSH : TEXCOORD4;
            #endif
        };

        struct DepthVaryings
        {
            float4 positionCS : SV_POSITION;
        };

        struct VehicleSurface
        {
            half3 albedo;
            half3 normalTS;
            half metallic;
            half occlusion;
            half smoothness;
        };

        // Top tier only: the cheaper tiers exist not to pay for this, so they do not
        // compile it in at all. Plain #include, not %%include, which would emit ahead
        // of the surface struct it takes apart.
        #if VEHICLE_TIER_FULL_PBR
            #include "Packages/com.shanghaiwindy.middlelayer/Shader/Weather/WindySurfaceWeather.hlsl"
        #endif

        // Named after its tier like every other entry point here: HDRP's shader passes
        // declare their own Vert, and an overload set would make the entry ambiguous.
        Varyings VertUnlitFast(Attributes input)
        {
            Varyings output;
            half3 normalWS = half3(TransformObjectToWorldNormal(input.normalOS));

            output.positionCS = WINDY_ObjectToHClip(input.positionOS);
            output.uv = input.uv;
            output.normalWS = normalWS;
            // On URP, SampleSHVertex collapses to zero unless EVALUATE_SH_VERTEX is
            // on the pass, which costs a variant pair. Evaluate the probe outright.
            output.vertexSH = WindySampleSH(normalWS);
            output.fogFactor = WindyFogFactor(output.positionCS.z);
            return output;
        }

        // normalWS is the geometric normal, used only by the directional dirt build-up.
        VehicleSurface AssembleVehicleSurface(float2 uv, half3 normalWS)
        {
            // Track scroll rides the base UV only; camo and dirt stay pinned to the mesh.
            float2 uvBase = uv + _TrackOffset.xy;
            half3 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uvBase).rgb;

            // Fetched before the camo branch: paint coverage now rewrites metallic and
            // smoothness, and the wear term reads the same occlusion channel.
            // _DefaultSmooth is an unbounded Float property; the rest of the shading
            // assumes smoothness in [0,1] to keep roughness and the sky mip in range.
            half4 mask = half4(0.0, 1.0, 0.0, saturate(_DefaultSmooth));
            #if VEHICLE_SURFACE_USE_MASK || defined(_KEYWORDENABLEDIRT)
                mask = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, uvBase);
            #endif

            #ifdef _KEYWORDENABLEDIRT
                // Clamped at the source: pow() of a negative base is NaN, and
                // _DirtRange comes straight from C# where a mod or a corrupt save
                // can hand us anything. Legal values are already inside [0,1].
                half dirtRange = saturate(_DirtRange);
            #endif

            #ifdef _KEYWORD_ENABLE_CAMO
                float2 uvCamo = uv * _CamoPatternTiling.xy;
                #ifdef _KEYWORD_ENABLE_CAMO_COLOR_TEX
                    half3 camoColor = SAMPLE_TEXTURE2D(_CamoColorTex, sampler_CamoColorTex, uvCamo).rgb;
                    half paintCoverage = _Lerp;
                #else
                    // Pattern channels stack black -> red -> green -> blue, so a texel
                    // lit in several channels lands on the last one that claims it.
                    half4 pattern = SAMPLE_TEXTURE2D(_CamoPatternMap, sampler_CamoPatternMap, uvCamo);
                    half4 camo = lerp(lerp(lerp(_CamoBlackTint, _CamoRedTint, pattern.r),
                                           _CamoGreenTint, pattern.g),
                                      _CamoBlueTint, pattern.b);
                    half3 camoColor = camo.rgb;
                    half paintCoverage = camo.a * _Lerp;
                #endif

                #ifdef _KEYWORDENABLEDIRT
                    // Bright occlusion means the surface stands proud, which is exactly
                    // where paint gets rubbed away. Capped, because a flat white mask
                    // reads as "everything exposed" and would otherwise strip the lot.
                    half wear = smoothstep(VEHICLE_WEAR_START, half(1.0), mask.g) *
                        dirtRange * VEHICLE_WEAR_MAX;
                    paintCoverage *= half(1.0) - wear;
                #endif

                // Paint is a thin coat, not a repaint: modulate it by the base map's own
                // luminance so seams and rivets keep their shading. Clamped both ways so
                // white markings cannot push the tint fluorescent.
                half detail = clamp(half(Luminance(albedo)) / VEHICLE_PAINT_DETAIL_PIVOT,
                    VEHICLE_PAINT_DETAIL_MIN, VEHICLE_PAINT_DETAIL_MAX);
                albedo = lerp(albedo, camoColor * detail, paintCoverage);

                #if VEHICLE_SURFACE_USE_MASK
                    // Painted steel is a dielectric. Where the wear term brought coverage
                    // back down, metallic and smoothness return to the bare substrate on
                    // their own, so chipped edges read as exposed metal for free.
                    mask.r = lerp(mask.r, half(0.0), paintCoverage);
                    mask.a = lerp(mask.a, VEHICLE_PAINT_SMOOTHNESS, paintCoverage);
                #endif
            #endif

            #ifdef _KEYWORDENABLEDIRT
                half3 dirt = SAMPLE_TEXTURE2D(_DirtTex, sampler_DirtTex, uv * VEHICLE_DIRT_TILING).rgb;

                // Crevices pack dirt, upward faces collect dust. A flat white mask zeroes
                // the cavity term and leaves the dust term, which still reads correctly
                // instead of banding at one mask brightness the way v1 did.
                half dirtPotential = saturate(
                    VEHICLE_DIRT_CAVITY_WEIGHT * (half(1.0) - mask.g) +
                    VEHICLE_DIRT_DUST_WEIGHT * saturate(normalWS.y));

                // The slider sweeps a gate down through the potential field, so grime
                // grows outward from the deepest pockets instead of fading in globally.
                // The sweep floors at GATE_MIN: even at full range, faces with no
                // accumulation potential (clean vertical armour) never go full mud.
                half gate = lerp(half(1.0), VEHICLE_DIRT_GATE_MIN, dirtRange);
                half dirtBlend = smoothstep(gate, gate + VEHICLE_DIRT_GROWTH_SOFT, dirtPotential);
                albedo = lerp(albedo, dirt, dirtBlend);

                #if VEHICLE_SURFACE_USE_MASK
                    mask.a = lerp(mask.a, VEHICLE_DIRT_SMOOTHNESS_VALUE, dirtBlend);
                #endif
            #endif

            VehicleSurface surface;
            surface.albedo = saturate(_BaseColor.rgb * albedo);
            #ifdef _ENABLE_NORMAL
                surface.normalTS = half3(UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uvBase)));
            #else
                surface.normalTS = half3(0.0, 0.0, 1.0);
            #endif
            surface.metallic = saturate(mask.r);
            surface.occlusion = saturate(mask.g);
            // Roughness, the sky mip and the Lazarov fit all assume [0,1]. The mask
            // can be a workshop-supplied texture, so pin the range once here rather
            // than trusting every upload to be UNORM.
            surface.smoothness = saturate(mask.a);
            return surface;
        }

        half3 ShadeUnlitFast(half3 albedo, half3 normalWS, half3 vertexSH)
        {
            WindyLight mainLight = WindyGetMainLight();

            // Squared half-lambert: no normal map on this tier, so the terminator
            // has to carry all of the shape reading and a raw NdotL is too hard.
            half halfLambert = dot(normalWS, mainLight.direction) * half(0.5) + half(0.5);
            half diffuse = halfLambert * halfLambert * mainLight.attenuation;

            return albedo * (vertexSH + mainLight.color * diffuse);
        }

        half4 FragUnlitFast(Varyings input) : SV_Target
        {
            half3 normalWS = half3(SafeNormalize(float3(input.normalWS)));
            VehicleSurface surface = AssembleVehicleSurface(input.uv, normalWS);
            half3 color = ShadeUnlitFast(surface.albedo, normalWS, input.vertexSH);
            return half4(WindyGuardOutput(WindyMixFog(color, input.fogFactor) * WINDY_Exposure), 1.0);
        }

        PBRVaryings VertPBR(Attributes input)
        {
            PBRVaryings output;
            float3 positionWS = TransformObjectToWorld(input.positionOS);
            half3 normalWS = half3(TransformObjectToWorldNormal(input.normalOS));

            output.positionCS = TransformWorldToHClip(positionWS);
            output.uv = input.uv;
            output.positionWS = positionWS;
            output.normalWSAndFog = half4(normalWS, WindyFogFactor(output.positionCS.z));

            #ifdef _ENABLE_NORMAL
                // Bitangent is rebuilt in the fragment, so only the sign travels.
                output.tangentWS = half4(half3(TransformObjectToWorldDir(input.tangentOS.xyz)),
                    half(input.tangentOS.w) * half(GetOddNegativeScale()));
            #endif

            #if VEHICLE_TIER_SIMPLE_PBR
                output.vertexSH = WindySampleSH(normalWS);
            #endif

            return output;
        }

        // Perturbs the already-normalized geometric normal by the tangent-space map.
        // SafeNormalize throughout: mod vehicles ship degenerate triangles and zero
        // tangents, and plain normalize turns those into NaN that then spreads through
        // the whole frame via bloom and TAA history.
        half3 ResolveVehicleNormalWS(PBRVaryings input, half3 normalTS, half3 geometricNormalWS)
        {
            #ifdef _ENABLE_NORMAL
                half3 tangentWS = half3(SafeNormalize(float3(input.tangentWS.xyz)));
                half3 bitangentWS = cross(geometricNormalWS, tangentWS) * input.tangentWS.w;
                half3x3 tangentToWorld = half3x3(tangentWS, bitangentWS, geometricNormalWS);
                return half3(SafeNormalize(float3(TransformTangentToWorld(normalTS, tangentToWorld))));
            #else
                return geometricNormalWS;
            #endif
        }

        #if VEHICLE_TIER_SIMPLE_PBR || VEHICLE_TIER_FULL_PBR
        // Lazarov's analytic DFG fit (Siggraph 2013 "Getting More Physical in Call of
        // Duty: Black Ops II") in Karis' two-constant form, standing in for a BRDF LUT.
        half2 EnvBRDFApprox(half perceptualRoughness, half NoV)
        {
            const half4 c0 = half4(-1.0, -0.0275, -0.572, 0.022);
            const half4 c1 = half4(1.0, 0.0425, 1.04, -0.04);
            half4 r = perceptualRoughness * c0 + c1;
            half a004 = min(r.x * r.x, exp2(half(-9.28) * NoV)) * r.x + r.y;
            return half2(-1.04, 1.04) * a004 + r.zw;
        }

        // Transcribed from URP's DirectBRDFSpecular, including the parts that look
        // like noise but are not. The half-vector dot and the squared denominator
        // stay in float: at maximum smoothness roughness^2 bottoms out at HALF_MIN
        // and d collapses to it, so d*d lands near 1e-9 and flushes to zero in fp16,
        // making the reciprocal INF and every downstream sum NaN. The 1.00001
        // epsilon and the REAL_IS_HALF clamp guard the same failure from the other
        // end (overflow). Do not "simplify" this back into a single half rcp.
        half WindyDirectSpecularTerm(half roughness, half roughness2, half3 normalWS,
            half3 lightDirWS, half3 viewDirWS)
        {
            float3 lightDirFloat = float3(lightDirWS);
            float3 halfDir = SafeNormalize(lightDirFloat + float3(viewDirWS));

            float NoH = saturate(dot(float3(normalWS), halfDir));
            half LoH = half(saturate(dot(lightDirFloat, halfDir)));

            half roughness2MinusOne = roughness2 - half(1.0);
            half normalizationTerm = roughness * half(4.0) + half(2.0);

            float d = NoH * NoH * roughness2MinusOne + 1.00001f;
            half LoH2 = LoH * LoH;
            half specularTerm = half(roughness2 / ((d * d) * max(half(0.1), LoH2) * normalizationTerm));

            #if REAL_IS_HALF
                specularTerm = specularTerm - HALF_MIN;
                specularTerm = clamp(specularTerm, 0.0, 1000.0);
            #endif

            return specularTerm;
        }

        // Mid-tier shading, and the whole PBR path on HDRP where URP's BRDF library
        // does not exist. bakedGI is the indirect diffuse the caller already resolved.
        half3 ShadeSimplePBR(VehicleSurface surface, half3 normalWS, half3 viewDirWS, half3 bakedGI)
        {
            half perceptualRoughness = half(1.0) - surface.smoothness;
            half roughness = max(perceptualRoughness * perceptualRoughness, HALF_MIN_SQRT);
            half roughness2 = max(roughness * roughness, HALF_MIN);

            half oneMinusReflectivity = WINDY_DIELECTRIC_SPEC.a - surface.metallic * WINDY_DIELECTRIC_SPEC.a;
            half3 diffuseColor = surface.albedo * oneMinusReflectivity;
            half3 specularColor = lerp(WINDY_DIELECTRIC_SPEC.rgb, surface.albedo, surface.metallic);

            WindyLight mainLight = WindyGetMainLight();
            half NoL = saturate(dot(normalWS, mainLight.direction));
            half specularTerm = WindyDirectSpecularTerm(roughness, roughness2, normalWS,
                mainLight.direction, viewDirWS);

            half3 direct = (diffuseColor + specularColor * specularTerm) *
                (mainLight.color * (NoL * mainLight.attenuation));

            half NoV = saturate(dot(normalWS, viewDirWS));
            half3 envSpecular = WindySampleEnvSpecular(reflect(-viewDirWS, normalWS), perceptualRoughness);
            half2 envBRDF = EnvBRDFApprox(perceptualRoughness, NoV);
            half3 indirect = bakedGI * diffuseColor + envSpecular * (specularColor * envBRDF.x + envBRDF.y);

            return direct + indirect * surface.occlusion;
        }
        #endif

        #if VEHICLE_TIER_SIMPLE_PBR
        half4 FragSimplePBR(PBRVaryings input) : SV_Target
        {
            half3 geometricNormalWS = half3(SafeNormalize(float3(input.normalWSAndFog.xyz)));
            VehicleSurface surface = AssembleVehicleSurface(input.uv, geometricNormalWS);
            half3 normalWS = ResolveVehicleNormalWS(input, surface.normalTS, geometricNormalWS);
            half3 viewDirWS = half3(GetWorldSpaceNormalizeViewDir(input.positionWS));

            half3 color = ShadeSimplePBR(surface, normalWS, viewDirWS, input.vertexSH);
            return half4(WindyGuardOutput(WindyMixFog(color, input.normalWSAndFog.w) * WINDY_Exposure), 1.0);
        }
        #endif

        #if VEHICLE_TIER_FULL_PBR
        half3 ShadeFullPBR(VehicleSurface surface, half3 normalWS, half3 viewDirWS,
            half3 geometricNormalWS, float3 positionWS)
        {
            // Curvature packed into one pixel widens the effective NDF; without this
            // the hull edges strobe as the camera pulls back.
            half smoothness = half(GeometricNormalFiltering(surface.smoothness, geometricNormalWS,
                VEHICLE_SPEC_AA_VARIANCE, VEHICLE_SPEC_AA_THRESHOLD));

            #ifdef WINDY_URP
                // Deliberately routed through URP's own BRDFData rather than a local
                // copy of the maths, so vehicles and URP Lit map props share one solution.
                half alpha = half(1.0);
                BRDFData brdfData;
                InitializeBRDFData(surface.albedo, surface.metallic, half3(0.0, 0.0, 0.0),
                    smoothness, alpha, brdfData);

                // Shadow coord costs no interpolator: positionWS is already carried for
                // the view vector. LightingPhysicallyBased folds shadowAttenuation into
                // the radiance itself, so the BRDF maths is untouched.
                //
                // The positionWS overload, never the bare shadowCoord one. When the sun's
                // shadows are switched off URP does not drop _MAIN_LIGHT_SHADOWS: it binds
                // an uncleared 1x1 shadowmap, leaves _MainLightWorldToShadow stale and sets
                // strength to 1, then relies on the distance fade (_ShadowParams.z=1, w=0)
                // to force attenuation back to 1. Only this overload applies that fade.
                // MainLightRealtimeShadow alone samples the garbage and kills the sun.
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(positionWS), positionWS,
                    half4(1.0, 1.0, 1.0, 1.0));
                half3 color = LightingPhysicallyBased(brdfData, mainLight, normalWS, viewDirWS);

                // The no-positionWS overload takes the single unity_SpecCube0 path: no probe
                // blending, no box projection, so neither keyword can inflate the variant set.
                const BRDFData noClearCoat = (BRDFData)0;
                color += GlobalIllumination(brdfData, noClearCoat, 0.0, WindySampleSH(normalWS),
                    surface.occlusion, normalWS, viewDirWS);

                return color;
            #else
                // HDRP has no equivalent BRDF library to borrow, and a custom forward
                // pass sees only the sky as environment, so both PBR tiers converge on
                // the hand-written path here. Per-pixel probe is what separates them.
                VehicleSurface filtered = surface;
                filtered.smoothness = smoothness;
                return ShadeSimplePBR(filtered, normalWS, viewDirWS, WindySampleSH(normalWS));
            #endif
        }

        half4 FragFullPBR(PBRVaryings input) : SV_Target
        {
            half3 geometricNormalWS = half3(SafeNormalize(float3(input.normalWSAndFog.xyz)));
            VehicleSurface surface = AssembleVehicleSurface(input.uv, geometricNormalWS);
            // Before the tangent-space normal is resolved: the weather layer flattens it.
            ApplyWindySurfaceWeather(surface.albedo, surface.normalTS, surface.metallic,
                surface.occlusion, surface.smoothness, input.uv, geometricNormalWS,
                TransformWorldToObject(input.positionWS).xz,
                half(distance(input.positionWS, _WorldSpaceCameraPos)));
            half3 normalWS = ResolveVehicleNormalWS(input, surface.normalTS, geometricNormalWS);
            half3 viewDirWS = half3(GetWorldSpaceNormalizeViewDir(input.positionWS));

            half3 color = ShadeFullPBR(surface, normalWS, viewDirWS, geometricNormalWS, input.positionWS);
            return half4(WindyGuardOutput(WindyMixFog(color, input.normalWSAndFog.w) * WINDY_Exposure), 1.0);
        }
        #endif

        DepthVaryings VertDepthOnly(Attributes input)
        {
            DepthVaryings output;
            output.positionCS = WINDY_ObjectToHClip(input.positionOS);
            return output;
        }

        DepthVaryings VertShadowCaster(Attributes input)
        {
            DepthVaryings output;
            output.positionCS = WindyShadowPositionCS(input.positionOS, input.normalOS);
            return output;
        }

        half4 FragDepth(DepthVaryings input) : SV_Target
        {
            return 0;
        }

        struct NormalsVaryings
        {
            float4 positionCS : SV_POSITION;
            half3 normalWS : TEXCOORD0;
        };

        NormalsVaryings VertDepthNormals(Attributes input)
        {
            NormalsVaryings output;
            output.positionCS = WINDY_ObjectToHClip(input.positionOS);
            output.normalWS = half3(TransformObjectToWorldNormal(input.normalOS));
            return output;
        }

        // Geometric normal only, matching URP SimpleLit's DepthNormals pass: good
        // enough for _CameraNormalsTexture consumers (SSAO/SSGI) on every tier.
        half4 FragDepthNormals(NormalsVaryings input) : SV_Target
        {
            return half4(half3(SafeNormalize(float3(input.normalWS))), 0.0);
        }
    ENDHLSL
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "IgnoreProjector" = "True" }
        LOD 300

        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex VertPBR
            #pragma fragment FragFullPBR

            // Flipped at runtime by VehicleCamoManager, so shader_feature would let
            // the bundle build strip the variants nobody references at build time.
            #pragma multi_compile_local _ _KEYWORD_ENABLE_CAMO
            #pragma multi_compile_local _ _KEYWORD_ENABLE_CAMO_COLOR_TEX
            #pragma multi_compile_local _ _KEYWORDENABLEDIRT

            // Flipped per material by AsyncApplySrpMat from the source normal map.
            #pragma multi_compile_local _ _ENABLE_NORMAL

            // Global, flipped once when the weather entity activates, never as the sky
            // blends: a mid-transition flip would cost a variant switch. Off leaves the
            // rain/snow layer uncompiled, so a map without weather pays nothing.
            // On every pass that builds the surface, because the layer flattens the
            // normal and the depth-normal and motion passes must agree with GBuffer.
            #pragma multi_compile _ _WINDY_SURFACE_WEATHER

            // Top tier only: receiving the sun's shadow is what stops vehicles
            // reading as pasted on top of the terrain. No _SCREEN variant, the
            // project does not enable screen-space shadows.
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

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

        Pass
        {
            Name "DepthNormals"
            Tags { "LightMode" = "DepthNormals" }

            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex VertDepthNormals
            #pragma fragment FragDepthNormals
            ENDHLSL
        }
    }
    Fallback "Universal Render Pipeline/Unlit"
}
