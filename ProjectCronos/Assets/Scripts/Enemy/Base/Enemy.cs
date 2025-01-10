using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using TMPro;

namespace ProjectCronos
{
    [RequireComponent(typeof(EnemyStatus), typeof(EnemyInfo))]
    public class Enemy : Character, IEnemy
    {
        public enum ENEMY_AI_STATE
        {
            WAIT,
            MOVE,
            ATTACK,
            IDLE,
        }

        public ENEMY_AI_STATE state;
        ENEMY_AI_STATE nextState;

        bool isAiStateRunning = false;
        bool isWait = false;
        bool isInit;

        /// <summary>
        /// ターゲットとの距離
        /// </summary>
        protected float targetDistance;

        /// <summary>
        /// プレイヤー参照
        /// </summary>
        protected GameObject player;

        /// <summary>
        /// プレイヤーの位置
        /// </summary>
        protected Transform target;

        /// <summary>
        /// アニメーター
        /// </summary>
        [SerializeField]
        protected Animator anim;

        /// <summary>
        /// アニメ―ターのステート情報
        /// </summary>
        protected AnimatorStateInfo animStateInfo;

        [SerializeField]
        protected NavMeshAgent agent;
        protected Vector3 targetDir;

        //　デバック用
        [SerializeField]
        TextMeshPro stateDebugText;

        /// <summary>
        /// 行動することができるか
        /// </summary>
        protected bool isAct;

        /// <summary>
        /// 追跡するか
        /// </summary>
        protected bool isTracking;

        [SerializeField]
        protected Transform bodyTransform;

        Action EnemyTimeScaleApplyEvent;

        /// <summary>
        /// コライダ情報
        /// </summary>
        protected EnemyColliderInfo colliderInfo;

        /// <summary>
        /// 敵のステータス情報
        /// </summary>
        EnemyStatus enemyStatus;

        /// <summary>
        /// 体の位置
        /// </summary>
        [SerializeField]
        protected Transform bodyPos;

        public float attackMoveSpeed = 20.0f;

        /// <summary>
        /// 体の親子関係コンストレイント
        /// </summary>
        [SerializeField]
        public ParentConstraint bodyParentConstraint;

        /// <summary>
        /// ナビメッシュの親子関係コンストレイント
        /// </summary>
        [SerializeField]
        public ParentConstraint navmeshParentConstraint;

        [SerializeField]
        Transform allBody;

        /// <summary>
        /// 死亡時アクション
        /// </summary>
        Action deathAction;

        IEnumerator hitStopCoroutine;
        bool isHitStop = false;
        float animSpeed = 1;

        protected virtual void OnEnemyTimeScaleApply()
        {
            var enemyTimeScale = TimeManager.Instance.GetEnemyTimeScale();
            Debug.Log("敵のタイムスケール変更時イベントを行うよ");
            agent.speed = status.moveSpeed * enemyTimeScale;
            isAct = enemyTimeScale > 0;
            isTracking = false;

            if (anim != null)
            {
                anim.speed = enemyTimeScale;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override async UniTask<bool> Initialize()
        {
            isInit = false;

            status = this.GetComponent<EnemyStatus>();
            status.Initialize();
            enemyStatus = this.GetComponent<EnemyStatus>();
            enemyStatus.Initialize();

            player = GameObject.FindWithTag("Player");
            target = player.GetComponent<Player>().GetCenterPos();

            colliderInfo = this.GetComponent<EnemyColliderInfo>();
            colliderInfo?.Init(status.attack);

            // エージェントの移動速度初期化
            agent.speed = status.moveSpeed;

            // コンストレイント設定
            bodyParentConstraint.enabled = true;
            navmeshParentConstraint.enabled = false;

            hitStopCoroutine = HitStop();
            animSpeed = anim.speed;

            await base.Initialize();

            state = ENEMY_AI_STATE.IDLE;

            EnemyTimeScaleApplyEvent = OnEnemyTimeScaleApply;
            TimeManager.Instance.RegisterEnemyTimeScaleApplyAction(EnemyTimeScaleApplyEvent);

            await Load();

            isAct = true;

            Debug.Log("敵の初期化完了！");
            isInit = true;

            return true;
        }

        /// <summary>
        /// 読み込む必要があるものを読み込む
        /// </summary>
        /// <returns></returns>
        protected virtual async UniTask Load()
        {
        }

        async void Update()
        {
            if (isTracking)
            {
                TrackingTarget();
            }

            //　デバック用
            stateDebugText.text = state.ToString();

            if (isInit && isAct)
            {
                ApplyAi();
            }

            // アニメーターステート情報更新
            if (isInit && anim)
            {
                animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
                anim.SetFloat("Speed", agent.velocity.magnitude);
            }
        }

        async void ApplyAi()
        {
            SetAi();

            if (isTracking)
            {
                TrackingTarget();
            }

            switch (state)
            {
                case ENEMY_AI_STATE.WAIT:
                    Wait();
                    break;
                case ENEMY_AI_STATE.MOVE:
                    Move();
                    break;
                case ENEMY_AI_STATE.ATTACK:
                    Attack();
                    break;
                case ENEMY_AI_STATE.IDLE:
                    Idle();
                    break;
            }
        }

        async void SetAi()
        {
            if (isAiStateRunning)
            {
                return;
            }

            // AI初期化
            InitAi();
            AiMainRoutine();

            state = nextState;

            await AiInterval();
        }

        /// <summary>
        /// AI初期化
        /// </summary>
        void InitAi()
        {
            CalcTargetDistAndDirection();
        }

        void CalcTargetDistAndDirection()
        {
            if (target != null)
            {
                //　ターゲットまでの方向と距離を計算
                targetDir = target.position - bodyTransform.position;
                targetDistance = targetDir.magnitude;
            }
        }

        /// <summary>
        /// AIのメインルーチン
        /// </summary>
        void AiMainRoutine()
        {
            if (isWait)
            {
                nextState = ENEMY_AI_STATE.WAIT;
                isWait = false;
                return;
            }

            if (target == null)
            {
                // そもそもターゲットがいなかったら待機
                nextState = ENEMY_AI_STATE.IDLE;
            }
            else if (targetDistance < enemyStatus.attackDist)
            {
                // 攻撃範囲内の場合、攻撃
                nextState = ENEMY_AI_STATE.ATTACK;
            }
            else if (enemyStatus.attackDist <= targetDistance && targetDistance < enemyStatus.searchDist)
            {
                // 索敵範囲内で攻撃範囲外の場合、移動
                nextState = ENEMY_AI_STATE.MOVE;
            }
            else if (enemyStatus.searchDist <= targetDistance)
            {
                //　一定以上離れたら待機
                nextState = ENEMY_AI_STATE.IDLE;
            }
        }

        /// <summary>
        /// AIの思考インターバル
        /// </summary>
        async UniTask AiInterval()
        {
            isAiStateRunning = true;

            await UniTask.Delay(TimeSpan.FromSeconds(enemyStatus.aiThinkingIntervalTime));

            isAiStateRunning = false;
        }

        protected virtual void Wait()
        {

        }

        protected virtual void Move()
        {
            if (agent.isStopped)
            {
                agent.isStopped = false;
            }

            TrackingTarget();
        }

        protected virtual void Attack()
        {
            LookTarget();
        }

        protected virtual void Idle()
        {
            agent.SetDestination(bodyTransform.position);
        }

        void TrackingTarget()
        {
            if (target != null)
            {
                //Debug.Log("追跡！");
                //agent.destination = target.position;

                agent.SetDestination(target.position);
            }
        }

        /// <summary>
        /// 追跡オン
        /// </summary>
        protected void TrackingOn()
        {
            isTracking = true;

            // 攻撃中の追跡速度を上げるテスト
            agent.speed = attackMoveSpeed;

            // ターゲットの位置へ移動
            TrackingTarget();

            Debug.Log("追跡オン！");
        }

        /// <summary>
        /// 追跡オフ
        /// </summary>
        protected void TrackingOff()
        {
            isTracking = false;

            // 攻撃中の追跡速度を元に戻す
            agent.speed = status.moveSpeed;

            // 移動を止める
            agent.isStopped = true;

            Debug.Log("追跡オフ！");
        }

        /// <summary>
        /// 回転速度に応じてターゲットのほうへ向く
        /// </summary>
        protected void LookTarget()
        {
            // その方向に向けて旋回する(120度/秒)
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            bodyTransform.rotation = Quaternion.RotateTowards(bodyTransform.rotation, targetRotation, 120f * Time.deltaTime);
        }

        /// <summary>
        /// 一瞬でターゲットの方へ向く
        /// </summary>
        public void LookTargetMoment()
        {
            Debug.Log("一瞬でターゲットの方へ向く");

            CalcTargetDistAndDirection();

            // その方向に向けて旋回する
            bodyTransform.rotation = Quaternion.LookRotation(targetDir);
        }

        public void SetDeathAction(Action action)
        {
            this.deathAction = action;
        }

        /// <summary>
        /// 出現時処理
        /// </summary>
        public override void Appear()
        {
            //Debug.Log("敵出現");
        }

        /// <summary>
        /// 被弾時処理
        /// </summary>
        /// <param name="value">ダメージの値</param>
        /// <param name="isRight">被弾箇所が体の右側かどうか(ここは必須)</param>
        /// <returns>この被弾により死亡した場合、Trueで返す</returns>
        public override bool Damage(int value, bool isRight = false)
        {
            HitAnimation(value, isRight);
            return base.Damage(value, isRight);
        }

        void HitAnimation(int value, bool isRight = false)
        {
            if (isRight)
            {
                if (value < 10)
                {
                    anim.SetTrigger("Hit_S_R");
                }
                else
                {
                    anim.SetTrigger("Hit_M_R");
                }
            }
            else
            {
                if (value < 10)
                {
                    anim.SetTrigger("Hit_S_L");
                }
                else
                {
                    anim.SetTrigger("Hit_M_L");
                }
            }

            // 既に進んでいるヒットストップコルーチンが進んでいたら一旦止める
            if (isHitStop)
            {
                StopCoroutine(hitStopCoroutine);
            }

            StartCoroutine(HitStop());
        }

        IEnumerator HitStop()
        {
            isHitStop = true;

            yield return new WaitForSeconds(0.2f);

            anim.speed = 0;

            yield return new WaitForSeconds(0.5f);

            anim.speed = animSpeed;

            isHitStop = false;
        }

        /// <summary>
        /// 死亡時処理
        /// </summary>
        public override void Death()
        {
            Debug.Log("敵死亡");
            TimeManager.Instance.UnregisterEnemyTimeScaleApplyAction(EnemyTimeScaleApplyEvent);

            // ドロップ追加
            player.GetComponent<PlayerStatus>().AddCoin(enemyStatus.dropCoin);

            // 死亡時アクション(あれば)
            deathAction?.Invoke();

            base.Death();
        }

        void OnDestroy()
        {
            TimeManager.Instance.UnregisterEnemyTimeScaleApplyAction(EnemyTimeScaleApplyEvent);
        }

        void SwitchParentConstraint()
        {
            bodyParentConstraint.enabled = !bodyParentConstraint.enabled;
            navmeshParentConstraint.enabled = !navmeshParentConstraint.enabled;
        }

        protected void SetNavmeshUpdatePositionFlase()
        {
            //Debug.Log("Agentの更新を止めました");
            agent.updatePosition = false;
            SwitchParentConstraint();
        }

        protected void SetNavmeshUpdatePositionTrue()
        {
            if(agent != null)
            {
                //Debug.Log("Agentの更新を再開しました");
                agent.updatePosition = true;

                SwitchParentConstraint();
                agent.Warp(allBody.position);
            }
        }

        /// <summary>
        /// ナビメッシュエージェントの移動制御
        /// </summary>
        /// <param name="result">移動するかどうか</param>
        protected void SetIsStoppedAgent(bool result)
        {
            if (agent != null)
            {
                agent.isStopped = result;
            }
        }
    }
}
