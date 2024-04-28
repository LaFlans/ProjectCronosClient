using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// ギミック(仕掛け)クラス
    /// </summary>
    public class Gimmick : MonoBehaviour
    {
        /// <summary>
        /// ギミックの種類
        /// </summary>
        [SerializeField]
        EnumCollection.Stage.GIMMICK_TYPE gimmickType;

        /// <summary>
        /// アニメーター
        /// </summary>
        [SerializeField]
        Animator anim;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            gimmickType = EnumCollection.Stage.GIMMICK_TYPE.Switch;
        }

        public void Open()
        {
            anim.SetTrigger("Open");
        }

        public void Close()
        {
            anim.SetTrigger("Close");
        }
    }
}
