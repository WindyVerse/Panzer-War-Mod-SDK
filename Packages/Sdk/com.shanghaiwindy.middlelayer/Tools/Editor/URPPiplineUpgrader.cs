using UnityEditor;
using UnityEngine;
#if URP_TOOL_PROJ
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#endif

public class URPPiplineUpgrader
{
    private const string PiplinePath = "Packages/com.shanghaiwindy.middlelayer/URPSetting/Common_PipelineAsset.asset";

    [InitializeOnLoadMethod]
    private static void CheckUpgrade()
    {
#if URP_TOOL_PROJ
        if (QualitySettings.renderPipeline == null || GraphicsSettings.renderPipelineAsset == null)
        {
            var urpAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PiplinePath);

            QualitySettings.renderPipeline = urpAsset;
            GraphicsSettings.renderPipelineAsset = urpAsset;

            EditorUtility.DisplayDialog("Info",
                "你的项目已升级成基于 URP 渲染管线 - Your project has been upgraded to Universal Render Pipline based",
                "OK");
        }

        Debug.Log("Check if project is urp project.");
#endif
    }
}