using ShanghaiWindy.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.RuntimeDebugger.Scripts
{
    public class MenuViewCtrl : MonoBehaviour
    {
        public Button menuBtn;

        private void Start()
        {
            menuBtn.onClick.AddListener(() => { UIManager.Instance.ShowUI(UIEnum.GAME_SETTING_UI); });
        }
    }
}