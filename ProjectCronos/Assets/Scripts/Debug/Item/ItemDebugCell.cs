using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCronos
{
    /// <summary>
    /// アイテムのデバックセル
    /// </summary>
    public class ItemDebugCell : MonoBehaviour
    {
        /// <summary>
        /// アイテム名と個数のテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI itemNameText;

        /// <summary>
        /// アイテム加算ボタン
        /// </summary>
        [SerializeField]
        Button addButton;

        /// <summary>
        /// アイテム減算ボタン
        /// </summary>
        [SerializeField]
        Button subButton;

        int amount;
        public int itemId;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize(int itemId, string itemNameText, int amount, Action addAction, Action subAction)
        {
            this.itemId = itemId;
            this.itemNameText.text = itemNameText + $"({amount})";
            this.amount = amount;

            addButton.onClick.AddListener(() => { addAction?.Invoke(); });
            subButton.onClick.AddListener(() => {  subAction?.Invoke(); });
        }

        /// <summary>
        /// アイテム
        /// </summary>
        /// <param name="itemNameText"></param>
        public void UpdateView(int amount)
        {
            this.amount = amount;
        }
    }
}
