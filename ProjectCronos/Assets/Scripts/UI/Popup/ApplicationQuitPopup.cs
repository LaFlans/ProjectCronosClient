using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace ProjectCronos
{
    /// <summary>
    /// アプリケーション終了時のポップアップ
    /// </summary>
    class ApplicationQuitPopup : PopupBase
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
        CanvasGroup canvasGroup;

        bool isSetup = false;
        bool isOpenAnimation = false;

        public override void Setup(Action callback)
        {
            canvasGroup.alpha = 0;

            // メッセージ設定
            titleText.text = "終了確認";
            messageText.text = "ゲームを終了しますか？";
            positiveButtonMessageText.text = "はい";
            negativeButtonMessageText.text = "いいえ";

            // アクション設定
            positiveAction = OnClickPositiveButton;
            negativeAction = OnClickNegativeButton;
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
                GetComponent<Animator>().SetTrigger("Open");
            }
        }

        void OnClickPositiveButton()
        {
            Utility.ApplicationQuit();
        }

        void OnClickNegativeButton()
        {
            Close();
        }
    }
}
