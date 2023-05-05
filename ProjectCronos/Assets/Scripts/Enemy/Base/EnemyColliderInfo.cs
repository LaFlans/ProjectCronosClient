using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 敵の当たり判定情報クラス
    /// </summary>
    public class EnemyColliderInfo : MonoBehaviour
    {
        [Serializable]
        public class AttackTriggerArray
        {
            [SerializeField]
            AttackTrigger[] attackTriggers;

            /// <summary>
            /// 初期化
            /// </summary>
            /// <param name="type">攻撃の種類</param>
            /// <param name="attackVal">攻撃の値</param>
            /// <param name="isHitDestroy">ヒットした時にオブジェクトを消すか</param>
            public void Init(EnumCollection.Attack.ATTACK_TYPE type, int attackVal, bool isHitDestroy)
            {
                foreach (var trigger in attackTriggers)
                {
                    trigger.Init(type, attackVal, isHitDestroy);
                }
            }

            /// <summary>
            /// 当たり判定を有効化
            /// </summary>
            public void EnableCollider()
            {
                foreach (var trigger in attackTriggers)
                {
                    trigger.EnableCollider();
                }
            }

            /// <summary>
            /// 当たり判定を無効化
            /// </summary>
            public void DisableCollider()
            {
                foreach(var trigger in attackTriggers)
                {
                    trigger.DisableCollider();
                }
            }
        }

        public AttackTriggerArray[] attackTriggerArray;

        public void Init(int attackVal)
        {
            foreach (var attackTrigger in attackTriggerArray)
            {
                attackTrigger.Init(EnumCollection.Attack.ATTACK_TYPE.ENEMY, attackVal, false);
            }
        }

        /// <summary>
        /// 指定した要素の当たり判定を有効化
        /// </summary>
        /// <param name="elementId">要素数</param>
        public void EnableCollider(int elementId)
        {
            if (attackTriggerArray.Length > 0 && attackTriggerArray.Length > elementId)
            {
                attackTriggerArray[elementId].EnableCollider();
            }
        }

        /// <summary>
        /// 指定した要素の当たり判定を無効化
        /// </summary>
        /// <param name="elementId">要素数</param>
        public void DisableCollider(int elementId)
        {
            if (attackTriggerArray.Length > 0 && attackTriggerArray.Length > elementId)
            {
                attackTriggerArray[elementId].DisableCollider();
            }
        }

        /// <summary>
        /// 登録済みのすべての当たり判定を有効化
        /// </summary>
        public void EnableAllColliders()
        {
            foreach (var trigger in attackTriggerArray)
            {
                trigger.EnableCollider();
            }
        }

        /// <summary>
        /// すべての当たり判定を無効化
        /// </summary>
        public void DisableAllColliders()
        {
            foreach (var trigger in attackTriggerArray)
            {
                trigger.DisableCollider();
            }
        }
    }
}
