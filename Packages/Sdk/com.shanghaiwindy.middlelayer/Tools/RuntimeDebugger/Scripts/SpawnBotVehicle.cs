using System.Collections;
using CustomAttributes;
using ShanghaiWindy.Core;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tools.RuntimeDebugger.Scripts
{
    public class SpawnBotVehicle : MonoBehaviour
    {
        [LocalizedLabel("人机阵营 - Bot Team")] public TeamManager.Team botTeam = TeamManager.Team.blue;

        [LocalizedLabel("是否 Idle - Is Idle")] public bool isIdle;

        [LocalizedLabel("载具人机 - Bot Vehicle Info")] [ValueDropdown(nameof(GetVehicleInfos))]
        public string vehicleGuid;

        private void Awake()
        {
            EditorModeStartup.OnInit += () =>
            {
                if (!string.IsNullOrEmpty(vehicleGuid))
                {
                    var vehicleInfo = VehicleInfoManager.Instance.GetVehicleInfoByGuid(vehicleGuid);
                    if (vehicleInfo != null)
                    {
                        CreateVehicleUtility.CreateLocalBot(vehicleInfo, transform.position, transform.rotation,
                            botTeam, true, isIdle);
                    }
                }
            };
        }

        public static IEnumerable GetVehicleInfos()
        {
            var vehicleInfos = new ValueDropdownList<string>();
#if UNITY_EDITOR

            foreach (var x in AssetDatabase.FindAssets($"t:{nameof(VehicleInfo)}"))
            {
                var res = AssetDatabase.LoadAssetAtPath<VehicleInfo>(AssetDatabase.GUIDToAssetPath(x));
                vehicleInfos.Add(res.GetDisplayName(), res.GetLoadGUID());
            }

#endif
            return vehicleInfos;
        }
    }
}