using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEditor;


namespace ProjectCronos
{
    public class ScenarioChoiceButton : MonoBehaviour
    {
        /// <summary>
        /// メッセージテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI messageText;

        [SerializeField]
        Button button;
        [SerializeField]
        Animator anim;

        /// <summary>
        /// ボタン押下時のアクション
        /// </summary>
        Action onClick;

        public void Init(string message, Action onClick)
        {
            messageText.text = message;
            this.onClick = onClick;
            anim.SetTrigger("In");

            // 全てのイベントを削除してから登録
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { this.onClick(); });
        }

        public void SelectButton()
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }

        public void SetSelectAnim(bool isSelect)
        {
            anim.SetBool("IsSelect", isSelect);
        }
    }
}
