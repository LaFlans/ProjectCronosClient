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
        [SerializeField]
        float speed = 10;
        Vector3 vec = Vector3.zero;

        GameObject target;

        /// <summary>
        /// 移動可能か
        /// </summary>
        bool isMove = false;

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
        /// 開始処理
        /// </summary>
        void Start()
        {
            col = GetComponent<Collider>();
            if (col == null)
            {
                Debug.Log("当たり判定コンポーネントが設定されていません");
            }
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
            if (isMove)
            {
                this.transform.position += vec * speed;
            }
            else
            {
                //　移動できない間は、向きだけ更新
                this.transform.LookAt(target.transform);
            }
        }

        void SetVec()
        {
            this.vec = target.transform.position - this.transform.position;
        }

        /// <summary>
        /// AIの思考インターバル
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
                    if (col.gameObject.tag == "Enemy")
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
                    if (col.gameObject.tag == "Player")
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
