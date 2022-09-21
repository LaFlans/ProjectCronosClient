using UnityEngine;
using TMPro;
using System;

namespace ProjectCronos
{
    /// <summary>
    /// デフォルトポップアップ
    /// </summary>
    class DefaultPopup : PopupBase
    {
        [SerializeField]
        TextMeshProUGUI titleText;
        [SerializeField]
        TextMeshProUGUI messageText;
        [SerializeField]
        TextMeshProUGUI positiveButtonMessageText;
        [SerializeField]
        TextMeshProUGUI negativeButtonMessageText;
        [SerializeField]
        TextMeshProUGUI otherButtonMessageText;
        [SerializeField]
        CanvasGroup canvasGroup;

        bool isSetup = false;
        bool isOpenAnimation = false;

        public override void Setup(Action callback)
        {
            base.Setup(callback);

            canvasGroup.alpha = 0;

            // メッセージ設定
            titleText.text = "デフォルトポップアップ";
            messageText.text = "テスト用のポップアップです";
            positiveButtonMessageText.text = "YES";
            negativeButtonMessageText.text = "NO";
            otherButtonMessageText.text = "OTHER";

            // アクション設定
            buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE] = OnClickPositiveButton;
            buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE] = OnClickNegativeButton;
            buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER] = OnClickOtherButton;

            closeAction = callback;

            // ボタンの設定
            ButtonSetup();

            isSetup = true;
        }

        void Update()
        {
            if (!isOpenAnimation && isSetup)
            {
                isOpenAnimation = true;
                canvasGroup.alpha = 1;

                // アニメーション開始
                anim.SetTrigger("Open");
            }
        }

        void OnClickPositiveButton()
        {
            Debug.Log("ポジティブボタンを押したよ！");
            Close();
        }

        void OnClickNegativeButton()
        {
            Debug.Log("ネガティブボタンを押したよ！");
            Close();
        }

        void OnClickOtherButton()
        {
            Debug.Log("その他ボタンを押したよ！");
            Close();
        }
    }
}
