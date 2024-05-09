using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// アイテムの種類一覧の画面
    /// </summary>
    public class ItemCategoryListView : MonoBehaviour
    {
        /// <summary>
        /// セルを生成する親オブジェクト
        /// </summary>
        [SerializeField]
        GameObject parent;

        /// <summary>
        /// 生成するセル
        /// </summary>
        [SerializeField]
        GameObject cell;

        List<ItemCategoryCell> itemCategoryCells;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            itemCategoryCells = new List<ItemCategoryCell>();

            // 生成前に初期化しておく
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var i in Enum.GetValues(typeof(EnumCollection.Item.ITEM_CATEGORY)))
            {
                var category = (EnumCollection.Item.ITEM_CATEGORY)i;
                if (category != EnumCollection.Item.ITEM_CATEGORY.MAXMUM)
                {
                    var obj = Instantiate(cell, parent.transform);
                    var itemCategoryCell = obj.GetComponent<ItemCategoryCell>();
                    itemCategoryCell.Initialize((EnumCollection.Item.ITEM_CATEGORY)i);
                    itemCategoryCells.Add(itemCategoryCell);
                }
            }
        }

        /// <summary>
        /// アイテムの種類一覧の更新
        /// </summary>
        /// <param name="currentCategory">現在選択中のカテゴリ</param>
        public void UpdateView(EnumCollection.Item.ITEM_CATEGORY currentCategory)
        {
            foreach (var cell in itemCategoryCells)
            {
                cell.UpdateView(currentCategory);
            }
        }
    }
}
