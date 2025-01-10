using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

namespace ProjectCronos
{
    public class DemonHand : MagicCircle
    {
        const string magicCircleMatBasePath = "Assets/ProjectCronosAssets/Materials/MagicCircle/MagicCircleLevel";

        [SerializeField]
        GameObject target;
        Material mat;
        Animator anim;

        [SerializeField]
        GameObject frontCrest;

        [SerializeField]
        GameObject backCrest;

        /// <summary>
        /// 行動することができるか
        /// </summary>
        bool isAct;

        float tempSpeed;

        /// <summary>
        /// 攻撃の当たり判定
        /// </summary>
        [SerializeField]
        AttackTrigger attackTrigger;

        /// <summary>
        /// 魔法陣設置前用当たり判定
        /// 設置できるかどうか確認する用
        /// </summary>
        [SerializeField]
        MagicCircleControlledCollider magicCircleControlledCol;

        /// <summary>
        /// 魔法陣当たり判定
        /// 魔法陣どうしの合成用
        /// </summary>
        [SerializeField]
        MagicCirclePlacedCollider magicCirclePlacedCol;

        /// <summary>
        /// 魔法陣トラップ用当たり判定
        /// 敵が範囲内に来たことを判定する用
        /// </summary>
        [SerializeField]
        MagicCircleTriggerCollider magicCircleTriggerCol;

        /// <summary>
        /// 魔法陣設置時エフェクトオブジェクト
        /// </summary>
        [SerializeField]
        GameObject placeEffectObj;

        [SerializeField]
        TextMeshPro debugLevelText;


        EnumCollection.Attack.MAGIC_CIRCLE_STATUS magicCircleStatus;

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
            tempSpeed = 1;

            // デバック処理
            Debug();

            InitSummonMat();
            anim = target.GetComponent<Animator>();
            placeEffectObj.SetActive(false);

            target.GetComponent<DefaultAnimationEvent>().Init(
                finishAction: AnimationFinishEvent,
                extension1Action: EnableCollider,
                extension2Action: DisableCollider);

            // オブジェクトのタイムスケールの値更新時イベント設定
            TimeManager.Instance.RegisterObjectTimeScaleApplyAction(OnObjectTimeScaleApply);

            magicCircleStatus = EnumCollection.Attack.MAGIC_CIRCLE_STATUS.BEFORE_PUT;

            InitAct();
            attackTrigger.Init(EnumCollection.Attack.ATTACK_TYPE.PLAYER, attack);
            attackTrigger.DisableCollider();

            if (magicCircleTriggerCol != null)
            {
                // 攻撃の種類がトラップの場合は初期アニメーションスピードは0
                switch (summonAttackType)
                {
                    case EnumCollection.Attack.SUMMON_ATTACK_TYPE.DIRECT:
                        magicCircleStatus = EnumCollection.Attack.MAGIC_CIRCLE_STATUS.INVOKE;
                        magicCircleTriggerCol.gameObject.SetActive(false);
                        break;
                    case EnumCollection.Attack.SUMMON_ATTACK_TYPE.TRAP:
                        magicCircleStatus = EnumCollection.Attack.MAGIC_CIRCLE_STATUS.BEFORE_PUT;
                        magicCirclePlacedCol.gameObject.SetActive(isTrap);
                        magicCircleTriggerCol.gameObject.SetActive(false);
                        break;
                    default:
                        magicCircleStatus = EnumCollection.Attack.MAGIC_CIRCLE_STATUS.INVOKE;
                        magicCircleTriggerCol.gameObject.SetActive(false);
                        AttackAction();
                        break;
                }
            }
            else
            {
                AttackAction();
            }
        }

        void InitSummonMat()
        {
            // 召喚物のマテリアルカラー初期化
            mat = target.GetComponent<Renderer>().material;
            mat.SetVector("_Up", this.transform.up.normalized);
            mat.SetVector("_CenterPos", this.transform.position);

            // 召喚陣のマテリアルカラー初期化
            Material crestMat = AddressableManager.Instance.GetLoadedMaterial(magicCircleMatBasePath + level.ToString() + ".mat");
            if (crestMat != null)
            {
                frontCrest.GetComponent<Renderer>().material = crestMat;

                Material backMat = new Material(crestMat);
                backMat.SetFloat("_TransparentSortPriority", 1);
                HDMaterial.ValidateMaterial(backMat);
                backCrest.GetComponent<Renderer>().material = backMat;
            }
        }

        /// <summary>
        /// オブジェクト(プレイヤーの攻撃等)のタイムスケールの値更新時イベント
        /// </summary>
        void OnObjectTimeScaleApply()
        {
            InitAct();
            TrapStatusUpdate();
        }

        void InitAct()
        {
            if (anim == null)
            {
                return;
            }

            if (!TimeManager.Instance.IsStopObjectTimeScale())
            {
                anim.speed = tempSpeed;
                isAct = true;
            }
            else
            {
                tempSpeed = anim.speed;
                anim.speed = 0;
                isAct = false;
            }
        }

        void EnableCollider()
        {
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

        void TrapStatusUpdate()
        {
            switch (magicCircleStatus)
            {
                case EnumCollection.Attack.MAGIC_CIRCLE_STATUS.INVOKE:
                    magicCirclePlacedCol.gameObject.SetActive(false);
                    magicCircleTriggerCol.gameObject.SetActive(false);
                    break;
                case EnumCollection.Attack.MAGIC_CIRCLE_STATUS.BEFORE_PUT:
                    break;
                case EnumCollection.Attack.MAGIC_CIRCLE_STATUS.TRAP:
                    var isStopobjTime = TimeManager.Instance.IsStopObjectTimeScale();
                    magicCircleControlledCol.gameObject.SetActive(false);
                    magicCirclePlacedCol.gameObject.SetActive(true);
                    magicCircleTriggerCol.gameObject.SetActive(!isStopobjTime);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// トラップ設置
        /// </summary>
        public override void PutTrap()
        {
            magicCircleStatus = EnumCollection.Attack.MAGIC_CIRCLE_STATUS.TRAP;
            TrapStatusUpdate();

            // 設置エフェクトを再生して削除
            placeEffectObj.SetActive(true);
            Destroy(placeEffectObj, 1.0f);
        }

        /// <summary>
        /// トラップ発動
        /// </summary>
        public override void TriggerTrap()
        {
            if (anim != null)
            {
                magicCircleStatus = EnumCollection.Attack.MAGIC_CIRCLE_STATUS.INVOKE;
                TrapStatusUpdate();
                AttackAction();
            }
        }

        /// <summary>
        /// トラップを設置できるか
        /// </summary>
        public override bool IsPutTrap()
        {
            return magicCircleControlledCol.IsPutTrap();
        }

        void AttackAction()
        {
            // 魔法陣の当たり判定を非アクティブに
            magicCirclePlacedCol.SetColliderEnable(false);

            anim.SetTrigger("Action");
        }
    }
}
