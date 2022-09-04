using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class EnemyBody : MonoBehaviour
    {
        /// <summary>
        /// 親
        /// </summary>
        Enemy enemy;
        
        /// <summary>
        /// ダメージ倍率
        /// </summary>
        [SerializeField]
        int damageRate = 1;

        void Start()
        {
            enemy = this.transform.parent.GetComponent<Enemy>();
        }

        /// <summary>
        /// ダメージを与える
        /// </summary>
        public void Damage(int value)
        {
            enemy.Damage(value * damageRate);
        }
    }
}
