using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cinemachine;
using System.Diagnostics.Contracts;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.UIElements;

namespace ProjectCronos
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(PlayerStatus))]
    public class Player : Character
    {
        /// <summary>
        /// 攻撃中の移動速度低下倍率
        /// </summary>
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
        /// 操作可能かどうか
        /// </summary>
        bool isControl;

        EnumCollection.Player.PLAYER_JUMP_STATE jumpState = EnumCollection.Player.PLAYER_JUMP_STATE.IDOL;

        string demonHandPrefabPath = "Assets/Resources_moved/Prefabs/DemonHand.prefab";
        string demonHandTrapPrefabPath = "Assets/Resources_moved/Prefabs/MagicCircle/DemonHandTrap.prefab";
        //string fireSkeltonPrefabPath = "Assets/Prefabs/FireSkelton.prefab";
        string fireSkeltonPrefabPath = "Assets/Prefabs/NierBullet.prefab";

        /// <summary>
        /// プレイヤー用カメラ
        /// </summary>
        PlayerCamera playerCamera;

        /// <summary>
        /// プレイヤーのスキル
        /// </summary>
        PlayerSkill playerSkill;

        /// <summary>
        /// プレイヤーステータス情報
        /// </summary>
        PlayerStatus playerStatus;

        [SerializeField]
        float dodgeRollPower = 5;

        bool isDodge = false;

        [SerializeField]
        Animator testEnemyAnim;

        [SerializeField]
        Transform attackObjParent;


        GameObject controlMagicCircle;

        [SerializeField]
        float controlMagicCircleSpeed = 1.0f;

        /// <summary>
        /// 初期化
        /// </summary>
        public override async UniTask<bool> Initialize()
        {
            await base.Initialize();

            rigid = this.GetComponent<Rigidbody>();
            anim = this.GetComponent<Animator>();

            // ステータス設定
            playerStatus = this.GetComponent<PlayerStatus>();
            playerStatus.Initialize();
            status = playerStatus;

            // FIXME: 本来は別の場所でやる
            // アイテム画像読み込み
            foreach (var item in MasterDataManager.DB.ItemDataTable.All)
            {
                await AddressableManager.Instance.LoadTexture(item.Path);
            }

            // プレイヤーの位置をセーブデータで保存されている位置に移動
            if (SaveManager.Instance.lastLoadSaveData != null)
            {
                var saveAreaInfo = SaveManager.Instance.lastLoadSaveData.saveAreaInfo;
                this.transform.position = saveAreaInfo.respawnPosition;
                this.transform.rotation = saveAreaInfo.respawnRotation;
            }

            //　地面判定設定
            groundChecker.Initialized(OnLanding, OnTakeoff);

            // カメラ設定
            playerCamera = this.GetComponent<PlayerCamera>();
            playerCamera.Initialize(this.transform);

            // 状態設定
            jumpState = EnumCollection.Player.PLAYER_JUMP_STATE.IDOL;

            // プレイヤースキル初期化
            playerSkill = this.GetComponent<PlayerSkill>();
            playerSkill.Initialize();

            // 入力イベント設定
            SetInputAction();

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
            await AddressableManager.Instance.Load(demonHandTrapPrefabPath);
            await AddressableManager.Instance.Load(fireSkeltonPrefabPath);

            // 魔法陣マテリアル読み込み
            await AddressableManager.Instance.LoadMaterial("Assets/ProjectCronosAssets/Materials/MagicCircle/MagicCircleLevel1.mat");
            await AddressableManager.Instance.LoadMaterial("Assets/ProjectCronosAssets/Materials/MagicCircle/MagicCircleLevel2.mat");
            await AddressableManager.Instance.LoadMaterial("Assets/ProjectCronosAssets/Materials/MagicCircle/MagicCircleLevel3.mat");
            await AddressableManager.Instance.LoadMaterial("Assets/ProjectCronosAssets/Materials/MagicCircle/MagicCircleLevel4.mat");
            await AddressableManager.Instance.LoadMaterial("Assets/ProjectCronosAssets/Materials/MagicCircle/MagicCircleLevel5.mat");
            await AddressableManager.Instance.LoadMaterial("Assets/ProjectCronosAssets/Materials/MagicCircle/MagicCircleLevel6.mat");
            await AddressableManager.Instance.LoadMaterial("Assets/ProjectCronosAssets/Materials/MagicCircle/MagicCircleLevel7.mat");
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

            // 第一のスキル
            InputManager.Instance.inputActions.Player.FirstSkill.performed += playerSkill.OnActiveFirstSkill;

            //InputManager.Instance.inputActions.Player.Attack2.performed += Shot;

            // 回避
            InputManager.Instance.inputActions.Player.Dodge.performed += OnDodge;
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

            // 第一のスキル
            InputManager.Instance.inputActions.Player.FirstSkill.performed -= playerSkill.OnActiveFirstSkill;

            //InputManager.Instance.inputActions.Player.Attack2.performed -= Shot;

            // 回避
            InputManager.Instance.inputActions.Player.Dodge.performed -= OnDodge;     
        }

        void OnDestroy()
        {
            Debug.Log("プレイヤーのオブジェクト破壊");

            // 入力イベントを外す
            RemoveInputAction();
        }

        /// <summary>
        /// 回避処理
        /// </summary>
        void OnDodge(InputAction.CallbackContext context)
        {
            // 操作中の魔法陣がある場合、回避できない
            if (controlMagicCircle != null)
            {
                return;
            }

            if (!isDodge && isGround)
            {
                isDodge = true;
                anim.SetTrigger("Dodge");
                rigid.linearVelocity = Vector3.zero;

                var moveVec = Vector3.ProjectOnPlane(this.transform.forward, normalVector);

                rigid.AddForce(moveVec * dodgeRollPower, ForceMode.Impulse);
            }
        }

        void OnTest(InputAction.CallbackContext context)
        {
            if (controlMagicCircle == null)
            {
                // トラップ攻撃テスト
                GameObject obj = AddressableManager.Instance.GetLoadedObject(demonHandTrapPrefabPath);
                obj.transform.position = this.transform.position;
                obj.transform.position += new Vector3(0, 0.1f, 0);
                obj.GetComponent<DemonHand>().Initialize(10, 1);
                controlMagicCircle = obj;
            }
            else
            {
                if(controlMagicCircle.GetComponent<MagicCircle>().IsPutTrap())
                {
                    // 魔法陣の操作を解除
                    controlMagicCircle.GetComponent<MagicCircle>().PutTrap();
                    controlMagicCircle = null;
                }
                else
                {
                    Debug.LogError("この場所にはトラップを設置することができません");
                }
            }

            //if(isTest)
            //{
            //    isTest = false;
            //    SoundManager.Instance.Play("WorkBGM1");
            //}
            //else
            //{
            //    isTest = true;
            //    SoundManager.Instance.Play("WorkBGM2");
            //}

            //SoundManager.Instance.Play("Shot1");

            //UnityEngine.Debug.Log("敵攻撃テスト");
            //testEnemyAnim.SetTrigger("Attack");

            //// プレイヤーにダメージを与えるテスト
            //UnityEngine.Debug.Log("ダメージテスト");
            //Damage(1);
        }

        /// <summary>
        /// 出現時処理
        /// </summary>
        public override void Appear()
        {
            // プレイヤーは一旦何もしない
        }

        /// <summary>
        /// 被弾時処理
        /// </summary>
        /// <param name="value">ダメージの値</param>
        /// <returns>この被弾により死亡した場合、Trueで返す</returns>
        public override bool Damage(int value)
        {
            SoundManager.Instance.Play("Damage1");
            return base.Damage(value);
        }

        /// <summary>
        /// 死亡時
        /// </summary>
        public override void Death()
        {
            // 入力イベントを外す
            RemoveInputAction();

            base.Death();

            ManagerScene.SetGameStatus(EnumCollection.Game.GAME_STATUS.GAME_OVER);
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
                    rigid.linearVelocity = Vector3.zero;
                }

                // 弾の発射間隔の計算
                bulletFreqTime -= Time.deltaTime;

                if (InputManager.Instance.inputActions.Player.Attack2.IsPressed())
                {
                    Shot();
                }
            }
        }

        float rayDist = 1.0f;

        // 高さ調整
        void AdjustmentHeight()
        {
            // 正面レイ
            var origin = this.transform.position + (this.transform.forward * 0.4f) + (Vector3.up * 1.0f);
            var direction = Vector3.down * rayDist;
            Ray ray = new Ray(origin, direction);
            //Debug.DrawRay(origin, direction, Color.blue);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDist))
            {
                //Debug.LogError($"hit:{hit.collider.name}");

                if (hit.collider.tag == "Ground")
                {
                    // 水平であればプレイヤーの高さを補正する
                    var hitNormal = hit.normal;
                    var angle = Vector3.Angle(hitNormal, Vector3.up);
                    //Debug.LogError($"当たっているオブジェクト:{hitNormal}:{(int)angle}°");
                    if (angle <= 30)
                    {
                        var vec = this.transform.position;
                        vec.y = hit.point.y;
                        this.transform.position = vec;
                    }
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
            obj.GetComponent<DemonHand>().Initialize(5, 1);
        }

        /// <summary>
        /// 第二段階攻撃アニメーションイベント
        /// </summary>
        void AnimEventAttackSecond()
        {
            GameObject obj = AddressableManager.Instance.GetLoadedObject(demonHandPrefabPath);
            obj.transform.position = spawnPos[1].position;
            obj.transform.rotation = spawnPos[1].rotation;
            obj.transform.localScale = spawnPos[1].localScale;
            obj.GetComponent<DemonHand>().Initialize(10, 1);
        }

        /// <summary>
        /// 第三段階攻撃アニメーションイベント
        /// </summary>
        void AnimEventAttackThird()
        {
            GameObject obj = AddressableManager.Instance.GetLoadedObject(demonHandPrefabPath);
            obj.transform.position = spawnPos[2].position;
            obj.transform.rotation = spawnPos[2].rotation;
            obj.transform.localScale = spawnPos[2].localScale;
            obj.GetComponent<DemonHand>().Initialize(15, 1);
        }

        /// <summary>
        /// 回避終了イベント
        /// </summary>
        void EventFinishDodge()
        {
            isDodge = false;
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
                    rigid.AddForce(Vector3.up * playerStatus.jumpPower, ForceMode.Impulse);

                    SoundManager.Instance.Play("TakeoffPlayer");
                }

                if (!isDodge)
                {
                    // 操作途中の魔法陣がある場合、移動はできない
                    if (controlMagicCircle == null)
                    {
                        // 回避中は移動できない
                        Move();
                    }
                    else
                    {
                        // 魔法陣移動
                        MoveMagicCircle();
                    }
                }
            }
        }

        /// <summary>
        /// プレイヤーの着地判定時のイベント
        /// </summary>
        public void OnLanding()
        {
            if (!isGround)
            {
                isGround = true;
                anim.SetBool("IsGround", true);

                SoundManager.Instance.Play("LandingPlayer");
            }
        }

        /// <summary>
        /// プレイヤーの離陸判定時のイベント
        /// </summary>
        public void OnTakeoff()
        {
            if (isGround)
            {
                isGround = false;
                anim.SetBool("IsGround", false);
            }
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

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        void Shot()
        {
            if (bulletFreqTime <= 0)
            {
                //var pos = spawnPos[UnityEngine.Random.Range(0, spawnPos.Count)];
                //if (pos != null)
                {
                    GameObject obj = AddressableManager.Instance.GetLoadedObject(fireSkeltonPrefabPath);
                    obj.transform.position = this.transform.position + new Vector3(0,2,0);
                    obj.transform.SetParent(attackObjParent);
                    SoundManager.Instance.Play("Shot1");

                    if (playerCamera.GetTarget() != null)
                    {
                        obj.GetComponent<AttackTrigger>().Init(
                            playerCamera.GetTarget().gameObject,
                            10,
                            EnumCollection.Attack.ATTACK_TYPE.PLAYER,
                            10,
                            0.0f,
                            true);
                    }
                    else
                    {
                        obj.GetComponent<AttackTrigger>().Init(
                            Camera.main.transform.forward,
                            10,
                            EnumCollection.Attack.ATTACK_TYPE.PLAYER,
                            status.attack,
                            0.0f,
                            true);
                    }
                }

                // HACK: 設計から後で修正する必要あり
                //book.Shot(targetObj.IsTargetEnemy() ? targetObj.transform.position : Camera.main.transform.forward * 1000);
                bulletFreqTime = bulletFreq;
            }
        }

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
                    GameObject targetEnemy = null;
                    foreach (var enemy in enemys)
                    {
                        float dist = Vector3.Distance(transform.position, enemy.transform.position);
                        if (dist < minDistance || minDistance == 0)
                        {
                            minDistance = dist;
                            targetEnemy = enemy;
                        }
                    }

                    // ロックオンを行う
                    playerCamera.RockOn(targetEnemy.GetComponent<EnemyBody>().GetParentObject().GetComponent<EnemyInfo>().GetCenterPos());
                }
            }
        }

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

            var moveVec = Vector3.ProjectOnPlane(vec, normalVector);

            // 高さ調整
            if (isGround)
            {
                AdjustmentHeight();
            }

            // アニメーション中は移動速度を落とす
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                //rigid.velocity = new Vector3(vec.x * status.moveSpeed / attackMoveDelayRate, rigid.velocity.y, vec.z * status.moveSpeed / attackMoveDelayRate);

                rigid.linearVelocity = new Vector3(
                    moveVec.x * status.moveSpeed / attackMoveDelayRate,
                    rigid.linearVelocity.y,
                    moveVec.z * status.moveSpeed / attackMoveDelayRate);
            }
            else
            {
                //rigid.velocity = new Vector3(vec.x * status.moveSpeed, rigid.velocity.y, vec.z * status.moveSpeed);

                rigid.linearVelocity = new Vector3(
                    moveVec.x * status.moveSpeed,
                    rigid.linearVelocity.y,
                    moveVec.z * status.moveSpeed);
            }

            anim.SetFloat("Speed", vec.magnitude);

            if (vec.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vec, Vector3.up), rotateSpeed * Time.deltaTime);
            }


            latestPos = transform.position;
        }

        /// <summary>
        /// 魔法陣移動
        /// </summary>
        void MoveMagicCircle()
        {
            var vec = GetDirection();

            // プレイヤーの移動を止める
            rigid.linearVelocity = Vector3.zero;
            anim.SetFloat("Speed", 0);


            var moveVec = Vector3.ProjectOnPlane(vec, normalVector);

            controlMagicCircle.transform.position += moveVec * controlMagicCircleSpeed;
        }

        /// <summary>
        /// 入力されている方向を取得(コントローラー)
        /// </summary>
        /// <returns>移動する方向を返す</returns>
        Vector3 GetDirection()
        {
            return SetCameraDirection(new Vector3(inputVec.x, 0, inputVec.y));
            //return SetCameraDirection(new Vector3(inputVec.x, 0, inputVec.y));
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

        Vector3 normalVector = Vector3.zero;

        private void OnCollisionStay(Collision collision)
        {
            // 衝突した面の、接触した点における法線を取得
            normalVector = collision.contacts[0].normal;
        }
    }
}
