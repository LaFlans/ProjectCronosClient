using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace ProjectCronos
{
    /// <summary>
    /// プレイヤーのスキル関連
    /// </summary>
    public class PlayerSkill : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーのスキル第一情報
        /// </summary>
        EnumCollection.Attack.SKILL_TYPE firstSkillType;

        /// <summary>
        /// プレイヤーのステータス情報
        /// </summary>
        PlayerStatus status;

        /// <summary>
        /// 時間停止中かどうか
        /// </summary>
        bool isTimeStopWorld;

        float timeStopTimer;

        /// <summary>
        /// ボリューム設定
        /// </summary>
        [SerializeField]
        Volume volume;

        /// <summary>
        /// 時間停止中のポストエフェクト(今は仮)
        /// </summary>
        GrayScale grayScale;

        void Update()
        {
            if (isTimeStopWorld)
            {
                timeStopTimer += Time.deltaTime;
                if (timeStopTimer > 1)
                {
                    timeStopTimer = 0;
                    if (status.DamageMp(200))
                    {
                        //　MPが足りなくなった場合、スキルを停止する
                        TimeStop();
                    }
                }
            }
        }

        public void Initialize()
        {
            firstSkillType = EnumCollection.Attack.SKILL_TYPE.TIME_STOP;

            // 時間停止状態設定
            isTimeStopWorld = false;

            // ステータス情報設定
            status = this.GetComponent<PlayerStatus>();

            volume.profile.TryGet(out grayScale);
            if (grayScale == null)
            {
                Debug.Log("時間停止中のポストエフェクトがせっていされていないよ！");
            }
        }

        public void OnActiveFirstSkill(InputAction.CallbackContext context)
        {
            switch (firstSkillType)
            {
                case EnumCollection.Attack.SKILL_TYPE.TIME_STOP:
                    TimeStop();
                    break;
            }
        }

        /// <summary>
        /// 時間停止スキル
        /// </summary>
        void TimeStop()
        {
            // 時間停止テスト
            //Debug.Log("時間停止テスト");
            isTimeStopWorld = !isTimeStopWorld;
            TimeManager.Instance.ApplyEnemyTimeScale(isTimeStopWorld ? 0.0f : 1.0f);
            TimeManager.Instance.ApplyObjectTimeScale(isTimeStopWorld ? 0.0f : 1.0f);

            //　時間停止中のポストエフェクト設定
            grayScale.intensity.value = isTimeStopWorld ? 1.0f : 0.0f;
        }
    }
}
