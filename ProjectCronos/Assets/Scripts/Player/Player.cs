using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cinemachine;

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

        [SerializeField]
        int attackMoveDelayRate = 10;

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

        [SerializeField]
        Transform[] spawnPos;

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
        /// プレイヤーの中心の位置を示すオブジェクト
        /// </summary>
        [SerializeField]
        GameObject center;

        /// <summary>
        /// ジャンプしたか
        /// </summary>
        bool isJump = false;

        /// <summary>
        /// 地面に着地している状態か
        /// </summary>
        bool isGround = false;

        [SerializeField]
        GroundChecker groundChecker;

        /// <summary>
        /// ジャンプ力
        /// </summary>
        [SerializeField]
        float jumpPower = 10.0f;

        /// <summary>
        /// 操作可能かどうか
        /// </summary>
        bool isControl;

        /// <summary>
        /// 時間停止中かどうか
        /// </summary>
        bool isTimeStopWorld;

        EnumCollection.Player.PLAYER_JUMP_STATE jumpState = EnumCollection.Player.PLAYER_JUMP_STATE.IDOL;

        string demonHandPrefabPath = "Assets/Resources_moved/Prefabs/DemonHand.prefab";

        /// <summary>
        /// プレイヤー用カメラ
        /// </summary>
        PlayerCamera playerCamera;

        /// <summary>
        /// 初期化
        /// </summary>
        public override async UniTask<bool> Initialize()
        {
            await base.Initialize();

            rigid = this.GetComponent<Rigidbody>();
            anim = this.GetComponent<Animator>();

            // ステータス設定
            status = this.GetComponent<PlayerStatus>();

            //　地面判定設定
            groundChecker.Initialized(OnLanding, OnTakeoff);

            // カメラ設定
            playerCamera = this.GetComponent<PlayerCamera>();
            playerCamera.Initialize(this.transform);

            // 状態設定
            jumpState = EnumCollection.Player.PLAYER_JUMP_STATE.IDOL;

            // 時間停止状態設定
            isTimeStopWorld = false;

            //　事前読み込み完了時操作可能状態にする
            isControl = true;

            return true;
        }

        /// <summary>
        /// 事前読み込み
        /// マネージャー系生成後に呼ばれる
        /// </summary>
        /// <returns></returns>
        public async UniTask PreLoadAsync()
        {
            await AddressableManager.Instance.Load(demonHandPrefabPath);

            // 入力イベント設定
            SetInputAction();
        }

        /// <summary>
        /// プレイヤーの中心のトランスフォームを渡す
        /// </summary>
        /// <returns>プレイヤーの中心のトランスフォーム</returns>
        public Transform GetCenterPos()
        {
            return center.transform;
        }

        void SetInputAction()
        {
            // 移動
            InputManager.Instance.inputActions.Player.Move.started += OnMove;
            InputManager.Instance.inputActions.Player.Move.performed += OnMove;
            InputManager.Instance.inputActions.Player.Move.canceled += OnMove;

            // ジャンプ
            InputManager.Instance.inputActions.Player.Jump.performed += OnJump;

            // 攻撃
            InputManager.Instance.inputActions.Player.Attack.performed += OnAttack;

            // プレイヤー操作不能時にFreeLookカメラを操作できないようにする処理を登録
            // FIXME: providerのenableを切り替える暫定対応の為、InputActionsの有効無効に影響されるようにする
            InputManager.Instance.RegistSwitchInputStatusEvent(
                () => 
                {
                    playerCamera.EnableFreeLookCamera();
                });

            // ロックオン
            InputManager.Instance.inputActions.Player.RockOn.performed += OnRockon;

            // テスト
            InputManager.Instance.inputActions.Player.Test.performed += OnTest;
        }

        void RemoveInputAction()
        {
            // 移動
            InputManager.Instance.inputActions.Player.Move.started -= OnMove;
            InputManager.Instance.inputActions.Player.Move.performed -= OnMove;
            InputManager.Instance.inputActions.Player.Move.canceled -= OnMove;

            // ジャンプ
            InputManager.Instance.inputActions.Player.Jump.performed -= OnJump;

            // 攻撃
            InputManager.Instance.inputActions.Player.Attack.performed -= OnAttack;

            // ロックオン
            InputManager.Instance.inputActions.Player.RockOn.performed -= OnRockon;

            // テスト
            InputManager.Instance.inputActions.Player.Test.performed -= OnTest;
        }

        void OnDestroy()
        {
            Debug.Log("プレイヤーのオブジェクト破壊");

            // 入力イベントを外す
            RemoveInputAction();
        }

        void OnTest(InputAction.CallbackContext context)
        {
            //// プレイヤーにダメージを与えるテスト
            //UnityEngine.Debug.Log("ダメージテスト");
            //Damage(1);

            // 時間停止テスト
            UnityEngine.Debug.Log("時間停止テスト");
            isTimeStopWorld = !isTimeStopWorld;
            TimeManager.Instance.ApplyEnemyTimeScale(isTimeStopWorld ? 0.0f : 1.0f);
            TimeManager.Instance.ApplyObjectTimeScale(isTimeStopWorld ? 0.0f : 1.0f);
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

            SoundManager.Instance.Play("Damage1");
        }

        /// <summary>
        /// 死亡時
        /// </summary>
        public override void Death()
        {
            // 入力イベントを外す
            RemoveInputAction();

            base.Death();
        }

        /// <summary>
        /// Update
        /// </summary>
        void Update()
        {
            if (isControl && InputManager.Instance.IsMatchInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER))
            {
                if (jumpState == EnumCollection.Player.PLAYER_JUMP_STATE.JUMP && isGround)
                {
                    jumpState = EnumCollection.Player.PLAYER_JUMP_STATE.IDOL;
                    rigid.velocity = Vector3.zero;
                }
            }
        }

        void OnAttack(InputAction.CallbackContext context)
        {
            anim.SetTrigger("Attack");
        }

        /// <summary>
        /// 第一段階攻撃アニメーションイベント
        /// </summary>
        void AnimEventAttackFirst()
        {
            GameObject obj = AddressableManager.Instance.GetLoadedObject(demonHandPrefabPath);
            obj.transform.position = spawnPos[0].position;
            obj.transform.rotation = spawnPos[0].rotation;
            obj.transform.localScale = spawnPos[0].localScale;
        }

        /// <summary>
        /// 第一段階攻撃アニメーションイベント
        /// </summary>
        void AnimEventAttackSecond()
        {
            GameObject obj = AddressableManager.Instance.GetLoadedObject(demonHandPrefabPath);
            obj.transform.position = spawnPos[1].position;
            obj.transform.rotation = spawnPos[1].rotation;
            obj.transform.localScale = spawnPos[1].localScale;
        }

        /// <summary>
        /// 第一段階攻撃アニメーションイベント
        /// </summary>
        void AnimEventAttackThird()
        {
            GameObject obj = AddressableManager.Instance.GetLoadedObject(demonHandPrefabPath);
            obj.transform.position = spawnPos[2].position;
            obj.transform.rotation = spawnPos[2].rotation;
            obj.transform.localScale = spawnPos[2].localScale;
        }

        /// <summary>
        /// FixedUpdate
        /// </summary>
        void FixedUpdate()
        {
            if (isControl && InputManager.Instance.IsMatchInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER))
            {
                if (isJump)
                {
                    isJump = false;
                    jumpState = EnumCollection.Player.PLAYER_JUMP_STATE.JUMP;
                    rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

                    SoundManager.Instance.Play("TakeoffPlayer");
                }

                Move();
            }
        }

        /// <summary>
        /// プレイヤーの着地判定時のイベント
        /// </summary>
        public void OnLanding()
        {
            isGround = true;
            anim.SetBool("IsGround", true);

            SoundManager.Instance.Play("LandingPlayer");
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
        void OnJump(InputAction.CallbackContext context)
        {
            if (isGround && 
                jumpState == EnumCollection.Player.PLAYER_JUMP_STATE.IDOL)
            {
                jumpState = EnumCollection.Player.PLAYER_JUMP_STATE.START;
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
                    jumpState = EnumCollection.Player.PLAYER_JUMP_STATE.IDOL;
                }
            }
        }

        ///// <summary>
        ///// 弾を撃つ
        ///// </summary>
        //void Shot()
        //{
        //    bulletFreqTime -= Time.deltaTime;

        //    if (Gamepad.current.rightTrigger.ReadValue() > 0.1f)
        //    {
        //        if (bulletFreqTime < 0)
        //        {
        //            // HACK: 設計から後で修正する必要あり
        //            book.Shot(targetObj.IsTargetEnemy() ? targetObj.transform.position : Camera.main.transform.forward * 1000);
        //            bulletFreqTime = bulletFreq;
        //        }
        //    }
        //}

        /// <summary>
        /// ターゲットロックオン
        /// </summary>
        void OnRockon(InputAction.CallbackContext context)
        {
            if (playerCamera.IsRockOn())
            {
                playerCamera.CancelRockOn();
            }
            else
            {
                // 敵を探す
                var enemys = Physics.SphereCastAll(
                    transform.position, searchRange, transform.forward, 0.01f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
                    .Where(h => h.transform.gameObject.tag == "Enemy")
                    .Select(h => h.transform.gameObject)
                    .ToList();

                if (enemys.Count > 0)
                {
                    float minDistance = 0;
                    Transform targetEnemy = null;
                    foreach (var enemy in enemys)
                    {
                        float dist = Vector3.Distance(transform.position, enemy.transform.position);
                        if (dist < minDistance || minDistance == 0)
                        {
                            minDistance = dist;
                            targetEnemy = enemy.transform;
                        }
                    }

                    // ロックオンを行う
                    playerCamera.RockOn(targetEnemy);
                }
            }
        }

        ///// <summary>
        ///// ロックオン処理
        ///// </summary>
        //void SetTarget()
        //{
        //    if (targetGroup.m_Targets.Count() > 1)
        //    {
        //        // 既に敵をロックオン中なので、解除する
        //        // ターゲットを全解除
        //        foreach (var target in targetGroup.m_Targets)
        //        {
        //            targetGroup.RemoveMember(target.target);
        //        }

        //        //　プレイヤーの頭を対象に追加
        //        targetGroup.AddMember(head.transform, 1, 1);
        //    }
        //    else
        //    {
        //        // 敵を探す
        //        var enemys = Physics.SphereCastAll(
        //            transform.position, searchRange, transform.forward, 0.01f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
        //            .Where(h => h.transform.gameObject.tag == "Enemy")
        //            .Select(h => h.transform.gameObject)
        //            .ToList();

        //        if (enemys.Count > 0)
        //        {
        //            Debug.Log("敵をロックオンしたよ！");
        //            float minDistance = searchRange;
        //            Transform targetEnemy = null;
        //            foreach (var enemy in enemys)
        //            {
        //                float dist = Vector3.Distance(this.transform.position, enemy.transform.position);
        //                if (dist < minDistance)
        //                {
        //                    minDistance = dist;
        //                    targetEnemy = enemy.GetComponent<EnemyInfo>().GetHeadPos();
        //                }
        //            }

        //            //　プレイヤーの頭を対象に追加
        //            targetGroup.AddMember(targetEnemy, 1, 1);
        //            rockOnCamera.Priority = 15;
        //        }
        //    }
        //}

        private void OnDrawGizmos()
        {
            var isHitAll = Physics.SphereCastAll(transform.position, searchRange, transform.forward, 0.01f);
            foreach (var hit in isHitAll)
            {
                Gizmos.DrawWireSphere(transform.position + transform.forward * (hit.distance), searchRange);
            }
        }

        /// <summary>
        /// 移動
        /// </summary>
        void Move()
        {
            var vec = GetDirection();

            // アニメーション中は移動速度を落とす
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                rigid.velocity = new Vector3(vec.x * moveSpeed / attackMoveDelayRate, rigid.velocity.y, vec.z * moveSpeed / attackMoveDelayRate);
            }
            else
            {
                rigid.velocity = new Vector3(vec.x * moveSpeed, rigid.velocity.y, vec.z * moveSpeed);
            }

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
