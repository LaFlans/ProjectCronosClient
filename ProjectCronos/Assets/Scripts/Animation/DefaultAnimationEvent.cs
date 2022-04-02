using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjectCronos
{
    public class DefaultAnimationEvent : MonoBehaviour
    {
        Action onStart;
        Action onFinish;

        public void Init(Action startAction = null, Action finishAction = null)
        {
            onStart = startAction;
            onFinish = finishAction;
        }

        /// <summary>
        /// アニメーション開始時
        /// </summary>
        public void AnimationStart()
        {
            onStart.Invoke();
        }

        /// <summary>
        /// アニメーション終了時
        /// </summary>
        public void AnimationFinish()
        {
            onFinish.Invoke();
        }
    }
}
