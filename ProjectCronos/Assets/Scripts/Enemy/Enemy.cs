using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    [RequireComponent(typeof(EnemyStatus))]
    public class Enemy : Character, IEnemy
    {
        /// <summary>
        /// ������
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Debug.Log("�G������");
        }

        /// <summary>
        /// �o��������
        /// </summary>
        public override void Appear()
        {
            Debug.Log("�G�o��");
        }

        /// <summary>
        /// ��e������
        /// </summary>
        /// <param name="value">�_���[�W�̒l</param>
        public override void Damage(int value)
        {
            base.Damage(value);
            Debug.Log("�G��e");
        }

        /// <summary>
        /// ���S������
        /// </summary>
        public override void Death()
        {
            base.Death();

            Debug.Log("�G���S");
            // ���g��j��
            Destroy(this.gameObject);
        }
    }
}
