using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class Book : MonoBehaviour
    {
        /// <summary>
        /// 弾生成位置オブジェ
        /// </summary>
        [SerializeField]
        Transform bulletSpawnPosObj;

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        /// <param name="targetVec">ターゲットのベクトル</param>
        public void Shot(Vector3 targetVec)
        {
            var bullet = Utility.CreatePrefab("Prefabs/EnergyBall");
            bullet.transform.position = bulletSpawnPosObj.position;
            bullet.GetComponent<Bullet>().Initialize(Vector3.Normalize(targetVec - this.transform.position), 5.0f);
        }
    }
}
