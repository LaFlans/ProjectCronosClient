using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace ProjectCronos
{
    internal class ShopPopup : PopupBase
    {
        /// <summary>
        /// 画面のタイトルテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI savePopupTitleText;

        /// <summary>
        /// 操作できるかどうか
        /// </summary>
        bool isOperate;

        Action closeAction;

        /// <summary>
        /// 適用
        /// </summary>
        public async UniTask Apply(Action closeAction)
        {
            isOperate = true;
            this.closeAction = closeAction;
            savePopupTitleText.text = MasterDataManager.Instance.GetDic("ShopPopupTitle");
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="context"></param>
        void OnClose(InputAction.CallbackContext context)
        {
            if (!isOperate)
            {
                return;
            }

            closeAction?.Invoke();

            Debug.Log("閉じるを押したよ！");
            SoundManager.Instance.Play("Button47");

            Close();
        }

        protected override void Close()
        {
            base.Close();
        }

        public override void RegisterInputActions()
        {
            InputManager.Instance.inputActions.UI.Close.performed += OnClose;
        }

        public override void UnregisterInputActions()
        {
            InputManager.Instance.inputActions.UI.Close.performed -= OnClose;
        }
    }
}
