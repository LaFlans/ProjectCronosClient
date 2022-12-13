using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProjectCronos
{
    /// <summary>
    /// 入手したアイテムログのセルクラス
    /// </summary>
    public class ItemLogCell : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI text;

        Animator anim;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize(string message)
        {
            anim = GetComponent<Animator>();
            anim.SetTrigger("In");

            text.text = message;
            //Destroy(this.gameObject, 20.0f);

            Invoke("DestroyLog", 3.0f);
        }

        void DestroyLog()
        {
            anim.SetTrigger("Out");
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}
