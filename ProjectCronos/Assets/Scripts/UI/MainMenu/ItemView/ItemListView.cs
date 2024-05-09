using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectCronos
{
    public class ItemListView : MonoBehaviour
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
        /// アイテムがない時のテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI noItemText;

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

            noItemText.gameObject.SetActive(false);
           
            var playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

            if (playerStatus.itemHolder.IsHoldItems())
            {
                foreach (var item in playerStatus.itemHolder.ownItems)
                {
                    var obj = Instantiate(cell, parent.transform);
                    obj.GetComponent<ItemCell>().Initialize(item.Key, item.Value);
                }
            }
            else
            {
                noItemText.gameObject.SetActive(true);
            }
        }
    }
}
