using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;
using UnityEditorInternal.Profiling.Memory.Experimental;

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
        /// アイテム詳細画面
        /// </summary>
        [SerializeField]
        ItemDetailView itemDetailView;

        /// <summary>
        /// 操作できるかどうか
        /// </summary>
        bool isOperate;

        int shopGroupId;
        float priceRate;
        Action closeAction;

        PlayerStatus playerStatus;

        /// <summary>
        /// 適用
        /// </summary>
        public async UniTask Apply(int shopGroupId, float priceRate, Action closeAction)
        {
            popupCanvasGroup = GetComponent<CanvasGroup>();

            isOperate = true;
            this.closeAction = closeAction;
            this.shopGroupId = shopGroupId;
            this.priceRate = priceRate;
            shopPopupTitleText.text = MasterDataManager.Instance.GetDic("ShopPopupTitle");

            // プレイヤーのセーブ情報を作成  
            playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            moneyUnitView.Init(playerStatus.coinNum, isComma: true);

            shopItemListView.Init(PurchaseItem, UpdateItemDetailView, this.priceRate, shopGroupId);

            RegisterInputActions();
        }

        /// <summary>
        /// アイテムの詳細画面の更新
        /// </summary>
        public void UpdateItemDetailView(int itemId)
        {
            var itemHolder = playerStatus.itemHolder;
            var info = itemHolder.GetItemDetailInfo(itemId);
            itemDetailView.UpdateView(info, itemHolder.GetHoldItemCount(itemId));
        }

        /// <summary>
        /// アイテムを購入
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount"></param>
        /// <returns>アイテムの購入に成功した場合、Trueを返す</returns>
        public bool PurchaseItem((int itemId, int amount) info)
        {
            var item = MasterDataManager.DB.ItemDataTable.FindById(info.itemId);
            var sum = (item.BasePrice * priceRate) * info.amount;

            if (playerStatus.ConsumeCoin((int)sum))
            {
                if (playerStatus != null)
                {
                    playerStatus.itemHolder.AddItem(info.itemId, info.amount);
                }

                Debug.Log($"{sum}円のアイテム({item.Name})を購入したよ");
                moneyUnitView.ApplyView(playerStatus.coinNum);

                return true;
            }

            Debug.Log($"{playerStatus.coinNum - sum}不足しているよ");
            return false;
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
            if (popupCanvasGroup == null)
            {
                popupCanvasGroup = GetComponent<CanvasGroup>();
            }

            popupCanvasGroup.interactable = true;
            shopItemListView.SelectedItemCell();

            InputManager.Instance.inputActions.UI.Close.performed += OnClose;
        }

        public override void UnregisterInputActions()
        {
            if (popupCanvasGroup == null)
            {
                popupCanvasGroup = GetComponent<CanvasGroup>();
            }

            popupCanvasGroup.interactable = false;

            InputManager.Instance.inputActions.UI.Close.performed -= OnClose;
        }
    }
}
