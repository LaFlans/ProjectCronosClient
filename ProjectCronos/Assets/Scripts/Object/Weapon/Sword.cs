using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// �����̕�����̊��N���X
    /// </summary>
    public class Sword : Weapon
    {
        /// <summary>
        /// �����蔻��R���|�[�l���g
        /// </summary>
        Collider col;

        /// <summary>
        /// �J�n����
        /// </summary>
        void Start()
        {
            col = GetComponent<Collider>();
            if(col == null)
            {
                Debug.Log("���̓����蔻��R���|�[�l���g���ݒ肳��Ă��܂���");
            }
        }

        /// <summary>
        /// �����蔻���L���ɂ���
        /// </summary>
        public void EnableCollider()
        {
            if (!col.enabled) col.enabled = true;
        }

        /// <summary>
        /// �����蔻��𖳌��ɂ���
        /// </summary>
        public void DisableCollider()
        {
            if(col.enabled) col.enabled = false;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<Enemy>().Damage(1);
                Utility.CreatePrefab("Prefabs/BulletHitEffect", this.transform.position, 0.5f);
            }
        }
    }
}
