using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    /// <summary>
    /// キャラクター(プレイヤーや敵など動くやつ全般)クラス
    /// </summary>
    public class Character : MonoBehaviour
    {
        protected Status status = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual async UniTask<bool> Initialize() 
        {
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
        /// <param name="value">ダメージの値</param>
        /// <param name="isRight">被弾箇所が体の右側かどうか(ここは必須)</param>
        /// <returns>この被弾により死亡した場合、Trueで返す</returns>
        public virtual bool Damage(int value, bool isRight = false) 
        {
            Debug.Log($"{value}ダメージを受けました");
            if (status != null)
            {
                if (status.DamageHp(value))
                {
                    Death();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 死亡時処理
        /// </summary>
        public virtual void Death()
        {
            // 一旦消す
            //Destroy(this.gameObject);
            StartCoroutine(DeathDelay());
        }

        IEnumerator DeathDelay()
        {
            yield return new WaitForSeconds(2.0f);

            Destroy(this.gameObject);
        }
    }
}
