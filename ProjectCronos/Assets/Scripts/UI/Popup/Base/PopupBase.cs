using UnityEngine;
using UnityEngine.UI;
using System;

namespace ProjectCronos
{
    /// <summary>
    /// ポップアップのベースとなるクラス
    /// </summary>
    public abstract class PopupBase : MonoBehaviour
    {
        [SerializeField]
        Button positiveButton;
        [SerializeField]
        Button negativeButton;
        [SerializeField]
        Button otherButton;

        /// <summary>
        /// ポジティブボタンを押した時のコールバック
        /// </summary>
        protected Action positiveAction;
        /// <summary>
        /// ネガティブボタンを押した時のコールバック
        /// </summary>
        protected Action negativeAction;
        /// <summary>
        /// その他ボタンを押した時のコールバック
        /// </summary>
        protected Action otherAction;
        /// <summary>
        /// ポップアップを閉じた時のコールバック
        /// </summary>
        protected Action closeAction;

        void Start()
        {
            positiveButton.onClick.AddListener(OnClickPositiveButton);
            negativeButton.onClick.AddListener(OnClickNegativeButton);
            otherButton.onClick.AddListener(OnClickOtherButton);
        }

        public abstract void Setup(Action callback);

        public void ButtonSetup()
        {
            bool isExistPositiveAction = positiveAction != null;
            bool isExistNegativeAction = negativeAction != null;
            bool isExistOtherAction = otherAction != null;

            positiveButton.gameObject.SetActive(isExistPositiveAction);
            negativeButton.gameObject.SetActive(isExistNegativeAction);
            otherButton.gameObject.SetActive(isExistOtherAction);
        }

        void OnClickPositiveButton()
        {
            if (positiveAction != null)
            {
                positiveAction.Invoke();
            }
        }

        void OnClickNegativeButton()
        {
            if (negativeAction != null)
            {
                negativeAction.Invoke();
            }
        }

        void OnClickOtherButton()
        {
            if (otherButton != null)
            {
                otherAction.Invoke();
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
