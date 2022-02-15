using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// �L�����N�^�[(�v���C���[��G�ȂǓ�����S��)�N���X
    /// </summary>
    public class Character : MonoBehaviour
    {
        Status status = null;

        /// <summary>
        /// �J�n����
        /// </summary>
        void Start()
        {
            Initialize();
        }

        /// <summary>
        /// �X�V
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// ������
        /// </summary>
        public virtual void Initialize() 
        {
            status = gameObject.GetComponent<Status>();
        }

        /// <summary>
        /// �o��������
        /// </summary>
        public virtual void Appear()
        {

        }

        /// <summary>
        /// ��e������
        /// </summary>
        public virtual void Damage(int value) 
        {
            // ��U�K���Ȓl����Ƃ�
            if (status.Damage(value))
            {
                Death();
            }
        }

        /// <summary>
        /// ���S������
        /// </summary>
        public virtual void Death()
        {
            // ��U����
            Destroy(this.gameObject);
        }
    }
}
