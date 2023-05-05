using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProjectCronos
{
    /// <summary>
    /// ダメージログのセルクラス
    /// </summary>
    public class DamageLogCell : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI playerDamageText;

        [SerializeField]
        TextMeshProUGUI enemyDamageText;

        Animator anim;

        [SerializeField]
        float moveSpeed = 1.0f;

        void Update()
        {
            var pos = this.transform.position;
            pos.y += moveSpeed;
            this.transform.position = pos;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize(string message, EnumCollection.Attack.ATTACK_TYPE type)
        {
            anim = GetComponent<Animator>();
            anim.SetTrigger("In");

            playerDamageText.gameObject.SetActive(false);
            enemyDamageText.gameObject.SetActive(false);
            var isAttackTypePlayer = type == EnumCollection.Attack.ATTACK_TYPE.PLAYER;
            if (isAttackTypePlayer)
            {
                enemyDamageText.gameObject.SetActive(true);
                enemyDamageText.text = message;
            }
            else
            {
                playerDamageText.gameObject.SetActive(true);
                playerDamageText.text = message;
            }

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
