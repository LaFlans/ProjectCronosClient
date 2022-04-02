using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class EnemyBody : MonoBehaviour
    {
        /// <summary>
        /// �e��
        /// </summary>
        Enemy enemy;
        
        /// <summary>
        /// �_���[�W�{��
        /// </summary>
        [SerializeField]
        int damageRate = 1;

        void Start()
        {
            enemy = this.transform.parent.GetComponent<Enemy>();
        }

        /// <summary>
        /// �_���[�W��^����
        /// </summary>
        public void Damage(int value)
        {
            enemy.Damage(value * damageRate);
        }
    }
}