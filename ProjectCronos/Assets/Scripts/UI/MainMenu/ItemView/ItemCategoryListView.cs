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

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            // 生成前に初期化しておく
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var i in Enum.GetValues(typeof(EnumCollection.Item.ITEM_CATEGORY)))
            {
                var obj = Instantiate(cell, parent.transform);
                obj.GetComponent<ItemCategoryCell>().Initialize(i.ToString());
            }
        }
    }
}
