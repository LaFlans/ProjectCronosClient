using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

namespace ProjectCronos
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(PlayerStatus))]
    public class Player : Character
    {
        /// <summary>
        /// 移動速度
        /// </summary>
        [SerializeField]
        int moveSpeed = 10;

        /// <summary>
        /// 回転速度
        /// </summary>
        [SerializeField]
        int rotateSpeed = 10;

        /// <summary>
        /// 索敵範囲
        /// </summary>
        [SerializeField]
        int searchRange = 10;

        /// <summary>
        /// リジッドボディ
        /// </summary>
        Rigidbody rigid;

        /// <summary>
        /// アニメーター
        /// </summary>
        Animator anim;

        /// <summary>
        /// 過去の位置
        /// </summary>
        Vector3 latestPos;

        /// <summary>
        /// 入力方向
        /// </summary>
        Vector3 inputVec;

        /// <summary>
        /// 本
        /// </summary>
        [SerializeField]
        Book book;

        /// <summary>
        /// 目標となるオブジェクト
        /// </summary>
        [SerializeField]
        TargetObject targetObj;

        /// <summary>
        /// 弾が出る頻度
        /// </summary>
        [SerializeField]
        float bulletFreq = 0.5f;
        float bulletFreqTime = 0.0f;

        /// <summary>
        /// プレイヤーの頭の位置を示すオブジェクト
        /// </summary>
        [SerializeField]
        GameObject head;

        /// <summary>
        /// ジャンプしたか
        /// </summary>
        bool isJump = false;

        /// <summary>
        /// 地面に着地している状態か
        /// </summary>
        bool isGround = false;

        /// <summary>
        /// ジャンプ力
        /// </summary>
        [SerializeField]
        float jumpPower = 10.0f;

        /// <summary>
        /// ジャンプ状態
        /// </summary>
        enum JumpState
        {
            eIDOL,      // ジャンプしていない状態
            eSTART,     // ジャンプ開始
            eJUMP,      // ジャンプ中
            eLANDING,   // 着地
        };

        JumpState jumpState = JumpState.eIDOL;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            rigid = this.GetComponent<Rigidbody>();
            anim = this.GetComponent<Animator>();
        }

        /// <summary>
        /// 出現時処理
        /// </summary>
        public override void Appear()
        {
            // プレイヤーは一旦何もしない
        }

        /// <summary>
        /// 被弾時
        /// </summary>
        public override void Damage(int value)
        {
            base.Damage(value);
        }

        /// <summary>
        /// 死亡時
        /// </summary>
        public override void Death()
        {
            base.Death();
        }

        /// <summary>
        /// Update
        /// </summary>
        void Update()
        {
            if (jumpState == JumpState.eJUMP && isGround)
            {
                jumpState = JumpState.eIDOL;
                rigid.velocity = Vector3.zero;
            }

            JumpStart();
        }

        /// <summary>
        /// FixedUpdate
        /// </summary>
        void FixedUpdate()
        {
            if (isJump)
            {
                isJump = false;
                jumpState = JumpState.eJUMP;
                rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

            Move();
        }

        /// <summary>
        /// プレイヤーの着地判定時のイベント
        /// </summary>
        public void OnLanding()
        {
            isGround = true;
            anim.SetBool("IsGround", true);
        }

        /// <summary>
        /// プレイヤーの離陸判定時のイベント
        /// </summary>
        public void OnTakeoff()
        {
            isGround = false;
            anim.SetBool("IsGround", false);
        }

        /// <summary>
        /// ジャンプ開始処理
        /// </summary>
        void JumpStart()
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame && 
                isGround && 
                jumpState == JumpState.eIDOL)
            {
                jumpState = JumpState.eSTART;
                anim.SetTrigger("Jump");
            }
        }

        /// <summary>
        /// ジャンプ処理
        /// </summary>
        public void Jump()
        {
            if (!isJump)
            {
                // 地面に接地している時はジャンプ、宙に浮いている場合はジャンプ処理を行わない
                if (isGround)
                {
                    isJump = true;
                    OnTakeoff();
                }
                else
                {
                    jumpState = JumpState.eIDOL;
                }
            }
        }

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        void Shot()
        {
            bulletFreqTime -= Time.deltaTime;

            if (Gamepad.current.rightTrigger.ReadValue() > 0.1f)
            {
                if (bulletFreqTime < 0)
                {
                    // HACK: 設計から後で修正する必要あり
                    book.Shot(targetObj.IsTargetEnemy() ? targetObj.transform.position : Camera.main.transform.forward * 1000);
                    bulletFreqTime = bulletFreq;
                }
            }
        }

        /// <summary>
        /// ターゲットロックオン
        /// </summary>
        void RockonTarget()
        {
            if (Gamepad.current.rightStickButton.wasPressedThisFrame)
            {
                // ターゲットがいる場合、プレイヤーにターゲットを戻す
                if (targetObj.IsTargetEnemy())
                {
                    Debug.Log("ロックオン解除");
                    targetObj.SetTarget(head);
                    return;
                }

                // いなければ付近の一番近い敵を探してロックオン処理を行う
                RockOn();
            }
        }

        /// <summary>
        /// ロックオン処理
        /// </summary>
        public void RockOn()
        {
            var enemys = Physics.SphereCastAll(
                this.transform.position, searchRange, this.transform.forward, 0.01f)
                .Where(h => h.transform.gameObject.tag == "Enemy")
                .Select(h => h.transform.gameObject)
                .ToList();

            if (enemys.Count > 0)
            {
                float minDistance = searchRange;
                foreach (var obj in enemys)
                {
                    float dist = Vector3.Distance(this.transform.position, obj.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;

                        targetObj.GetComponent<TargetObject>().SetTarget(obj);
                    }
                }
            }
            else
            {
                targetObj.SetTarget(head);
            }
        }

        /// <summary>
        /// 移動
        /// </summary>
        void Move()
        {
            var vec = GetDirection();
            var speed = moveSpeed;

            rigid.velocity = new Vector3(vec.x * speed, rigid.velocity.y, vec.z * speed);

            anim.SetFloat("Speed", vec.magnitude);

            if (vec.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vec, Vector3.up), rotateSpeed * Time.deltaTime);
            }

            latestPos = transform.position;
        }

        /// <summary>
        /// 入力されている方向を取得(コントローラー)
        /// </summary>
        /// <returns>移動する方向を返す</returns>
        Vector3 GetDirection()
        {
            return SetCameraDirection(new Vector3(inputVec.x, 0, inputVec.y));
        }

        /// <summary>
        /// 入力イベント
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            inputVec = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// カメラの向きから移動方向を計算して返す
        /// </summary>
        /// <param name="inputDir">入力方向</param>
        /// <returns>移動方向</returns>
        Vector3 SetCameraDirection(Vector3 inputDir)
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * inputDir.z + Camera.main.transform.right * inputDir.x;

            return moveForward;
        }
    }
}