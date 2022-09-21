using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
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
            base.Setup(callback);

            canvasGroup.alpha = 0;

            // メッセージ設定
            titleText.text = MasterDataManager.Instance.GetDic("ApplicationPopupQuitTitle");
            messageText.text = MasterDataManager.Instance.GetDic("ApplicationPopupQuitMessage");
            positiveButtonMessageText.text = MasterDataManager.Instance.GetDic("ApplicationPopupQuitYesButtonMessage");
            negativeButtonMessageText.text = MasterDataManager.Instance.GetDic("ApplicationPopupQuitNoButtonMessage");

            // アクション設定
            buttonActions = new Action[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.MAXMUM];
            buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE] = OnClickPositiveButton;
            buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE] = OnClickNegativeButton;

            closeAction = callback;

            // ボタンの設定
            ButtonSetup();

            isSetup = true;

            if (!isOpenAnimation && isSetup)
            {
                isOpenAnimation = true;
                canvasGroup.alpha = 1;
                SoundManager.Instance.Play("Button30");

                // アニメーション開始
                anim.SetTrigger("Open");
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
