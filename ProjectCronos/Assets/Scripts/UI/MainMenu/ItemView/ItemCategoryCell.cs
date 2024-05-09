using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace ProjectCronos
{
    /// <summary>
    /// アイテムの種類セル
    /// </summary>
    public class ItemCategoryCell : MonoBehaviour
    {
        /// <summary>
        /// 種類名テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI categoryNameText;

        /// <summary>
        /// 選択状態を表す画像
        /// </summary>
        [SerializeField]
        ToggleObject toggleImage;

        public EnumCollection.Item.ITEM_CATEGORY category;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize(EnumCollection.Item.ITEM_CATEGORY category)
        {
            this.category = category;
            categoryNameText.text = category.ToString();
            toggleImage.SetToggle(false);
        }

        public void UpdateView(EnumCollection.Item.ITEM_CATEGORY currentCategory)
        {
            toggleImage.SetToggle(category == currentCategory);
        }
    }
}
