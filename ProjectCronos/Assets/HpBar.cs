using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProjectCronos
{
    public class HpBar : MonoBehaviour
    {
        /// <summary>
        /// HP�\���e�L�X�g
        /// </summary>
        [SerializeField]
        TextMeshPro hpText;

        /// <summary>
        /// �e�L�X�g�X�V
        /// </summary>
        public void UpdateHpText(int value)
        {
            hpText.text = value.ToString();
        }
    }
}
