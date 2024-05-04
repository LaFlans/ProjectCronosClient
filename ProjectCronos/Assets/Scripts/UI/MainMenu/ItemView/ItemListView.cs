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
            noItemText.gameObject.SetActive(false);
           
            var playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

            if (playerStatus.ownItems != null)
            {
                foreach (var item in playerStatus.ownItems)
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
