using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace ProjectCronos
{
    public class GuideView : MonoBehaviour
    {
        [SerializeField]
        GameObject controlGuideObj;

        /// <summary>
        /// ボタンの画像
        /// </summary>
        [SerializeField]
        RawImage buttonIconImage;

        [SerializeField]
        TextMeshProUGUI messageText;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            controlGuideObj.SetActive(false);
        }

        /// <summary>
        /// 操作方法のガイドを表示
        /// </summary>
        public void ShowControlGuide(
            string message,
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON gamepadButtonType)
        {
            buttonIconImage.texture = LoadedImageUtil.GetGamepadButtonImageTexture(gamepadButtonType);
            controlGuideObj.SetActive(true);
            messageText.text = message;
        }

        /// <summary>
        /// 操作方法のガイドを非表示
        /// </summary>
        public void HideControlGuide()
        {
            if (controlGuideObj.activeSelf)
            {
                controlGuideObj.SetActive(false);
            }
        }
    }
}
