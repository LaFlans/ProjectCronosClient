using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    /// <summary>
    /// キャラクター(プレイヤーや敵など動くやつ全般)クラス
    /// </summary>
    public class Character : MonoBehaviour
    {
        Status status = null;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
        }

        /// <summary>
        /// 更新
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual async UniTask<bool> Initialize() 
        {
            status = gameObject.GetComponent<Status>();

            return true;
        }

        /// <summary>
        /// 出現時処理
        /// </summary>
        public virtual void Appear()
        {

        }

        /// <summary>
        /// 被弾時処理
        /// </summary>
        public virtual void Damage(int value) 
        {
            // 一旦適当な値入れとく
            if (status.Damage(value))
            {
                Death();
            }
        }

        /// <summary>
        /// 死亡時処理
        /// </summary>
        public virtual void Death()
        {
            // 一旦消す
            Destroy(this.gameObject);
        }
    }
}
