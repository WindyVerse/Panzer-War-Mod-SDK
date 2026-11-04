using ShanghaiWindy.Core;
using Tools.RuntimeDebugger.Scripts.GameMode;
using UnityEngine;

namespace Tools.RuntimeDebugger.Scripts
{
    public class VehicleTestFieldStartup : MonoBehaviour
    {
        private void Awake()
        {
            EditorModeStartup.OnInit += () =>
            {
                var gameMode = new VehicleTestFieldGameMode();
                GameModeManager.Instance.StartGameMode(gameMode);
            };
        }
    }
}
