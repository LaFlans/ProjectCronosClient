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

        float hpRate;
        float hpMax;

        /// <summary>
        /// テキスト更新
        /// </summary>
        public void UpdateHpText(int value, int hpMax)
        {
            hpText.text = value.ToString();

            // 3Dバー表示対応
            var scale = this.transform.localScale;
            scale.x = (float)value / (float)hpMax;
            this.transform.localScale = scale;
        }
    }
}
