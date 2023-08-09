using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.VFX;

namespace ProjectCronos
{
    public class FireSkelton : AttackTrigger
    {
        public VisualEffect effect;

        float tempPlayRate;

        void Start()
        {
            if (effect != null)
            {
                tempPlayRate = effect.playRate;

                // オブジェクトのタイムスケールの値更新時イベント設定
                TimeManager.Instance.RegisterObjectTimeScaleApplyAction(OnObjectTimeScaleApply);
            }
        }

        private void OnDestroy()
        {
            if (effect != null)
            {
                // オブジェクトのタイムスケールの値更新時イベント設定
                TimeManager.Instance.UnregisterObjectTimeScaleApplyAction(OnObjectTimeScaleApply);
            }
        }

        /// <summary>
        /// オブジェクト(プレイヤーの攻撃等)のタイムスケールの値更新時イベント
        /// </summary>
        void OnObjectTimeScaleApply()
        {
            var result = TimeManager.Instance.GetObjectTimeScale() > 0;
            if (result)
            {
                effect.playRate = tempPlayRate;
            }
            else
            {
                tempPlayRate = effect.playRate;
                effect.playRate = 0;
            }
        }
    }
}
