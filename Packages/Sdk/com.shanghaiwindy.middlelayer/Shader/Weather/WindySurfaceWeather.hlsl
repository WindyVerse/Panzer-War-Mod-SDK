#ifndef WINDY_SURFACE_WEATHER_INCLUDED
#define WINDY_SURFACE_WEATHER_INCLUDED

// Wet film, snow accumulation and close-range drops, driven only by globals.
// Docs/dev/VehicleSurfaceWeather.md
//
// Not a .windyshaderinc: %%include emits deepest-first, ahead of its caller.

// xy: accumulated film / snow (outlives the shower)  zw: falling right now
float4 _WindyWetness;

TEXTURE2D(_WindySurfaceWeatherTex);   // r grain, g drop height, ba drop normal xy
SAMPLER(sampler_WindySurfaceWeatherTex);

static const half WINDY_WET_SMOOTHNESS = half(0.90);
static const half WINDY_WET_DARKEN     = half(0.68);
static const half WINDY_WET_POROUS     = half(1.4);
static const half WINDY_WET_FLOOR      = half(0.35);
static const half WINDY_WET_METAL_KILL = half(0.6);
static const half WINDY_WET_FLATTEN    = half(0.45);
static const half WINDY_WET_CAVITY     = half(1.2);   // dirt layer's weights, so both pool alike
static const half WINDY_WET_DUST       = half(0.5);

// Capped: only vehicles carry this layer, and a white tank on green ground is what
// would give that away. Raise it once world surfaces read the same globals.
static const half  WINDY_SNOW_COVER_MAX  = half(0.5);
static const half3 WINDY_SNOW_ALBEDO     = half3(0.72, 0.74, 0.78);  // pure white blows out bloom
static const half  WINDY_SNOW_BREAKUP    = half(0.9);
static const half  WINDY_SNOW_TILING     = half(4.0);
// Must stay clear of zero: below the grain's reach the gate stops gating and heavy
// snow lands on a flat COVER_MAX everywhere.
static const half  WINDY_SNOW_GATE_MIN   = half(0.40);
static const half  WINDY_SNOW_GROWTH_SOFT= half(0.25);
static const half  WINDY_SNOW_SHADED     = half(0.25);
static const half  WINDY_SNOW_SMOOTH_DRY = half(0.18);
static const half  WINDY_SNOW_SMOOTH_WET = half(0.55);
static const half  WINDY_SNOW_FLATTEN    = half(0.8);
// Default Mask.png only spans 0.86..1.0 in occlusion; same remap the dirt layer needs.
static const half WINDY_SNOW_AO_FLOOR = half(0.80);
static const half WINDY_SNOW_AO_SCALE = half(5.0);

// World surfaces are the ground truth the vehicle cap is measured against, so they take
// full cover. Their grain tiles in metres, not UV: a terrain chart spans the whole map,
// so WINDY_SNOW_TILING there would put four noise cells across a kilometre.
static const half WINDY_SNOW_COVER_MAX_WORLD = half(1.0);
static const half WINDY_SNOW_TILING_WORLD = half(0.15);

// Snow has to be a material here, not a tint. On a hull the paint and dirt underneath
// keep carrying detail through a 0.5 cap, but a hillside at full cover is nothing but
// this layer, and a constant albedo over a flattened normal reads as plastic - leaving
// the base texture's tiling as the only thing on screen with any structure in it.
static const half WINDY_SNOW_DETAIL_TILING = half(0.8);    // ~1.25 m, second octave
static const half WINDY_SNOW_PATCH_MIX     = half(0.4);    // its share of the coverage gate
// Sized against the shipped texture, not guessed: R is mean 0.5 / std 0.15, so p5-p95
// of (r - 0.5) is only -0.25..+0.23. Anything under ~0.4 here is invisible.
static const half WINDY_SNOW_DETAIL_ALBEDO = half(0.45);   // -> albedo x0.89..1.11
static const half WINDY_SNOW_DETAIL_SMOOTH = half(0.55);   // -> +/-0.13 sheen, packed vs powder
// Domain warp, in fine-octave tiles. Two octaves off one texture still share its
// lattice; displacing the fine one by the coarse one is what turns the grid into
// patches. Cheap - it reuses a sample already in hand and costs no fetch.
static const half WINDY_SNOW_WARP = half(2.5);

// Flat surfaces - no normal, no specular, one channel. They never get painted out
// completely, for the reason the vehicle caps: whatever is underneath has to keep
// carrying the detail, because this layer has nothing to carry it with. SHOWTHROUGH
// pulls the snow tone toward the base's own luminance, and on these maps that base is
// the DAVA colormap, which is authored per map and does not tile at all.
static const half WINDY_SNOW_COVER_MAX_FLAT = half(0.82);
static const half WINDY_SNOW_SHOWTHROUGH    = half(0.55);
static const half WINDY_SNOW_BASE_MID       = half(0.30);
// The Blinn-Phong props' base maps sit about half as bright as a DAVA colormap - median
// linear luminance 0.156 across the 658 readable SimpleLit base maps, p25 0.094, p75
// 0.235 - so their window is centred lower. Measured, not guessed; re-measure before
// moving it.
static const half WINDY_SNOW_BASE_MID_PROP  = half(0.16);
// Far below the vehicle's 0.8: snow drapes terrain, it does not erase it, and those
// tile normals are the one piece of non-repeating detail the surface already owns.
static const half WINDY_SNOW_FLATTEN_WORLD = half(0.25);

// Foliage takes sky exposure where a surface takes a normal. Baked canopy AO is centred
// near 0.5, so the shell of the crown sits above the floor and the interior below it.
static const half WINDY_FOLIAGE_AO_FLOOR = half(0.30);
static const half WINDY_FOLIAGE_AO_SCALE = half(1.6);
static const half WINDY_FOLIAGE_UNDER    = half(0.50);
// Wider than a surface's: the shared band is sized for a potential that spans the whole
// range, and a canopy clusters far tighter than that. At the surface width the crown
// snaps from bare to solid with nothing between it and the branches below.
static const half WINDY_FOLIAGE_GROWTH_SOFT = half(0.70);

// Tiles per object-space metre: 0.8 m tile, 1-3 cm drops. UV tiling cannot hold a
// size, since hull charts are packed at whatever density the artist left.
static const half WINDY_BEAD_TILING      = half(1.25);
static const half WINDY_BEAD_RELIEF      = half(0.45);
static const half WINDY_BEAD_WETNESS     = half(0.5);
static const half WINDY_BEAD_RATE        = half(0.13);  // swell/dry cycles per second
static const half WINDY_BEAD_RESIDUAL    = half(0.4);   // what is left beading after the rain stops
static const half WINDY_DETAIL_FADE_START= half(6.0);   // a 1 cm drop is one pixel at 10 m
static const half WINDY_DETAIL_FADE_END  = half(18.0);

// No run-off. Rivulets have to travel along gravity, which forces a world-space
// frame, and a world-space frame slides across a hull that is driving; an
// object-space one stays put but no longer points down. Unity's own production
// weather sample lands on the same wall and says so: its drips "are projected in
// world space, so they work well for static objects but are not meant for moving
// objects". Vehicles are moving objects, and their near-vertical faces are mostly
// behind fenders and track runs anyway, so those keep the plain sheen (WET_FLOOR)
// that a rained-on plate actually shows at gameplay range.

// Shared by every tier, so a tank and the building behind it pool and whiten alike.
half WindyWetFilm(half wet, half ao, half up)
{
    half pool = saturate(WINDY_WET_CAVITY * (half(1.0) - ao) + WINDY_WET_DUST * up);
    return wet * lerp(WINDY_WET_FLOOR, half(1.0), pool);
}

// Gate, breakup and cap: the half that decides how snowy the world looks, so every
// object type goes through it. Only the potential feeding it is per-type.
half WindySnowFromPotential(half snowAmt, half potential, half grain, half coverMax,
    half growthSoft)
{
    // Grain scales potential, never adds: added, its positive half manufactures
    // snow on zero-potential surfaces like vertical armour.
    potential = saturate(potential * (half(1.0) + (grain - half(0.5)) * WINDY_SNOW_BREAKUP));

    half gate = lerp(half(1.0), WINDY_SNOW_GATE_MIN, snowAmt);
    return smoothstep(gate, gate + growthSoft, potential) * coverMax;
}

half WindySnowCover(half snowAmt, half up, half ao, half grain, half coverMax)
{
    half exposure = saturate((ao - WINDY_SNOW_AO_FLOOR) * WINDY_SNOW_AO_SCALE);
    return WindySnowFromPotential(snowAmt,
        up * up * lerp(WINDY_SNOW_SHADED, half(1.0), exposure), grain, coverMax,
        WINDY_SNOW_GROWTH_SOFT);
}

void WindyApplyWetPBR(inout half3 albedo, inout half metallic, inout half smoothness, half film)
{
    // Matte means porous means it drinks more. Remapped against 0.5, not 1: vehicle
    // smoothness only spans 0.08 to 0.45, and 1 - smoothness separates nothing there.
    half porous = lerp(half(1.0), WINDY_WET_POROUS, saturate((half(0.5) - smoothness) * half(2.0)));

    albedo *= lerp(half(1.0), WINDY_WET_DARKEN, saturate(film * porous));
    smoothness = lerp(smoothness, WINDY_WET_SMOOTHNESS, film);
    metallic = lerp(metallic, half(0.0), film * WINDY_WET_METAL_KILL);
}

void WindyApplySnowPBR(inout half3 albedo, inout half metallic, inout half smoothness,
    inout half occlusion, half cover, half wet)
{
    albedo = lerp(albedo, WINDY_SNOW_ALBEDO, cover);
    metallic = lerp(metallic, half(0.0), cover);
    smoothness = lerp(smoothness, lerp(WINDY_SNOW_SMOOTH_DRY, WINDY_SNOW_SMOOTH_WET, wet), cover);
    occlusion = lerp(occlusion, half(1.0), cover);   // snow is its own surface
}

// planarOS is the horizontal object-space position: drops need one physical size
// across vehicles and must stay put on a moving hull, and neither UV nor world
// space gives both. distToCamera is passed because each pipeline has a cheaper
// route to it.
void ApplyWindySurfaceWeather(inout half3 albedo, inout half3 normalTS, inout half metallic,
    inout half occlusion, inout half smoothness, float2 uv, half3 geoNormalWS,
    float2 planarOS, half distToCamera)
{
#ifdef _WINDY_SURFACE_WEATHER
    // The keyword is the switch; clear weather needs no early-out, since every lerp
    // is then an identity and both fetches are already branch-guarded.
    half wet = half(_WindyWetness.x);
    half snowAmt = half(_WindyWetness.y);

    half up = saturate(geoNormalWS.y);
    half ao = occlusion;             // snow overwrites occlusion; both layers read it
    half2 beadOffset = half2(0.0, 0.0);

    half film = WindyWetFilm(wet, ao, up);

    // Gate on `wet`, a cbuffer scalar, never on the per-pixel film or distance: a
    // fetch with implicit derivatives in varying flow control is undefined. Distance
    // is still honoured, through `detail` multiplying into both results.
    UNITY_BRANCH
    if (wet > half(0.001))
    {
        half detail = half(1.0) - smoothstep(WINDY_DETAIL_FADE_START, WINDY_DETAIL_FADE_END, distToCamera);
        half4 drop = SAMPLE_TEXTURE2D(_WindySurfaceWeatherTex, sampler_WindySurfaceWeatherTex,
            planarOS * WINDY_BEAD_TILING);

        // Grain is the per-drop phase: coarser than the beads, so each keeps its own
        // clock and the sheet never pulses as one. Float until after the frac, since
        // a half cannot resolve a second once _Time.y is minutes in.
        half cycle = half(frac(_Time.y * float(WINDY_BEAD_RATE) + float(drop.r)));
        half pulse = smoothstep(half(0.0), half(0.3), cycle) *
            (half(1.0) - smoothstep(half(0.6), half(1.0), cycle));

        // Drops keep renewing while it falls and mostly dry off once it stops, which
        // is the only thing left reading _WindyWetness.z now that run-off is gone.
        half beading = wet * lerp(WINDY_BEAD_RESIDUAL, half(1.0), half(_WindyWetness.z));

        // Routed through the film so a drop darkens and polishes like water. Relief
        // with no material cue behind it is what reads as damage.
        half bead = drop.g * detail * up * pulse * beading;
        film = saturate(film + bead * WINDY_BEAD_WETNESS);
        beadOffset = (drop.ba * half(2.0) - half(1.0)) * (bead * WINDY_BEAD_RELIEF);
    }

    WindyApplyWetPBR(albedo, metallic, smoothness, film);
    half flatten = film * WINDY_WET_FLATTEN;

    UNITY_BRANCH
    if (snowAmt > half(0.001))
    {
        half grain = SAMPLE_TEXTURE2D(_WindySurfaceWeatherTex, sampler_WindySurfaceWeatherTex,
            uv * WINDY_SNOW_TILING).r;
        half cover = WindySnowCover(snowAmt, up, ao, grain, WINDY_SNOW_COVER_MAX);

        WindyApplySnowPBR(albedo, metallic, smoothness, occlusion, cover, wet);
        flatten = max(flatten, cover * WINDY_SNOW_FLATTEN);
    }

    // Toward the geometric normal, not world up: both layers fill the surface's own
    // detail, and anything holding either already faces up.
    normalTS = lerp(normalTS, half3(0.0, 0.0, 1.0), flatten);
    normalTS.xy += beadOffset;   // drops sit on the film, so they go on after it
#endif
}

// Terrain, buildings and props. Static geometry, so the grain tiles in world metres and
// stays put; no beads, because a bead is a painted-metal cue and the two heaviest shaders
// in the scene should not pay a second fetch to grow water drops on soil.
void ApplyWindyWorldWeather(inout half3 albedo, inout half3 normalTS, inout half metallic,
    inout half occlusion, inout half smoothness, half3 geoNormalWS, float2 planarWS)
{
#ifdef _WINDY_SURFACE_WEATHER
    half wet = half(_WindyWetness.x);
    half snowAmt = half(_WindyWetness.y);

    half up = saturate(geoNormalWS.y);
    half ao = occlusion;

    half film = WindyWetFilm(wet, ao, up);
    WindyApplyWetPBR(albedo, metallic, smoothness, film);
    half flatten = film * WINDY_WET_FLATTEN;

    UNITY_BRANCH
    if (snowAmt > half(0.001))
    {
        half coarse = SAMPLE_TEXTURE2D(_WindySurfaceWeatherTex, sampler_WindySurfaceWeatherTex,
            planarWS * WINDY_SNOW_TILING_WORLD).r;
        half detail = SAMPLE_TEXTURE2D(_WindySurfaceWeatherTex, sampler_WindySurfaceWeatherTex,
            planarWS * WINDY_SNOW_DETAIL_TILING + (coarse - half(0.5)) * WINDY_SNOW_WARP).r;

        // Two octaves into the gate. One frequency over something as large as terrain
        // draws its own grid, and the patch edge is where that grid is legible.
        half grain = lerp(coarse, detail, WINDY_SNOW_PATCH_MIX);
        half cover = WindySnowCover(snowAmt, up, ao, grain, WINDY_SNOW_COVER_MAX_WORLD);

        half var = detail - half(0.5);
        WindyApplySnowPBR(albedo, metallic, smoothness, occlusion, cover, wet);
        albedo = lerp(albedo, albedo * (half(1.0) + var * WINDY_SNOW_DETAIL_ALBEDO), cover);
        smoothness = saturate(smoothness + var * WINDY_SNOW_DETAIL_SMOOTH * cover);

        // No snow normal of its own: the texture's spare channels hold a droplet
        // normal, which is 64% flat and shaped like scattered round beads. What
        // carries the relief instead is the surface's own normal, kept rather than
        // erased - it is real, continuous and does not repeat.
        flatten = max(flatten, cover * WINDY_SNOW_FLATTEN_WORLD);
    }

    normalTS = lerp(normalTS, half3(0.0, 0.0, 1.0), flatten);
#endif
}

// Fully dielectric surfaces (terrain): metallic would only be driven toward the zero it
// already holds, so it never has to leave the caller.
void ApplyWindyWorldWeather(inout half3 albedo, inout half3 normalTS,
    inout half occlusion, inout half smoothness, half3 geoNormalWS, float2 planarWS)
{
    half metallic = half(0.0);
    ApplyWindyWorldWeather(albedo, normalTS, metallic, occlusion, smoothness,
        geoNormalWS, planarWS);
}

// Snow over a surface with nothing but an albedo to draw with: the surface's own
// luminance, remapped around baseMid, lifts or drops the snow tone rather than being
// replaced by it. Noise is spent on the coverage edge (the caller's gate) and nowhere
// else - as a tone across full cover it is the one thing left on a blank plain, and a
// tiling texture read that way is read as its lattice.
half3 WindySnowToneFlat(half3 albedo, half baseMid)
{
    half lum = dot(albedo, half3(0.2126, 0.7152, 0.0722));
    half structure = clamp(lum * rcp(baseMid), half(0.65), half(1.30));
    return WINDY_SNOW_ALBEDO * lerp(half(1.0), structure, WINDY_SNOW_SHOWTHROUGH);
}

// The two world octaves that gate the flat tiers, warped so the second does not share
// the first's lattice; see ApplyWindyWorldWeather for the reasoning.
half WindySnowGrainWorld(float2 planarWS)
{
    half coarse = SAMPLE_TEXTURE2D(_WindySurfaceWeatherTex, sampler_WindySurfaceWeatherTex,
        planarWS * WINDY_SNOW_TILING_WORLD).r;
    half fine = SAMPLE_TEXTURE2D(_WindySurfaceWeatherTex, sampler_WindySurfaceWeatherTex,
        planarWS * WINDY_SNOW_DETAIL_TILING + (coarse - half(0.5)) * WINDY_SNOW_WARP).r;
    return lerp(coarse, fine, WINDY_SNOW_PATCH_MIX);
}

// Baked-lit props, tilemask terrain and foliage: albedo is the only channel they own.
// AO is 1 because they carry none, which reads as fully sky-exposed - correct for what
// this tier covers. Porosity stays neutral, so these darken no harder than a PBR surface
// of the same wetness rather than guessing a smoothness that was never authored.
void ApplyWindyWorldWeatherAlbedo(inout half3 albedo, half up, float2 planarWS)
{
#ifdef _WINDY_SURFACE_WEATHER
    half wet = half(_WindyWetness.x);
    half snowAmt = half(_WindyWetness.y);

    half film = WindyWetFilm(wet, half(1.0), up);
    albedo *= lerp(half(1.0), WINDY_WET_DARKEN, saturate(film));

    UNITY_BRANCH
    if (snowAmt > half(0.001))
    {
        half grain = WindySnowGrainWorld(planarWS);
        half cover = WindySnowCover(snowAmt, up, half(1.0), grain, WINDY_SNOW_COVER_MAX_FLAT);

        // The colormap under it is per-map and never repeats, so it stays the structure.
        albedo = lerp(albedo, WindySnowToneFlat(albedo, WINDY_SNOW_BASE_MID), cover);
    }
#endif
}

// Blinn-Phong props (WindyVerse/Scene/SimpleLit): a normal map on most, AO on none,
// specular on almost none. That fails the PBR tier's argument on both counts - no AO to
// break the cover up, no sheen to make the snow read as a material - and at full cover
// its detail noise was the only structure left on 870 tiled walls and roads, all of it
// one world-space lattice. So the snow takes the albedo tier's answer (capped below one,
// toned by the base map, no noise tone) and the normal takes the PBR tier's (kept, and
// flattened only by FLATTEN_WORLD): where a map exists it is the one piece of
// non-repeating relief the prop owns. Smoothness rides along for the few with specular.
void ApplyWindyPropWeather(inout half3 albedo, inout half3 normalTS, inout half smoothness,
    half3 geoNormalWS, float2 planarWS)
{
#ifdef _WINDY_SURFACE_WEATHER
    half wet = half(_WindyWetness.x);
    half snowAmt = half(_WindyWetness.y);

    half up = saturate(geoNormalWS.y);
    half metallic = half(0.0);

    half film = WindyWetFilm(wet, half(1.0), up);
    WindyApplyWetPBR(albedo, metallic, smoothness, film);
    half flatten = film * WINDY_WET_FLATTEN;

    UNITY_BRANCH
    if (snowAmt > half(0.001))
    {
        half grain = WindySnowGrainWorld(planarWS);
        half cover = WindySnowCover(snowAmt, up, half(1.0), grain, WINDY_SNOW_COVER_MAX_FLAT);

        albedo = lerp(albedo, WindySnowToneFlat(albedo, WINDY_SNOW_BASE_MID_PROP), cover);
        smoothness = lerp(smoothness,
            lerp(WINDY_SNOW_SMOOTH_DRY, WINDY_SNOW_SMOOTH_WET, wet), cover);
        flatten = max(flatten, cover * WINDY_SNOW_FLATTEN_WORLD);
    }

    normalTS = lerp(normalTS, half3(0.0, 0.0, 1.0), flatten);
#endif
}

// Billboarded foliage: the fragment owns neither a normal nor a world position, and a
// leaf card has no orientation to begin with, so this resolves on the vertex from what
// the sky can reach instead.
//
// vertexAO is the mesh's baked canopy occlusion, and it is the load-bearing input: a
// crown's own pseudo-normal points from the trunk axis outward, which calls every trunk
// vertex perfectly up-facing and would frost the bark before a single leaf. AO puts the
// trunk back under the canopy where it belongs. crown (0 underside, 1 top) only biases.
//
// The potential is a sky-visibility probability, so it skips the up*up a real surface
// normal needs - it is already the answer that cosine term estimates.
half2 WindyFoliageWeatherFactors(half vertexAO, half crown, float2 planarWS)
{
#ifdef _WINDY_SURFACE_WEATHER
    half exposed = saturate((vertexAO - WINDY_FOLIAGE_AO_FLOOR) * WINDY_FOLIAGE_AO_SCALE);
    half skyFacing = exposed * lerp(WINDY_FOLIAGE_UNDER, half(1.0), crown);

    half grain = SAMPLE_TEXTURE2D_LOD(_WindySurfaceWeatherTex, sampler_WindySurfaceWeatherTex,
        planarWS * WINDY_SNOW_TILING_WORLD, 0).r;
    return half2(
        WindyWetFilm(half(_WindyWetness.x), half(1.0), skyFacing),
        WindySnowFromPotential(half(_WindyWetness.y), skyFacing, grain,
            WINDY_SNOW_COVER_MAX_WORLD, WINDY_FOLIAGE_GROWTH_SOFT));
#else
    return half2(0.0, 0.0);
#endif
}

void ApplyWindyFoliageWeather(inout half3 albedo, half2 factors)
{
#ifdef _WINDY_SURFACE_WEATHER
    albedo *= lerp(half(1.0), WINDY_WET_DARKEN, saturate(factors.x));
    albedo = lerp(albedo, WINDY_SNOW_ALBEDO, factors.y);
#endif
}

#endif // WINDY_SURFACE_WEATHER_INCLUDED
