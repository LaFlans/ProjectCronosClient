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
        /// �A�j���[�V�����J�n��
        /// </summary>
        public void AnimationStart()
        {
            onStart.Invoke();
        }

        /// <summary>
        /// �A�j���[�V�����I����
        /// </summary>
        public void AnimationFinish()
        {
            onFinish.Invoke();
        }
    }
}
