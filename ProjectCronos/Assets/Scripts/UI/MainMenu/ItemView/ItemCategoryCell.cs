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

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize(string name)
        {
            categoryNameText.text = name;
            toggleImage.SetToggle(false);
        }
    }
}
