using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ProjectCronos
{
    /// <summary>
    /// ギミック(仕掛け)クラス
    /// </summary>
    public abstract class Gimmick : MonoBehaviour
    {
        /// <summary>
        /// ギミックの種類
        /// </summary>
        [SerializeField]
        public EnumCollection.Stage.GIMMICK_TYPE gimmickType;

        /// <summary>
        /// ギミックの状態
        /// </summary>
        public EnumCollection.Stage.GIMMICK_STATUS gimmickStatus;

        abstract public void Initialize(EnumCollection.Stage.GIMMICK_STATUS status);
    }
}
