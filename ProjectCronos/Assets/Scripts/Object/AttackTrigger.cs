using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace ProjectCronos
{
    /// <summary>
    /// 攻撃判定クラス
    /// </summary>
    public class AttackTrigger : MonoBehaviour
    {
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
        /// 生きているかどうか
        /// </summary>
        bool isAlive = true;

        bool isAct;

        public void SetIsAct(bool result)
        {
            isAct = result;
        }

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            col = GetComponent<Collider>();
            if (col == null)
            {
                Debug.Log("当たり判定コンポーネントが設定されていません");
            }

            isAct = true;
            moveSpeed = defaultMoveSpeed;
        }

        public async void Init(GameObject target, float lifeTime, EnumCollection.Attack.ATTACK_TYPE type, bool isDestory = false)
        {
            this.target = target;
            attackType = type;
            Destroy(this.gameObject, lifeTime);
            isHitDestory = isDestory;
            this.transform.LookAt(target.transform);

            await MoveDelay();
        }

        void FixedUpdate()
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
            }
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
                        col.gameObject.GetComponent<EnemyBody>().Damage(1);
                        Vector3 hitPos = col.ClosestPointOnBounds(this.transform.position);
                        Utility.CreateObject("Prefabs/DamageEffect1", hitPos, 1.0f);

                        if (isHitDestory)
                        {
                            Destroy(this.gameObject);
                        }
                    }

                    break;
                case EnumCollection.Attack.ATTACK_TYPE.ENEMY:
                    if (col.gameObject.tag == "PlayerBody")
                    {
                        col.gameObject.GetComponent<PlayerBody>().Damage(1);
                        Vector3 hitPos = col.ClosestPointOnBounds(this.transform.position);
                        Utility.CreateObject("Prefabs/DamageEffect1", hitPos, 1.0f);

                        if (isHitDestory)
                        {
                            Destroy(this.gameObject);
                        }
                    }

                    break;
            }

            if (col.gameObject.tag == "Ground")
            {
                Vector3 hitPos = col.ClosestPointOnBounds(this.transform.position);
                Utility.CreateObject("Prefabs/DamageEffect1", hitPos, 1.0f);
                Destroy(this.gameObject);
            }
        }
    }
}
