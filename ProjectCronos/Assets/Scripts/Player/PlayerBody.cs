using UnityEngine;

namespace ProjectCronos
{
    public class PlayerBody : MonoBehaviour
    {
        /// <summary>
        /// 親
        /// </summary>
        Player player;

        /// <summary>
        /// ダメージ倍率
        /// </summary>
        [SerializeField]
        int damageRate = 1;

        void Start()
        {
            player = this.transform.parent.GetComponent<Player>();
        }

        /// <summary>
        /// ダメージを与える
        /// </summary>
        public void Damage(int value)
        {
            Debug.Log($"PlayerBodyプレイヤーに{value}を与えました。");
            player.Damage(value * damageRate);
        }
    }
}
