using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    /// <summary>
    /// ポップアップのベースとなるクラス
    /// </summary>
    public abstract class PopupBase : MonoBehaviour
    {
        [SerializeField]
        Button[] buttons;

        [SerializeField]
        Image[] selectImages;

        [SerializeField]
        protected Action[] buttonActions;

        /// <summary>
        /// ポップアップを閉じた時のコールバック
        /// </summary>
        protected Action closeAction;

        protected EnumCollection.Popup.POPUP_SELECT_STATUS selectStatus;

        void Start()
        {
            // デフォルトはノーに合わせる
            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE;
            UpdateSelectButtonView();

            // ボタン設定
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].onClick.AddListener(OnClickPositiveButton);
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].onClick.AddListener(OnClickNegativeButton);
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].onClick.AddListener(OnClickOtherButton);
        }

        private void Update()
        {
            // 左操作
            if (Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                UpdateSelectButtonStatusLeft();
                UpdateSelectButtonView();
            }

            // 右操作
            if (Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                UpdateSelectButtonStatusRight();
                UpdateSelectButtonView();
            }

            // 決定キー
            if (Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                buttons[(int)selectStatus].onClick.Invoke();
            }
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
            foreach(var image in selectImages)
            {
                image.enabled = false;
            }

            // 選択中の画像設定
            selectImages[(int)selectStatus].enabled = true;
        }

        public abstract void Setup(Action callback);

        public void ButtonSetup()
        {
            for (int i = 0; i < (int)EnumCollection.Popup.POPUP_SELECT_STATUS.MAXMUM; i++)
            {
                buttons[i].gameObject.SetActive(buttonActions[i] != null);
            }
        }

        void OnClickPositiveButton()
        {
            if (buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE] != null)
            {
                buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].Invoke();
            }
        }

        void OnClickNegativeButton()
        {
            if (buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE] != null)
            {
                buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].Invoke();
            }
        }

        void OnClickOtherButton()
        {
            if (buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER] != null)
            {
                buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].Invoke();
            }
        }

        /// <summary>
        /// ポップアップを閉じる処理
        /// </summary>
        protected void Close()
        {
            if (closeAction != null)
            {
                closeAction.Invoke();
            }

            PopupManager.Instance.PopPopup();
            Destroy(this.gameObject);
        }
    }
}
