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
        const float DEFAULT_OBJECT_TIME_SCALE = 1.0f;

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

        /// <summary>
        /// オブジェクト(プレイヤーの攻撃など)のタイムスケール
        /// </summary>
        float objectTimeScale;

        /// <summary>
        /// オブジェクト(プレイヤーの攻撃など)のタイムスケール変化した時のイベント
        /// </summary>
        Action objectTimeScaleApplyAction;

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
            objectTimeScale = DEFAULT_OBJECT_TIME_SCALE;

            ApplyTimeScale();
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
        /// <param name="action"></param>
        public void RegisterEnemyTimeScaleApplyAction(Action action)
        {
            Debug.Log("敵のタイムスケール変更時イベントを登録するよ！");
            enemyTimeScaleApplyAction += action;
        }

        /// <summary>
        /// 敵のタイムスケール変更時のイベントを削除
        /// </summary>
        /// <param name="action"></param>
        public void UnregisterEnemyTimeScaleApplyAction(Action action)
        {
            Debug.Log("敵のタイムスケール変更時イベントを削除するよ！");
            enemyTimeScaleApplyAction -= action;
        }

        /// <summary>
        /// オブジェクト(プレイヤーの攻撃など)のタイムスケールを適用
        /// </summary>
        /// <param name="value">値</param>
        public void ApplyObjectTimeScale(float value)
        {
            objectTimeScale = value;

            // オブジェクトのタイムスケール変更時処理を行う
            objectTimeScaleApplyAction?.Invoke();

            if (objectTimeScaleApplyAction == null)
            {
                Debug.Log("オブジェクトのタイムスケール変更時イベントは登録されていないよ");
            }
        }

        /// <summary>
        /// オブジェクト(プレイヤーの攻撃など)のタイムスケール変更時のイベントを登録
        /// </summary>
        /// <param name="action"></param>
        public void RegisterObjectTimeScaleApplyAction(Action action)
        {
            objectTimeScaleApplyAction += action;
        }

        /// <summary>
        /// オブジェクト(プレイヤーの攻撃など)のタイムスケール変更時のイベントを削除
        /// </summary>
        /// <param name="action"></param>
        public void UnregisterObjectTimeScaleApplyAction(Action action)
        {
            objectTimeScaleApplyAction -= action;
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

        /// <summary>
        /// 現在設定されているオブジェクト(プレイヤーの攻撃等)のタイムスケールの値を取得
        /// </summary>
        /// <returns>オブジェクト(プレイヤーの攻撃等)のタイムスケールの値</returns>
        public float GetObjectTimeScale()
        {
            return objectTimeScale;
        }
    }
}
