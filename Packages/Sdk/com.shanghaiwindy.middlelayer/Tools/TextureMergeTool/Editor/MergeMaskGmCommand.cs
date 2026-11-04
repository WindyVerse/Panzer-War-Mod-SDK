using System.IO;
using ShanghaiWindy.Core;
using ShanghaiWindy.Editor;
using UnityEditor;
using UnityEngine;

namespace Tools.TextureMergeTool.Editor
{
    [System.Serializable]
    public class MergeMaskGmCommand : AbstractGmCommand
    {
        private Texture2D metallic, occlusion, smoothness;
        private Texture2D _metallic, _occlusion, _smoothness;
        private bool roughness, _roughness;

        private Material mat;
        private bool changed;


        public override string GetName()
        {
            return ELK.Cat_DataMigration + "♥ 合并遮罩贴图 - Remap PBR Masks";
        }

        public MergeMaskGmCommand()
        {
            Init();
        }

        private void Init()
        {
            if (mat == null)
            {
                mat = new Material(Shader.Find("Hidden/MaskMap"));
                mat.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        public override void OnImGUI()
        {
            base.OnImGUI();

            GetInput();
            Preview();
            CreateMask();
        }


        private void CreateMask()
        {
            if (GUILayout.Button("生成 Mask - Create Mask Map"))
            {
                GenPicture();
            }
        }

        private void GenPicture()
        {
            var output = GetTexture();

            var fullPath = EditorPrefs.GetString("MergeMaskPath", $"{Application.dataPath}/mask.png");

            var savePath =
                EditorUtility.SaveFilePanel("Save", Path.GetDirectoryName(fullPath), Path.GetFileName(fullPath), "png");
            if (string.IsNullOrEmpty(savePath))
            {
                return;
            }

            EditorPrefs.SetString("MergeMaskPath", savePath);
            File.WriteAllBytes(savePath, output.EncodeToPNG());
            AssetDatabase.Refresh();
        }

        private Texture2D GetTexture()
        {
            if (metallic != null)
            {
                mat.SetTexture("_Metallic", metallic);
            }

            if (occlusion != null)
            {
                mat.SetTexture("_Occlusion", occlusion);
            }

            if (smoothness != null)
            {
                mat.SetTexture("_Smoothness", smoothness);
            }

            mat.SetFloat("_UseRoughness", _roughness ? 1 : 0);

            var defaultSize = GetDefaultSize();
            var tempRT = new RenderTexture(defaultSize.x, defaultSize.y, 32,
                RenderTextureFormat.ARGB32);
            tempRT.Create();
            var temp2 = new Texture2D(tempRT.width, tempRT.height, TextureFormat.ARGB32, false);
            Graphics.Blit(temp2, tempRT, mat);
            var prev = RenderTexture.active;
            RenderTexture.active = tempRT;

            var output = new Texture2D(tempRT.width, tempRT.height, TextureFormat.ARGB32, false);
            output.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
            output.Apply();
            RenderTexture.active = prev;

            return output;
        }

        private Vector2Int GetDefaultSize()
        {
            if (metallic == null)
            {
                return Vector2Int.one * 512;
            }

            return new Vector2Int(metallic.width, metallic.height);
        }

        private void GetInput()
        {
            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.Label("Input Textures");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");
            _metallic = TextureField("metallic", metallic);
            if (_metallic != metallic)
            {
                metallic = _metallic;
                changed = true;
            }

            _occlusion = TextureField("occlusion", occlusion);
            if (_occlusion != occlusion)
            {
                occlusion = _occlusion;
                changed = true;
            }

            _smoothness = TextureField(_roughness ? "roughness" : "smoothness", smoothness);
            if (_smoothness != smoothness)
            {
                smoothness = _smoothness;
                changed = true;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            _roughness = EditorGUILayout.Toggle("Is Roughness", roughness);
            if (_roughness != roughness)
            {
                roughness = _roughness;
                changed = true;
            }

            EditorGUILayout.EndHorizontal();
        }

        private Texture2D preview = null;

        private void Preview()
        {
            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.Label("Preview Output");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("Box");

            var previewRect = new Rect(175, 270, 150, 150);
            if (preview == null)
            {
                preview = Texture2D.blackTexture;
            }

            if (changed)
            {
                preview = GetTexture();
                changed = false;
            }

            EditorGUI.DrawPreviewTexture(previewRect, preview);
            EditorGUILayout.Space(160);
            EditorGUILayout.EndHorizontal();
        }

        private Texture2D TextureField(string name, Texture2D texture)
        {
            EditorGUILayout.BeginVertical();
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.fixedWidth = 150;
            GUILayout.Label(name, style);
            var result = EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(150),
                GUILayout.Height(150)) as Texture2D;
            EditorGUILayout.EndHorizontal();
            return result;
        }
    }
}