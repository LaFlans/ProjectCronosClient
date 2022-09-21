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
        protected Action[] buttonActions = new Action[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.MAXMUM];

        /// <summary>
        /// ポップアップを閉じた時のコールバック
        /// </summary>
        protected Action closeAction;

        protected EnumCollection.Popup.POPUP_SELECT_STATUS selectStatus;

        protected Animator anim;

        void Start()
        {
            // デフォルトはノーに合わせる
            selectStatus = EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE;
            UpdateSelectButtonView();

            // ボタン設定
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].onClick.AddListener(OnClickPositiveButton);
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].onClick.AddListener(OnClickNegativeButton);
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].onClick.AddListener(OnClickOtherButton);

            //　入力設定
            InputManager.instance.inputActions.UI.Submit.performed += OnSubmit;
            InputManager.instance.inputActions.UI.Left.performed += OnLeft;
            InputManager.instance.inputActions.UI.Right.performed += OnRight;
        }

        void OnDestroy()
        {
            //　入力設定
            InputManager.instance.inputActions.UI.Submit.performed -= OnSubmit;
            InputManager.instance.inputActions.UI.Left.performed -= OnLeft;
            InputManager.instance.inputActions.UI.Right.performed -= OnRight;
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

        public virtual void Setup(Action callback)
        {
            // アニメーター設定
            // ポップアップのアニメーションは非スケール時間対象
            anim = GetComponent<Animator>();
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

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
                SoundManager.Instance.Play("Button38");

                buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].Invoke();
            }
        }

        void OnClickNegativeButton()
        {
            if (buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE] != null)
            {
                SoundManager.Instance.Play("Button37");

                buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].Invoke();
            }
        }

        void OnClickOtherButton()
        {
            if (buttonActions[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER] != null)
            {
                SoundManager.Instance.Play("Button37");

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

            // ボタンイベント削除
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.POSITIVE].onClick.RemoveAllListeners();
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.NEGATIVE].onClick.RemoveAllListeners();
            buttons[(int)EnumCollection.Popup.POPUP_SELECT_STATUS.OTHER].onClick.RemoveAllListeners();

            //　入力設定
            InputManager.instance.inputActions.UI.Submit.performed -= OnSubmit;
            InputManager.instance.inputActions.UI.Left.performed -= OnLeft;
            InputManager.instance.inputActions.UI.Right.performed -= OnRight;

            PopupManager.Instance.PopPopup();
            Destroy(this.gameObject);
        }
    }
}
