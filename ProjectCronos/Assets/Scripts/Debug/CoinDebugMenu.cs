using UnityEngine;

namespace ProjectCronos
{
    public class CoinDebugMenu : MonoBehaviour
    {
        /// <summary>
        /// コイン操作ボタン
        /// </summary>
        [SerializeField]
        CoinControlButton[] coinButtons;

        public void Initialize()
        {
            foreach (var coinButton in coinButtons)
            {
                coinButton.Initalize();
            }
        }
    }
}
