using UnityEngine;

namespace ProjectCronos
{
    public class EnemyBody : MonoBehaviour
    {
        /// <summary>
        /// 親
        /// </summary>
        [SerializeField]
        Enemy enemy;

        /// <summary>
        /// 敵の親トランスフォームを取得
        /// </summary>
        public Transform GetParentObject()
        {
            return enemy.transform;
        }
    }
}
