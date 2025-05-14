using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    internal class ShopPopup : PopupBase
    {
        /// <summary>
        /// 画面のタイトルテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI shopPopupTitleText;

        /// <summary>
        /// ショップのアイテム一覧画面
        /// </summary>
        [SerializeField]
        ShopItemListView shopItemListView;

        /// <summary>
        /// 所持金タイトルテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI moneyTitleText;

        /// <summary>
        /// 所持金量テキスト
        /// </summary>
        [SerializeField]
        UnitView moneyUnitView;

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
            shopPopupTitleText.text = MasterDataManager.Instance.GetDic("ShopPopupTitle");

            // プレイヤーのセーブ情報を作成  
            var playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            moneyUnitView.Init(playerStatus.coinNum, isComma: true);

            shopItemListView.Init();
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

            SoundManager.Instance.Play("Button47");
            Close();

            closeAction?.Invoke();
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
