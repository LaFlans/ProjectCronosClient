using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 剣等の武器回りの基底クラス
    /// </summary>
    public class Sword : Weapon
    {
        /// <summary>
        /// 当たり判定コンポーネント
        /// </summary>
        Collider col;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            col = GetComponent<Collider>();
            if(col == null)
            {
                Debug.Log("剣の当たり判定コンポーネントが設定されていません");
            }
        }

        /// <summary>
        /// 当たり判定を有効にする
        /// </summary>
        public void EnableCollider()
        {
            if (!col.enabled) col.enabled = true;
        }

        /// <summary>
        /// 当たり判定を無効にする
        /// </summary>
        public void DisableCollider()
        {
            if(col.enabled) col.enabled = false;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<EnemyBody>().Damage(1);
                Vector3 hitPos = col.ClosestPointOnBounds(this.transform.position);
                Utility.CreateObject("Prefabs/DamageEffect1", hitPos, 1.0f);
            }
        }
    }
}