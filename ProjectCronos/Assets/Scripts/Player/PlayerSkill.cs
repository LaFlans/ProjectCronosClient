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
        /// 時間停止中かどうか
        /// </summary>
        bool isTimeStopWorld;

        [SerializeField]
        Volume volume;

        /// <summary>
        /// 時間停止中のポストエフェクト(今は仮)
        /// </summary>
        GrayScale grayScale;

        public void Initialize()
        {
            firstSkillType = EnumCollection.Attack.SKILL_TYPE.TIME_STOP;

            // 時間停止状態設定
            isTimeStopWorld = false;

            volume.profile.TryGet(out grayScale);
            if(grayScale == null)
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
            UnityEngine.Debug.Log("時間停止テスト");
            isTimeStopWorld = !isTimeStopWorld;
            TimeManager.Instance.ApplyEnemyTimeScale(isTimeStopWorld ? 0.0f : 1.0f);
            TimeManager.Instance.ApplyObjectTimeScale(isTimeStopWorld ? 0.0f : 1.0f);

            //　時間停止中のポストエフェクト設定
            grayScale.intensity.value = isTimeStopWorld ? 1.0f : 0.0f;
        }
    }
}
