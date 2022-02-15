using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// ��b�X�e�[�^�X�N���X
    /// </summary>
    public class Status : MonoBehaviour
    {
        /// <summary>
        /// HP��\������o�[
        /// </summary>
        [SerializeField]
        HpBar hpBar;

        /// <summary>
        /// �ő�̗�
        /// </summary>
        [SerializeField]
        public int maxHp { get; set; }

        /// <summary>
        /// �c��̗�
        /// </summary>
        [SerializeField]
        public int currntHp { get; set; }

        /// <summary>
        /// �ړ����x
        /// </summary>
        [SerializeField]
        public int moveSpeed { get; set; }

        /// <summary>
        /// �U����
        /// </summary>
        [SerializeField]
        public int attack { get; set; }

        /// <summary>
        /// ���@�U����
        /// </summary>
        [SerializeField]
        public int magicAttack { get; set; }

        /// <summary>
        /// �h���
        /// </summary>
        [SerializeField]
        public int defence { get; set; }

        /// <summary>
        /// ���@�h���
        /// </summary>
        [SerializeField]
        public int magicDefence { get; set; }

        /// <summary>
        /// �N���e�B�J������������m��(��{��0)
        /// </summary>
        [SerializeField]
        public float criticalRate { get; set; }

        /// <summary>
        /// �N���e�B�J���������������ɍU���͂ɂ�����{��
        /// </summary>
        [SerializeField]
        public float criticalDamageRate { get; set; }

        /// <summary>
        /// �J�n����
        /// </summary>
        [SerializeField]
        void Start()
        {
            Initialize();
        }

        /// <summary>
        /// ������
        /// </summary>
        public virtual void Initialize()
        {
            currntHp = 10;
            UpdateHpText();
        }

        /// <summary>
        /// ��e������(HP���O�̏ꍇ�Atrue��Ԃ�)
        /// </summary>
        /// <param name="value">�_���[�W�̒l</param>
        /// <returns>HP�������Ȃ����ꍇtrue</returns>
        public bool Damage(int value)
        {
            currntHp -= value;

            if (currntHp <= 0)
            {
                return true;
            }

            UpdateHpText();

            return false;
        }

        /// <summary>
        /// HP�\���X�V
        /// </summary>
        void UpdateHpText()
        {
            if (hpBar != null)
            {
                hpBar.UpdateHpText(currntHp);
            }
        }
    }
}
