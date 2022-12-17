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
        public class Param
        {
            public Action positiveAction;
            public Action negativeAction;
            public Action otherAction;
            public Action closeAction;

            public Param(Action positiveAction = null, Action negativeAction = null, Action otherAction = null, Action closeAction = null)
            {
                this.positiveAction = positiveAction;
                this.negativeAction = negativeAction;
                this.otherAction = otherAction;
                this.closeAction = closeAction;
            }
        }

        public class MessageParam
        {
            public string title;
            public string message;
            public string positiveButton;
            public string negativeButton;
            public string otherButton;

            public MessageParam(
                string title = null,
                string message = null,
                string positiveButton = null,
                string negativeButton = null,
                string otherButton = null)
            {
                this.title = title;
                this.message = message;
                this.positiveButton = positiveButton;
                this.negativeButton = negativeButton;
                this.otherButton = otherButton;
            }
        }

        /// <summary>
        /// ポップアップを閉じた時のコールバック
        /// </summary>
        protected Action closeAction;

        protected Animator anim;

        public virtual void Setup(EnumCollection.Popup.POPUP_BUTTON_STATUS buttonStatus, Param param = null, MessageParam messageParam = null)
        {
            Debug.Log("ベースポップアップを作成");

            // アニメーター設定
            // ポップアップのアニメーションは非スケール時間対象
            anim = GetComponent<Animator>();
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        /// <summary>
        /// ポップアップを閉じる処理
        /// </summary>
        protected virtual void Close()
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
