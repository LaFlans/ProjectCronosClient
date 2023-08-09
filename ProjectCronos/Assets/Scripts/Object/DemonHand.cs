using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class DemonHand : MonoBehaviour
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

        float tempSpeed;

        /// <summary>
        /// 当たり判定
        /// </summary>
        [SerializeField]
        AttackTrigger attackTrigger;

        /// <summary>
        /// オブジェクト(プレイヤーの攻撃等)のタイムスケールの値更新時イベント
        /// </summary>
        void OnObjectTimeScaleApply()
        {
            if (anim != null)
            {
                InitAct();
            }
        }

        void InitAct()
        {
            var result = TimeManager.Instance.GetObjectTimeScale() > 0;
            if (result)
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

        void Start()
        {
            InitSummonMat();
            anim = target.GetComponent<Animator>();
            tempSpeed = anim.speed;
            target.GetComponent<DefaultAnimationEvent>().Init(
                finishAction: AnimationFinishEvent,
                extension1Action: EnableCollider,
                extension2Action: DisableCollider);

            // オブジェクトのタイムスケールの値更新時イベント設定
            TimeManager.Instance.RegisterObjectTimeScaleApplyAction(OnObjectTimeScaleApply);
            InitAct();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="attack">攻撃力</param>
        public void Initialize(int attack)
        {
            attackTrigger.Init(EnumCollection.Attack.ATTACK_TYPE.PLAYER, attack);
        }

        void InitSummonMat()
        {
            mat = target.GetComponent<Renderer>().material;
            mat.SetVector("_Up", this.transform.up.normalized);
            mat.SetVector("_CenterPos", this.transform.position);
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
    }
}
