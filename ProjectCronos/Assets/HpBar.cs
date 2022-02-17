using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProjectCronos
{
    public class HpBar : MonoBehaviour
    {
        /// <summary>
        /// HP表示テキスト
        /// </summary>
        [SerializeField]
        TextMeshPro hpText;

        /// <summary>
        /// テキスト更新
        /// </summary>
        public void UpdateHpText(int value)
        {
            hpText.text = value.ToString();
        }
    }
}
