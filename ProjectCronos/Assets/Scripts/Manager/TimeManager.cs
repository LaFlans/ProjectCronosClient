using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;


namespace ProjectCronos
{
    internal class TimeManager : ISingleton<TimeManager>
    {
        // FIXME: マスタで管理
        const float DEFAULT_TIME_SCALE = 1.0f;
        const float DEFAULT_PLAYER_TIME_SCALE = 1.0f;
        const float DEFAULT_ENEMY_TIME_SCALE = 1.0f;

        /// <summary>
        /// ゲーム全体のタイムスケール
        /// </summary>
        float timeScale;

        /// <summary>
        /// プレイヤーのタイムスケール
        /// </summary>
        float playerTimeScale;

        /// <summary>
        /// 敵のタイムスケール
        /// </summary>
        float enemyTimeScale;

        /// <summary>
        /// 敵のタイムスケール変化した時のイベント
        /// </summary>
        Action enemyTimeScaleApplyAction;

        public override async UniTask<bool> Initialize()
        {
            InitTimeScale();
            ApplyTimeScale();

            Debug.Log("TimeManager初期化");

            return true;
        }

        /// <summary>
        /// タイムスケールの初期化
        /// </summary>
        public void InitTimeScale()
        {
            timeScale = DEFAULT_TIME_SCALE;
            playerTimeScale = DEFAULT_PLAYER_TIME_SCALE;
            enemyTimeScale = DEFAULT_ENEMY_TIME_SCALE;
        }

        /// <summary>
        /// 時間を停止する
        /// </summary>
        public void StopTime()
        {
            ApplyTimeScale(0.0f);
        }

        /// <summary>
        /// ゲーム全体のタイムスケールを適用(デフォルトで設定されているタイムスケールを適用)
        /// </summary>
        public void ApplyTimeScale()
        {
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
        /// 敵のタイムスケールを適用
        /// </summary>
        /// <param name="value">値</param>
        public void ApplyEnemyTimeScale(float value)
        {
            enemyTimeScale = value;

            // 敵のタイムスケール変更時処理を行う
            enemyTimeScaleApplyAction?.Invoke();

            if (enemyTimeScaleApplyAction == null)
            {
                Debug.Log("敵のタイムスケール変更時イベントは登録されていないよ");
            }
        }

        /// <summary>
        /// 敵のタイムスケール変更時のイベントを登録
        /// </summary>
        /// <param name="eventHandler"></param>
        public void RegisterEnemyTimeScaleApplyAction(Action action)
        {
            enemyTimeScaleApplyAction += action;
        }

        /// <summary>
        /// 敵のタイムスケール変更時のイベントを削除
        /// </summary>
        /// <param name="eventHandler"></param>
        public void UnregisterEnemyTimeScaleApplyAction(Action action)
        {
            enemyTimeScaleApplyAction -= action;
        }

        /// <summary>
        /// 現在設定されているタイムスケールの値を取得
        /// </summary>
        /// <returns>タイムスケールの値</returns>
        public float GetTimeScale()
        {
            return timeScale;
        }

        /// <summary>
        /// 現在設定されている敵のタイムスケールの値を取得
        /// </summary>
        /// <returns>敵のタイムスケールの値</returns>
        public float GetEnemyTimeScale()
        {
            return enemyTimeScale;
        }
    }
}
