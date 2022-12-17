using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;
using System.Linq;

namespace ProjectCronos
{
    /// <summary>
    /// システムポップアップ
    /// </summary>
    class SystemPopup : PopupBase
    {
        [SerializeField]
        Button[] buttons;

        [SerializeField]
        Image[] selectImages;

        [SerializeField]
        Action[] buttonActions = new Action[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.MAXMUM];

        EnumCollection.Popup.POPUP_SELECT_STATUS selectStatus;

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

        public override void Setup(EnumCollection.Popup.POPUP_BUTTON_STATUS buttonStatus, Param param = null, MessageParam messageParam = null)
        {
            base.Setup(buttonStatus);

            // デフォルトはノーに合わせる
            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE;
            UpdateSelectButtonView();

            // ボタン設定
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].onClick.AddListener(OnClickPositiveButton);
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].onClick.AddListener(OnClickNegativeButton);
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].onClick.AddListener(OnClickOtherButton);

            //　入力設定
            InputManager.Instance.inputActions.UI.Submit.performed += OnSubmit;
            InputManager.Instance.inputActions.UI.Left.performed += OnLeft;
            InputManager.Instance.inputActions.UI.Right.performed += OnRight;

            canvasGroup.alpha = 0;

            // メッセージ設定
            titleText.text = messageParam?.title != null ? messageParam.title : "仮_タイトル";
            messageText.text = messageParam?.message != null ? messageParam.message : "仮_メッセージ";
            positiveButtonMessageText.text = messageParam?.positiveButton != null ? messageParam.positiveButton : "仮_はい";
            negativeButtonMessageText.text = messageParam?.negativeButton != null ? messageParam.negativeButton : "仮_いいえ";
            otherButtonMessageText.text = messageParam?.otherButton != null ? messageParam.otherButton : "仮_その他";

            // アクション設定
            buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE] = param.positiveAction;
            buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE] = param.negativeAction;
            buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER] = param.otherAction;

            closeAction = param.closeAction;

            // ボタンの設定
            ButtonSetup(buttonStatus);

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

        void OnDestroy()
        {
            Close();
        }

        /// <summary>
        /// 左選択
        /// </summary>
        /// <param name="context"></param>
        void OnLeft(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Button47");

            UpdateSelectButtonStatusLeft();
            UpdateSelectButtonView();
        }

        /// <summary>
        /// 右選択
        /// </summary>
        /// <param name="context"></param>
        void OnRight(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Button47");

            UpdateSelectButtonStatusRight();
            UpdateSelectButtonView();
        }

        /// <summary>
        /// 決定処理
        /// </summary>
        /// <param name="context"></param>
        void OnSubmit(InputAction.CallbackContext context)
        {
            buttons[(int)selectStatus].onClick.Invoke();
        }

        /// <summary>
        /// 左を押した時のボタン選択の更新
        /// </summary>
        void UpdateSelectButtonStatusLeft()
        {
            if (buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].gameObject.activeSelf)
            {
                if (buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].gameObject.activeSelf)
                {
                    switch (selectStatus)
                    {
                        case EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE:
                            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER;
                            break;
                        case EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE:
                            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE;
                            break;
                        case EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER:
                            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE;
                            break;
                    }
                }
                else
                {
                    switch (selectStatus)
                    {
                        case EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE:
                            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE;
                            break;
                        case EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE:
                            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE;
                            break;
                    }
                }
            }
            else
            {
                selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE;
            }
        }

        /// <summary>
        /// 右を押した時のボタン選択の更新
        /// </summary>
        void UpdateSelectButtonStatusRight()
        {
            if (buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].gameObject.activeSelf)
            {
                if (buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].gameObject.activeSelf)
                {
                    switch (selectStatus)
                    {
                        case EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE:
                            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE;
                            break;
                        case EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE:
                            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER;
                            break;
                        case EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER:
                            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE;
                            break;
                    }
                }
                else
                {
                    switch (selectStatus)
                    {
                        case EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE:
                            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE;
                            break;
                        case EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE:
                            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE;
                            break;
                    }
                }
            }
            else
            {
                selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE;
            }
        }

        /// <summary>
        /// 選択しているボタンの表示更新
        /// </summary>
        void UpdateSelectButtonView()
        {
            // 一旦すべての選択中画像非表示
            foreach (var image in selectImages)
            {
                image.enabled = false;
            }

            // 選択中の画像設定
            selectImages[(int)selectStatus].enabled = true;
        }

        public void ButtonSetup(EnumCollection.Popup.POPUP_BUTTON_STATUS status)
        {
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].gameObject.SetActive(false);
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].gameObject.SetActive(false);
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].gameObject.SetActive(false);

            switch (status)
            {
                case EnumCollection.Popup.POPUP_BUTTON_STATUS.DEFAULT:
                    buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].gameObject.SetActive(true);
                    buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].gameObject.SetActive(true);
                    break;
                case EnumCollection.Popup.POPUP_BUTTON_STATUS.ALL:
                    buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].gameObject.SetActive(true);
                    buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].gameObject.SetActive(true);
                    buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].gameObject.SetActive(true);
                    break;
                case EnumCollection.Popup.POPUP_BUTTON_STATUS.POSITIVE_ONLY:
                    buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].gameObject.SetActive(true);
                    break;
                case EnumCollection.Popup.POPUP_BUTTON_STATUS.NEGATIVE_ONLY:
                    buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].gameObject.SetActive(true);
                    break;
                case EnumCollection.Popup.POPUP_BUTTON_STATUS.OTHER_ONLY:
                    buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        void OnClickPositiveButton()
        {
            if (buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE] != null)
            {
                SoundManager.Instance.Play("Button38");

                buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].Invoke();
            }

            // デフォルトの挙動としてクローズ処理を行う
            Close();
        }

        void OnClickNegativeButton()
        {
            if (buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE] != null)
            {
                SoundManager.Instance.Play("Button37");

                buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].Invoke();
            }

            // デフォルトの挙動としてクローズ処理を行う
            Close();
        }

        void OnClickOtherButton()
        {
            if (buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER] != null)
            {
                SoundManager.Instance.Play("Button37");

                buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].Invoke();
            }

            // デフォルトの挙動としてクローズ処理を行う
            Close();
        }

        protected override void Close()
        {
            // ボタンイベント削除
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].onClick.RemoveAllListeners();
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].onClick.RemoveAllListeners();
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].onClick.RemoveAllListeners();

            //　入力設定
            InputManager.Instance.inputActions.UI.Submit.performed -= OnSubmit;
            InputManager.Instance.inputActions.UI.Left.performed -= OnLeft;
            InputManager.Instance.inputActions.UI.Right.performed -= OnRight;

            base.Close();
        }
    }
}
