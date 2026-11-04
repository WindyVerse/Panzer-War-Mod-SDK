using ShanghaiWindy.Core;
using UnityEditor;
using UnityEngine;

#if URP_TOOL_PROJ
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#endif

public class VehicleThumbnailAddOn : MonoBehaviour
{
#if URP_TOOL_PROJ
    private static Volume _volume;
#endif

    [InitializeOnLoadMethod]
    private static void RegisterOnFirstLaunch()
    {
        VehicleDataEditorUtil.PreCreateThumbnail += HandlePreCreateThumbnail;
    }

    private static void HandlePreCreateThumbnail(Camera cam)
    {
#if URP_TOOL_PROJ
        var camData = cam.GetComponent<UniversalAdditionalCameraData>();
        if (camData == null)
        {
            camData = cam.gameObject.AddComponent<UniversalAdditionalCameraData>();
        }

        camData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        camData.renderPostProcessing = true;

        _volume = FindObjectOfType<Volume>();
        if (_volume == null)
        {
            _volume = new GameObject("Volume").AddComponent<Volume>();
        }

        _volume.profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(
            "Packages/com.unity.render-pipelines.universal/Editor/SceneTemplates/Standard/GlobalVolumeProfile.asset");
#endif
    }
}