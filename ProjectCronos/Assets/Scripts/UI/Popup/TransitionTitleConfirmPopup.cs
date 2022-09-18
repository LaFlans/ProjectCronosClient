using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

namespace ProjectCronos
{
    /// <summary>
    /// タイトル遷移確認ポップアップ
    /// </summary>
    class TransitionTitleConfirmPopup : PopupBase
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
            titleText.text = MasterDataManager.Instance.GetDic("TransitionTitleConfirmPopupTitle");
            messageText.text = MasterDataManager.Instance.GetDic("TransitionTitleConfirmPopupMessage");
            positiveButtonMessageText.text = MasterDataManager.Instance.GetDic("TransitionTitleConfirmPopupButtonYesMessage");
            negativeButtonMessageText.text = MasterDataManager.Instance.GetDic("TransitionTitleConfirmPopupButtonNoMessage");

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
                GetComponent<Animator>().SetTrigger("Open");
            }
        }

        void OnClickPositiveButton()
        {
            // タイトルに遷移を行う
            SceneLoader.TransitionScene(EnumCollection.Scene.SCENE_TYPE.TITLE);
        }

        void OnClickNegativeButton()
        {
            // 何もしない
            Close();
        }
    }
}
