using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectCronos
{
    public class DemonHand : MagicCircle
    {
        [SerializeField]
        GameObject target;
        Material mat;
        Animator anim;

        [SerializeField] GameObject frontCrest;
        [SerializeField] GameObject backCrest;

        /// <summary>
        /// 行動することができるか
        /// </summary>
        bool isAct;

        /// <summary>
        /// トラップが発動したかどうか
        /// </summary>
        bool isTriggerTrap;

        float tempSpeed;

        /// <summary>
        /// 攻撃の当たり判定
        /// </summary>
        [SerializeField]
        AttackTrigger attackTrigger;

        /// <summary>
        /// 魔法陣当たり判定
        /// 魔法陣どうしの合成用
        /// </summary>
        [SerializeField]
        MagicCircleCollider circleCol;

        /// <summary>
        /// 魔法陣トラップ用当たり判定
        /// 敵が範囲内に来たことを判定する用
        /// </summary>
        [SerializeField]
        MagicCircleTrapCollider circleTrapCol;

        [SerializeField]
        TextMeshPro debugLevelText;

        void Start()
        {
            // デバック処理
            Debug();

            InitSummonMat();
            anim = target.GetComponent<Animator>();

            target.GetComponent<DefaultAnimationEvent>().Init(
                finishAction: AnimationFinishEvent,
                extension1Action: EnableCollider,
                extension2Action: DisableCollider);

            // オブジェクトのタイムスケールの値更新時イベント設定
            TimeManager.Instance.RegisterObjectTimeScaleApplyAction(OnObjectTimeScaleApply);

            InitAct();
        }

        void Debug()
        {
            if (debugLevelText != null)
            {
                debugLevelText.text = level.ToString();
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="attack">攻撃力</param>
        /// <param name="level">魔法陣レベル</param>
        public void Initialize(int attack, int level, bool isTrap = false)
        {
            this.level = level;
            attackTrigger.Init(EnumCollection.Attack.ATTACK_TYPE.PLAYER, attack);

            if (circleTrapCol != null)
            {
                // 攻撃の種類がトラップの場合は初期アニメーションスピードは0
                switch (summonAttackType)
                {
                    case EnumCollection.Attack.SUMMON_ATTACK_TYPE.DIRECT:
                        isTriggerTrap = true;
                        circleTrapCol.gameObject.SetActive(false);
                        tempSpeed = 1;
                        break;
                    case EnumCollection.Attack.SUMMON_ATTACK_TYPE.TRAP:
                        circleCol.gameObject.SetActive(isTrap);
                        circleTrapCol.gameObject.SetActive(false);
                        tempSpeed = 0;
                        break;
                    default:
                        isTriggerTrap = true;
                        circleTrapCol.gameObject.SetActive(false);
                        tempSpeed = 1;
                        break;
                }
            }
            else
            {
                tempSpeed = 1;
            }
        }

        void InitSummonMat()
        {
            mat = target.GetComponent<Renderer>().material;
            mat.SetVector("_Up", this.transform.up.normalized);
            mat.SetVector("_CenterPos", this.transform.position);
        }

        /// <summary>
        /// オブジェクト(プレイヤーの攻撃等)のタイムスケールの値更新時イベント
        /// </summary>
        void OnObjectTimeScaleApply()
        {
            InitAct();
        }

        void InitAct()
        {
            if (anim == null)
            {
                return;
            }

            var result = TimeManager.Instance.GetObjectTimeScale() > 0;
            if (result)
            {
                anim.speed = tempSpeed;
                isAct = true;
            }
            else
            {
                tempSpeed = anim.speed;
                //tempSpeed = isTriggerTrap ? anim.speed : 0;
                anim.speed = 0;
                isAct = false;
            }
        }

        void EnableCollider()
        {
            // 魔法陣の当たり判定を非アクティブに
            circleCol.SetColliderEnable(false);

            attackTrigger.EnableCollider();
        }

        void DisableCollider()
        {
            attackTrigger.DisableCollider();
        }

        /// <summary>
        /// アニメーション終了時イベント
        /// </summary>
        void AnimationFinishEvent()
        {
            // 自身を破壊
            // TODO: ワープホールを縮小して破壊
            Destroy(this.gameObject);
        }

        void OnDestroy()
        {
            // オブジェクトのタイムスケールの値更新時イベント削除
            TimeManager.Instance.UnregisterObjectTimeScaleApplyAction(OnObjectTimeScaleApply);
        }

        /// <summary>
        /// トラップ設置
        /// </summary>
        public override void PutTrap()
        {
            circleCol.gameObject.SetActive(true);
            circleTrapCol.gameObject.SetActive(true);
        }

        /// <summary>
        /// トラップ発動
        /// </summary>
        public override void TriggerTrap()
        {
            if (anim != null)
            {
                isTriggerTrap = true;
                circleTrapCol.gameObject.SetActive(false);
                tempSpeed = 1;
                InitAct();
            }
        }
    }
}
