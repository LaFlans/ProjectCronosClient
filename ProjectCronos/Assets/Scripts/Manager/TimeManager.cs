using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;


namespace ProjectCronos
{
    internal class TimeManager : ISingleton<TimeManager>
    {
        // FIXME: マスタで管理
        const float DEFAULT_TIME_SCALE = 1.0f;

        float timeScale { get; set; }

        public override async UniTask<bool> Initialize()
        {
            ApplyTimeScale();

            Debug.Log("TimeManager初期化");

            return true;
        }

        /// <summary>
        /// 時間を停止する
        /// </summary>
        public void StopTime()
        {
            ApplyTimeScale(0.0f);
        }

        /// <summary>
        /// タイムスケールを適用(デフォルトで設定されているタイムスケールを適用)
        /// </summary>
        public void ApplyTimeScale()
        {
            timeScale = DEFAULT_TIME_SCALE;

            Time.timeScale = timeScale;
        }

        /// <summary>
        /// タイムスケールを適用
        /// </summary>
        /// <param name="value">値</param>
        public void ApplyTimeScale(float value)
        {
            timeScale = value;
            Time.timeScale = timeScale;
        }

        /// <summary>
        /// 現在設定されているタイムスケールの値を取得
        /// </summary>
        /// <returns>タイムスケールの値</returns>
        public float GetTimeScale()
        {
            return timeScale;
        }
    }
}
