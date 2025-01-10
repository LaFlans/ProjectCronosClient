using UnityEngine;

namespace ProjectCronos
{
    public class EnemyDamageBody : MonoBehaviour
    {
        /// <summary>
        /// 親
        /// </summary>
        [SerializeField]
        Enemy enemy;
        
        /// <summary>
        /// ダメージ倍率
        /// </summary>
        [SerializeField]
        int damageRate = 1;

        //void Start()
        //{
        //    enemy = this.transform.parent.parent.GetComponent<Enemy>();
        //}

        /// <summary>
        /// ダメージを与える
        /// </summary>
        /// <param name="value">ダメージの値</param>
        /// <param name="isRight">被弾箇所が体の右側かどうか(ここは必須)</param>
        /// <returns>この被弾により死亡した場合、Trueで返す</returns>
        public bool Damage(int value, bool isRight = false)
        {
            if (enemy == null)
            {
                Debug.Log("敵情報がnullだよ");
                return false;
            }

            return enemy.Damage(value * damageRate, isRight);
        }
    }
}
