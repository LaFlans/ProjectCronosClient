using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// “G‚Ìî•ñƒNƒ‰ƒX
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
            Debug.Log("transform‚ğ•Ô‚µ‚½‚æ");
            return centerPos.transform;
        }
    }
}
