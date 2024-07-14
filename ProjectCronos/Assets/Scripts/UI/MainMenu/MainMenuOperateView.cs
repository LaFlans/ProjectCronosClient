using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCronos
{
    /// <summary>
    /// メインメニューの操作説明クラス
    /// </summary>
    public class MainMenuOperateView : MonoBehaviour
    {
        const int BUTTON_DESCRIPTION_MAX = 5;

        /// <summary>
        /// 操作説明テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI guideText;

        /// <summary>
        /// ボタン説明オブジェクト
        /// </summary>
        [SerializeField]
        ButtonDescription[] buttonDescriptions;

        /// <summary>
        /// レイアウトグループ
        /// </summary>
        [SerializeField]
        LayoutGroup layoutGroup;

        public void Initialize()
        {
            guideText.text = string.Empty;
        }

        public void SetUpDescription(
            string guideText,
            params (EnumCollection.Input.INPUT_GAMEPAD_BUTTON, string)[] descs)
        {
            // 操作説明テキストの設定
            this.guideText.text = guideText;

            // ボタン説明テキストの設定
            // 最初に全部非表示の状態にしておく
            InactiveButtonDescription();

            int min = Mathf.Min(descs.Length, BUTTON_DESCRIPTION_MAX);
            if (min > 0)
            {
                for (int i = 0; i < min; i++)
                {
                    buttonDescriptions[i].gameObject.SetActive(true);
                    
                    buttonDescriptions[i].SetUp(LoadedImageUtil.GetGamepadButtonImageTexture(descs[i].Item1), descs[i].Item2);
                }
            }

            // レイアウトを再計算
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.SetLayoutHorizontal();
        }

        /// <summary>
        /// ボタン説明は非表示
        /// </summary>
        void InactiveButtonDescription()
        {
            foreach (var buttonDescription in buttonDescriptions)
            {
                buttonDescription.gameObject.SetActive(false);
            }
        }
    }
}
