using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class Book : MonoBehaviour
    {
        /// <summary>
        /// �e�����ʒu�I�u�W�F
        /// </summary>
        [SerializeField]
        Transform bulletSpawnPosObj;

        /// <summary>
        /// �e������
        /// </summary>
        /// <param name="targetVec">�^�[�Q�b�g�̃x�N�g��</param>
        public void Shot(Vector3 targetVec)
        {
            var bullet = Utility.CreatePrefab("Prefabs/EnergyBall");
            bullet.transform.position = bulletSpawnPosObj.position;
            bullet.GetComponent<Bullet>().Initialize(Vector3.Normalize(targetVec - this.transform.position), 5.0f);
        }
    }
}
