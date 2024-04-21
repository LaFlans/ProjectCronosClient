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
        /// <returns>この被弾により死亡した場合、Trueで返す</returns>
        public bool Damage(int value)
        {
            if (enemy == null)
            {
                Debug.Log("敵情報がnullだよ");
                return false;
            }

            return enemy.Damage(value * damageRate);
        }
    }
}
