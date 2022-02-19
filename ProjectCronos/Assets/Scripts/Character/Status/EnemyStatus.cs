using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 敵のステータス
    /// </summary>
    public class EnemyStatus : Status
    {
        [SerializeField]
        public EnemyInfo info { get; }
    }
}
