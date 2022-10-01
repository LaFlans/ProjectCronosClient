using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 敵の情報クラス
    /// </summary>
    public class EnemyInfo : MonoBehaviour
    {
        string enemyName;

        [SerializeField]
        Transform headPos;

        [SerializeField]
        GameObject centerPos;

        [SerializeField]
        Transform underPos;

        public Transform GetHeadPos()
        {
            return headPos;
        }

        public Transform GetCenterPos()
        {
            return centerPos.transform;
        }
    }
}
