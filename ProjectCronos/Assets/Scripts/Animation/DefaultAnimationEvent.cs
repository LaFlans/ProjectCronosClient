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

        /// <summary>
        /// エクストライベント
        /// </summary>
        Action onExtension1;
        Action onExtension2;
        Action onExtension3;

        public void Init(Action startAction = null,
            Action finishAction = null,
            Action extension1Action = null,
            Action extension2Action = null,
            Action extension3Action = null)
        {
            onStart = startAction;
            onFinish = finishAction;
            onExtension1 = extension1Action;
            onExtension2 = extension2Action;
            onExtension3 = extension3Action;
        }

        /// <summary>
        /// アニメーション開始時
        /// </summary>
        public void AnimationStart()
        {
            onStart?.Invoke();
        }

        /// <summary>
        /// アニメーション終了時
        /// </summary>
        public void AnimationFinish()
        {
            onFinish?.Invoke();
        }

        /// <summary>
        /// 拡張アニメーションイベント１
        /// </summary>
        public void AnimationExtension1()
        {
            onExtension1?.Invoke();
        }

        /// <summary>
        /// 拡張アニメーションイベント２
        /// </summary>
        public void AnimationExtension2()
        {
            onExtension2?.Invoke();
        }

        /// <summary>
        /// 拡張アニメーションイベント３
        /// </summary>
        public void AnimationExtension3()
        {
            onExtension3?.Invoke();
        }
    }
}
