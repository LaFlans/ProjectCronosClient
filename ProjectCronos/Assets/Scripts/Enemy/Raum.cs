using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ProjectCronos
{
    /// <summary>
    /// ラウム
    /// </summary>
    class Raum : Enemy
    {
        [SerializeField]
        Transform attackObjParent;

        /// <summary>
        /// 攻撃のインターバル
        /// </summary>
        float attackInterval = 3f;

        /// <summary>
        /// 攻撃できるか
        /// </summary>
        bool isAttack = true;

        protected override async UniTask Load()
        {
            //await AddressableManager.Instance.Load(fireSkeltonPrefabPath);
        }

        protected override void OnEnemyTimeScaleApply()
        {
            base.OnEnemyTimeScaleApply();

            //攻撃オブジェクトの速度を調整
            bool result = TimeManager.Instance.GetEnemyTimeScale() > 0;
            for (int i = 0; i < attackObjParent.childCount; i++)
            {
                attackObjParent.GetChild(i).GetComponent<AttackTrigger>().SetIsAct(result);
            }
        }

        protected override async void Move()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                // 攻撃中は移動しない
                return;
            }

            base.Move();

            if (anim != null)
            {
                anim.SetFloat("Speed", agent.velocity.magnitude);
            }
        }

        protected override async void Attack()
        {
            isAct = false;
            base.Attack();

            if (!isAttack)
            {
                // 攻撃できない状態の場合、何もしない
                return;
            }

            //LookTargetMoment();

            // FIXME: 攻撃処理をここに
            Debug.Log("ラウムの攻撃!");
            if (targetDistance < 2)
            {
                Debug.Log("近距離攻撃");
                await ExecuteShortRangeAttack();
            }
            else if(targetDistance < 5)
            {
                Debug.Log("中距離攻撃");
                await ExecuteMiddleRangeAttack();
            }
            else if(targetDistance < 10)
            {
                await ExecuteMiddleRangeAttack();

                Debug.Log("遠距離攻撃");
                //await ExecuteLongRangeAttack();
            }

            await AttackInterval();

            isAct = true;
        }

        /// <summary>
        /// 近距離攻撃実行
        /// </summary>
        /// <returns></returns>
        async UniTask ExecuteShortRangeAttack()
        {
            if (anim == null || anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                return;
            }
            Debug.Log("近距離開始");

            SetNavmeshUpdatePositionFlase();
            agent.isStopped = true;
            isTracking = false;

            anim.Play("FlyingSpinningKick");

            // 攻撃判定出現タイミングまで待機
            await UniTask.WaitUntil(() => 0.65f <= animStateInfo.normalizedTime);

            colliderInfo.EnableCollider(0);
            Debug.Log("近距離0.65f");

            await UniTask.WaitUntil(() => 0.75f <= animStateInfo.normalizedTime);

            colliderInfo.DisableCollider(0);
            Debug.Log("近距離終了0.75f");

            await UniTask.WaitUntil(() => 0.90f <= animStateInfo.normalizedTime);

            isTracking = true;
            agent.isStopped = false;
            SetNavmeshUpdatePositionTrue();
            Debug.Log("近距離終了");
        }

        /// <summary>
        /// 中距離攻撃実行
        /// </summary>
        /// <returns></returns>
        async UniTask ExecuteMiddleRangeAttack()
        {
            if (anim == null || anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                return;
            }

            Debug.Log("中距離開始");
            SetNavmeshUpdatePositionFlase();
            agent.isStopped = true;
            isTracking = false;

            anim.Play("WingAttack1");

            // 攻撃判定出現タイミングまで待機
            await UniTask.WaitUntil(() => 0.33f <= animStateInfo.normalizedTime);
            colliderInfo.EnableCollider(1);
            await UniTask.WaitUntil(() => 0.5f <= animStateInfo.normalizedTime);
            colliderInfo.DisableCollider(1);
            await UniTask.WaitUntil(() => 0.90f <= animStateInfo.normalizedTime);

            isTracking = true;
            agent.isStopped = false;
            SetNavmeshUpdatePositionTrue();
            Debug.Log("中距離終了"); 
        }

        /// <summary>
        /// 長距離攻撃実行
        /// </summary>
        /// <returns></returns>
        async UniTask ExecuteLongRangeAttack()
        {
            agent.isStopped = true;
            isTracking = false;

            await UniTask.Delay(TimeSpan.FromSeconds(2));
            await UniTask.Delay(TimeSpan.FromSeconds(2));

            isTracking = true;
            agent.isStopped = false;
        }

        /// <summary>
        /// ナビメッシュの移動を更新するかの設定
        /// </summary>
        /// <param name="result"></param>
        void SetNavmeshPosition(bool result)
        {
            agent.updatePosition = result;
        }

        /// <summary>
        /// ナビメッシュの位置を体の位置に設定
        /// </summary>
        void SetNavmeshPositionToBody()
        {
            agent.transform.position = bodyPos.position;
        }
        
        /// <summary>
        /// 距離を詰める
        /// </summary>
        public void CloseDistance()
        {
            var dist = target.transform.position - agent.transform.position;
            this.transform.position = dist * 0.8f;
        }

        /// <summary>
        /// AIの思考インターバル
        /// </summary>
        async UniTask AttackInterval()
        {
            isAttack = false;

            await UniTask.Delay(TimeSpan.FromSeconds(attackInterval));

            isAttack = true;
        }
    }
}
