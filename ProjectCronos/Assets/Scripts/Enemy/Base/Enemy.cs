using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectCronos
{
    [RequireComponent(typeof(EnemyStatus))]
    public class Enemy : Character, IEnemy
    {
        public GameObject target;
        private NavMeshAgent agent;

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            //Debug.Log("敵初期化");

            agent = GetComponent<NavMeshAgent>();      
        }

        void Update()
        {
            agent.SetDestination(target.transform.position);
        }

        /// <summary>
        /// 出現時処理
        /// </summary>
        public override void Appear()
        {
            //Debug.Log("敵出現");
        }

        /// <summary>
        /// 被弾時処理
        /// </summary>
        /// <param name="value">ダメージの値</param>
        public override void Damage(int value)
        {
            base.Damage(value);
            //Debug.Log("敵被弾");
        }

        /// <summary>
        /// 死亡時処理
        /// </summary>
        public override void Death()
        {
            base.Death();

            //Debug.Log("敵死亡");
            // 自身を破壊
            Destroy(this.gameObject);
        }
    }
}
