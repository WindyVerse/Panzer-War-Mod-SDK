using UnityEngine;

namespace Tools.RuntimeDebugger.Scripts
{
    public class ActivateAssetHandler : MonoBehaviour
    {
        public GameObject[] targets;

        private void Awake()
        {
            foreach (var target in targets)
            {
                target.SetActive(false);
            }
        }

        private void Start()
        {
            EditorModeStartup.OnInit += () =>
            {
                foreach (var target in targets)
                {
                    target.SetActive(true);
                }
            };
        }
    }
}