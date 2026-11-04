using HoudiniEngineUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ShanghaiWindy.Core
{
    public class PDGBaker : MonoBehaviour
    {
        public string fileName = "Test";
        public int piexelError = 15;

        [Button("Bake")]
        public void Bake()
        {
            var folder = new DirectoryInfo($"Assets/Bake/{fileName}");
            if (!folder.Exists)
            {
                folder.Create();
            }

            GameObject pdgPrefabRoot = new GameObject($"PDG_{fileName}");
            var index = 0;

            var baseFolder = new DirectoryInfo($"Assets/HoudiniEngineAssetCache/Baked/{fileName}");
            if (baseFolder.Exists)
            {
                baseFolder.Delete(true);
            }

            foreach (var baseSync in FindObjectsOfType<HEU_BaseSync>())
            {
                foreach (HEU_GeneratedOutput generatedOutput in baseSync._generatedOutputs)
                {
                    var outputPath = $"Assets/HoudiniEngineAssetCache/Baked/{fileName}/{index}";
                    index += 1;

                    var dir = new DirectoryInfo(outputPath);
                    dir.Create();
#if UNITY_EDITOR
                    AssetDatabase.Refresh();
#endif
                    var obj =
                        HEU_GeneralUtility.CreateNewGameObject(generatedOutput._outputData._gameObject.name);

                    generatedOutput.WriteOutputToAssetCache(obj, outputPath, generatedOutput.IsInstancer);
                    obj.transform.parent = pdgPrefabRoot.transform;

                    if (!generatedOutput.IsInstancer)
                    {
                        foreach (var trans in obj.GetComponentsInChildren<Transform>())
                        {
                            trans.name = "_";

                            var terrainCom = trans.GetComponent<Terrain>();
                            if (terrainCom != null)
                            {
                                trans.gameObject.isStatic = true;
                                trans.gameObject.layer = WargameLayerDefined.TerrainLayer;

                                terrainCom.heightmapPixelError = piexelError;
                            }

                            if (trans.gameObject.isStatic)
                            {
                                if (trans.gameObject.layer == WargameLayerDefined.DefaultLayer)
                                {
                                    trans.gameObject.layer = WargameLayerDefined.BuildingLayer;
                                }

#if UNITY_EDITOR
                                var flags = StaticEditorFlags.NavigationStatic;
                                GameObjectUtility.SetStaticEditorFlags(trans.gameObject, flags);
#endif
                            }
                        }
                    }
                }
            }


            string prefabPath = HEU_AssetDatabase.AppendPrefabPath($"Assets/Bake/{fileName}", pdgPrefabRoot.name);
            GameObject prefabGO = HEU_EditorUtility.SaveAsPrefabAsset(prefabPath, pdgPrefabRoot);

            if (prefabGO != null)
            {
                HEU_EditorUtility.SelectObject(prefabGO);
                HEU_Logger.LogFormat("Exported prefab to {0}", prefabPath);
            }

            DestroyImmediate(pdgPrefabRoot);
        }
    }
}