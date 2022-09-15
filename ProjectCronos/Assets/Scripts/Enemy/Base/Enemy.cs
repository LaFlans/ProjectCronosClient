using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

namespace ProjectCronos
{
    [RequireComponent(typeof(EnemyStatus))]
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

        /// <summary>
        /// AI思考インターバル
        /// </summary>
        [SerializeField]
        float aiIntervalTime = 1;

        bool isAiStateRunning = false;
        bool isWait = false;
        bool isInit;

        /// <summary>
        /// ターゲットとの距離
        /// </summary>
        float targetDistance;

        protected Transform target;
        NavMeshAgent agent;
        protected Vector3 targetDir;

        //　デバック用
        [SerializeField]
        TextMeshPro stateDebugText;

        /// <summary>
        /// 初期化
        /// </summary>
        public override async void Initialize()
        {
            isInit = false;
            
            target = GameObject.Find("Player").GetComponent<Player>().GetCenterPos();

            base.Initialize();
            //Debug.Log("敵初期化");

            agent = GetComponent<NavMeshAgent>();

            state = ENEMY_AI_STATE.IDLE;

            await Load();

            isInit = true;
        }

        /// <summary>
        /// 読み込む必要があるものを読み込む
        /// </summary>
        /// <returns></returns>
        protected virtual async UniTask Load()
        {
        }

        void Update()
        {
            //　デバック用
            stateDebugText.text = state.ToString();

            if (isInit)
            {
                ApplyAi();
            }
        }

        void ApplyAi()
        {
            SetAi();

            switch(state)
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
            if (target != null)
            {
                //　ターゲットまでの方向と距離を計算
                targetDir = target.position - this.transform.position;
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
            else if (targetDistance < 6)
            {
                // 攻撃範囲内の場合、攻撃
                nextState = ENEMY_AI_STATE.ATTACK;
            }
            else if (6 <= targetDistance && targetDistance < 12)
            {
                // 索敵範囲内で攻撃範囲外の場合、移動
                nextState = ENEMY_AI_STATE.MOVE;
            }
            else if (12 <= targetDistance)
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

            await UniTask.Delay(TimeSpan.FromSeconds(aiIntervalTime));

            isAiStateRunning = false;
        }

        protected virtual void Wait()
        {

        }

        protected virtual void Move()
        {
            if (target != null)
            {
                agent.SetDestination(target.position);
            }
        }

        protected virtual void Attack()
        {
            LookTarget();
        }

        protected virtual void Idle()
        {
            agent.SetDestination(this.transform.position);
        }

        void LookTarget()
        {
            // その方向に向けて旋回する(120度/秒)
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);
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
        public override void Damage(int value)
        {
            base.Damage(value);
            //Debug.Log("敵被弾");
        }

        /// <summary>
        /// 死亡時処理
        /// </summary>
        public override void Death()
        {
            base.Death();

            //Debug.Log("敵死亡");
            // 自身を破壊
            Destroy(this.gameObject);
        }
    }
}
