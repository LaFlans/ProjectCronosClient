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
        [SerializeField]
        string enemyName;

        [SerializeField]
        Transform headPos;

        [SerializeField]
        GameObject centerPos;

        [SerializeField]
        Transform underPos;

        public Transform GetCenterPos()
        {
            Debug.Log("transformを返したよ");
            return centerPos.transform;
        }
    }
}
