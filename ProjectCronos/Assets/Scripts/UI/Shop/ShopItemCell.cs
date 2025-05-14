using TMPro;
using UnityEngine;
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
        /// アイテム名
        /// </summary>
        string itemName;

        /// <summary>
        /// アイテムの値段
        /// </summary>
        int itemPrice;

        /// <summary>
        /// 選択中かどうか
        /// </summary>
        bool isSelect;

        [SerializeField]
        TextMeshProUGUI nameText;

        [SerializeField]
        TextMeshProUGUI priceText;

        [SerializeField]
        UnitView unitView;

        public void Init(
            string itemName,
            int itemPrice)
        {
            this.itemName = itemName;
            this.itemPrice = itemPrice;
            unitView.Init(this.itemPrice);
            UpdateView();
        }

        public void UpdateView()
        {
            nameText.text = itemName;
        }
    }
}
