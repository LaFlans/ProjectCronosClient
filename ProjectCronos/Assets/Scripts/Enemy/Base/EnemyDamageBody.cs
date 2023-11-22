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
        public void Damage(int value)
        {
            if (enemy == null)
            {
                Debug.Log("敵情報がnullだよ");
            }

            enemy.Damage(value * damageRate);
        }
    }
}
