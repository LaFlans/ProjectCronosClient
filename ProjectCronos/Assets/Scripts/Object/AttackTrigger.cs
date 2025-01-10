using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.VFX;

namespace ProjectCronos
{
    /// <summary>
    /// 攻撃判定クラス
    /// </summary>
    public class AttackTrigger : MonoBehaviour
    {
        MeshRenderer meshRenderer;
        Material mat;

        /// <summary>
        /// デフォルトの移動速度
        /// </summary>
        [SerializeField]
        float defaultMoveSpeed = 10;

        /// <summary>
        /// 計算で使用する速度
        /// </summary>
        [SerializeField]
        float moveSpeed;

        /// <summary>
        /// 移動ベクトル
        /// </summary>
        Vector3 moveVec = Vector3.zero;

        /// <summary>
        /// 追尾対象
        /// </summary>
        GameObject target;

        /// <summary>
        /// 移動可能か
        /// </summary>
        bool isMove = false;

        /// <summary>
        /// 遅延時間
        /// </summary>
        float delayTime = 1.0f;

        /// <summary>
        /// 当たり判定コンポーネント
        /// </summary>
        Collider col;

        /// <summary>
        /// 攻撃の種類(敵かプレイヤーか)
        /// </summary>
        EnumCollection.Attack.ATTACK_TYPE attackType = EnumCollection.Attack.ATTACK_TYPE.ENEMY;

        /// <summary>
        /// 接触時に消すかどうか
        /// </summary>
        bool isHitDestory = false;

        /// <summary>
        /// ステージに当たり判定があるかどうか
        /// </summary>
        bool isHitStage = false;

        bool isAct;

        /// <summary>
        /// 攻撃力
        /// </summary>
        int attack;

        public void SetIsAct(bool result)
        {
            isAct = result;
        }

        void Initialize()
        {
            col = GetComponent<Collider>();
            if (col == null)
            {
                Debug.Log("当たり判定コンポーネントが設定されていません");
            }

            isAct = true;
            moveSpeed = defaultMoveSpeed;

            meshRenderer = this.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                mat = meshRenderer.material;
            }

            // 初期状態は当たり判定はない状態
            DisableCollider();
        }

        public async void Init(EnumCollection.Attack.ATTACK_TYPE type, int attack, bool isHitDestory = false)
        {
            Initialize();

            attackType = type;
            this.attack = attack;
            this.isHitDestory = isHitDestory;
            isHitStage = false;

            EnableCollider();
        }

        public async void Init(Vector3 targetVec, float lifeTime,
EnumCollection.Attack.ATTACK_TYPE type, int attack, float delayTime,
 bool isHitDestory = false)
        {
            Initialize();

            this.delayTime = delayTime;

            this.moveVec = targetVec;
            Destroy(gameObject, lifeTime);
            attackType = type;
            this.attack = attack;
            this.isHitDestory = isHitDestory;
            isHitStage = true;

            EnableCollider();

            isMove = true;
        }


        public async void Init(
            GameObject target,
            float lifeTime,
            EnumCollection.Attack.ATTACK_TYPE type,
            int attack,
            float delayTime,
            bool isHitDestory = false)
        {
            Initialize();

            this.delayTime = delayTime;
            this.target = target;
            attackType = type;
            Destroy(gameObject, lifeTime);
            this.isHitDestory = isHitDestory;
            transform.LookAt(target.transform);
            this.attack = attack;
            isHitStage = true;

            await MoveDelay();
            EnableCollider();
        }

        void FixedUpdate()
        {
            Move();
        }

        protected virtual void Move()
        {
            if (!isAct)
            {
                // 行動できない状態の場合、何もしない
                return;
            }

            if (isMove)
            {
                this.transform.position += moveVec * moveSpeed * Time.deltaTime;
            }
            else
            {
                if (target != null)
                {
                    //　移動できない間は、向きだけ更新
                    this.transform.LookAt(target.transform);
                }
            }
        }

        void SetVec()
        {
            if (target != null && this != null)
            {
                this.moveVec = Vector3.Normalize(target.transform.position - this.transform.position);
            }
        }

        /// <summary>
        /// 移動遅延
        /// </summary>
        async UniTask MoveDelay()
        {
            isMove = false;

            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));

            SetVec();
            isMove = true;
        }

        /// <summary>
        /// 当たり判定を有効にする
        /// </summary>
        public void EnableCollider()
        {
            if (!col.enabled)
            {
                col.enabled = true;
                SetDebugColliderColor();
            }
        }

        /// <summary>
        /// 当たり判定を無効にする
        /// </summary>
        public void DisableCollider()
        {
            if (col.enabled)
            {
                col.enabled = false;
                SetDebugColliderColor();
            }
        }

        void SetDebugColliderColor()
        {
            if (mat != null)
            {
                mat.SetColor("_EmissiveColor", col.enabled ? Color.green : Color.red);
            }
        }

        /// <summary>
        /// 攻撃の種類を返す
        /// </summary>
        /// <returns>攻撃の種類(敵かプレイヤーか)</returns>
        public EnumCollection.Attack.ATTACK_TYPE GetAttackType()
        {
            return attackType;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col == null)
            {
                // 当たり判定コライダがない場合、何もしない
                return;
            }

            switch (attackType)
            {
                case EnumCollection.Attack.ATTACK_TYPE.PLAYER:
                    if (col.gameObject.tag == "EnemyBody")
                    {

                        Vector3 hitPos = col.ClosestPointOnBounds(this.transform.position);
                        Utility.CreateObject("Assets/Resources_moved/Prefabs/Effects/DamageEffect1.prefab", hitPos, 1.0f);

                        var ab = col.transform.forward - col.transform.position;
                        var ac = hitPos - col.transform.position;
                        var cross = Vector3.Dot(ab, ac);

                        DamageLogger.ShowLog(attack, hitPos, EnumCollection.Attack.ATTACK_TYPE.PLAYER);

                        if (col.gameObject.GetComponent<EnemyDamageBody>().Damage(attack, cross > 0))
                        {
                            var rididBody = col.gameObject.GetComponent<Rigidbody>();
                            rididBody?.AddForce(this.transform.up * 50, ForceMode.Impulse);
                        }

                        if (isHitDestory)
                        {
                            Destroy(this.gameObject);
                        }
                    }

                    if (col.gameObject.tag == "AttackObject")
                    {
                        if (col.gameObject.GetComponent<AttackTrigger>().GetAttackType() == EnumCollection.Attack.ATTACK_TYPE.ENEMY)
                        {
                            Debug.Log($"エネミーの武器にあたりました！");
                            Vector3 hitPos = col.ClosestPointOnBounds(this.transform.position);
                            Utility.CreateObject("Assets/Resources_moved/Prefabs/Effects/DamageEffect1.prefab", hitPos, 1.0f);
                            Destroy(col.gameObject);
                        }
                    }

                    if (col.gameObject.tag == "BoundObject")
                    {
                        var rididBody = col.gameObject.GetComponent<Rigidbody>();
                        Debug.Log($"バウンドオブジェクトにあたりました！");
                        Vector3 hitPos = col.ClosestPointOnBounds(this.transform.position);
                        rididBody?.AddForce(this.transform.up * 50, ForceMode.Impulse);
                        Utility.CreateObject("Assets/Resources_moved/Prefabs/Effects/DamageEffect1.prefab", hitPos, 1.0f);
                    }

                    break;
                case EnumCollection.Attack.ATTACK_TYPE.ENEMY:
                    if (col.gameObject.tag == "PlayerBody")
                    {
                        Debug.Log($"AttackTriggerプレイヤーに{attack}を与えました。");
                        col.gameObject.GetComponent<PlayerBody>().Damage(attack);
                        Vector3 hitPos = col.ClosestPointOnBounds(this.transform.position);
                        Utility.CreateObject("Assets/Resources_moved/Prefabs/Effects/DamageEffect1.prefab", hitPos, 1.0f);

                        DamageLogger.ShowLog(attack, hitPos, EnumCollection.Attack.ATTACK_TYPE.ENEMY);

                        if (isHitDestory)
                        {
                            Destroy(this.gameObject);
                        }
                    }

                    break;
            }

            if (isHitStage && col.gameObject.CompareTag("Ground"))
            {
                //Debug.Log("地面にあたりました");
                Vector3 hitPos = col.ClosestPointOnBounds(this.transform.position);
                Utility.CreateObject("Assets/Resources_moved/Prefabs/Effects/DamageEffect1.prefab", hitPos, 1.0f);
                if (isHitDestory)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
