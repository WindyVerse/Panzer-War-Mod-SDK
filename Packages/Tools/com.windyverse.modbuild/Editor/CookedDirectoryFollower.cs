using System.IO;
using UnityEditor;

namespace WindyVerse.ModBuild
{
    // VehicleInfo/WeaponInfo store the folder their cooked prefab lives in, and the modpack build loads
    // {cookedDirectory}/{loadGuid}_pre.prefab by that path. Moving a vehicle folder (for example from
    // Assets/Res into its pack's package) must carry the path along, or the next build cannot find it.
    internal sealed class CookedDirectoryFollower : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved,
            string[] movedFrom)
        {
            for (var i = 0; i < moved.Length; i++)
            {
                if (!moved[i].EndsWith(".asset"))
                {
                    continue;
                }

                var asset = AssetDatabase.LoadMainAssetAtPath(moved[i]);
                if (asset == null)
                {
                    continue;
                }

                var serialized = new SerializedObject(asset);
                var cooked = serialized.FindProperty("cookedDirectory");
                if (cooked == null || cooked.propertyType != SerializedPropertyType.String)
                {
                    continue;
                }

                var from = Folder(movedFrom[i]);
                if (cooked.stringValue != from)
                {
                    continue; // cooked elsewhere on purpose, or into the shared folder
                }

                cooked.stringValue = Folder(moved[i]);
                serialized.ApplyModifiedPropertiesWithoutUndo();
                AssetDatabase.SaveAssetIfDirty(asset);
            }
        }

        private static string Folder(string assetPath)
        {
            return Path.GetDirectoryName(assetPath)?.Replace('\\', '/');
        }
    }
}
