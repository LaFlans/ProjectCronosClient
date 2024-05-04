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
        /// 初期化
        /// </summary>
        public void Initialize(string name)
        {
            categoryNameText.text = name;
        }
    }
}
