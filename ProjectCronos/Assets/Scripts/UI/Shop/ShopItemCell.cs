using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectCronos
{
    class ShopItemCell : MonoBehaviour
    {
        /// <summary>
        /// NodeのIndex番号
        /// </summary>
        public int NodeNumber { get { return transform.GetSiblingIndex(); } }

        /// <summary>
        /// アイテムID
        /// </summary>
        int itemId;

        /// <summary>
        /// アイテム名
        /// </summary>
        string itemName;

        /// <summary>
        /// アイテムの値段
        /// </summary>
        int itemPrice;

        /// <summary>
        /// アイテム購入処理
        /// </summary>
        Func<(int,int), bool> purchaceItemFunc;

        [SerializeField]
        TextMeshProUGUI nameText;

        [SerializeField]
        UnitView unitView;

        Button button;

        public void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClickButton);
        }

        public void Init(
            int itemId,
            string itemName,
            int itemPrice,
            Func<(int, int), bool> purchaceItemFunc)
        {
            this.itemId = itemId;
            this.itemName = itemName;
            this.itemPrice = itemPrice;
            this.purchaceItemFunc = purchaceItemFunc;
            unitView.Init(this.itemPrice);
            UpdateView();
        }

        int test(float t)
        {
            return 1;
        }

        void OnClickButton()
        {
            var obj = PopupManager.Instance.GetPopupObject(
                EnumCollection.Popup.POPUP_TYPE.DEFAULT);

            if (obj != null)
            {
                obj.GetComponent<PopupBase>().Setup(
                    EnumCollection.Popup.POPUP_BUTTON_STATUS.DEFAULT,
                    new PopupBase.Param(
                        () =>
                        {
                            if (purchaceItemFunc((itemId, 1)))
                            {
                                // 購入成功処理
                                PopupManager.Instance.ShowSystemPopup(new PopupBase.MessageParam("購入完了", $"{itemName}を購入しました。", "はい"));
                            }
                            else
                            {
                                // 購入失敗処理
                                PopupManager.Instance.ShowSystemPopup(new PopupBase.MessageParam("購入失敗", "所持金が足りませんでした。", "はい"));
                            }
                        },
                        null, null, () => {}),
                    new PopupBase.MessageParam(
                        "購入確認",
                        $"{itemName}を購入しますか？",
                        "はい",
                        "いいえ",
                        "その他"));
            }

            Debug.LogError($"{itemId}:{itemName}:{itemPrice}");
        }

        public void UpdateView()
        {
            nameText.text = itemName;
        }
    }
}
