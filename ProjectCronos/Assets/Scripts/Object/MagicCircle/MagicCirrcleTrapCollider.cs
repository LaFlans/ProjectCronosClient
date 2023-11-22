using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 魔法陣のトラップ当たり判定
    /// </summary>
    class MagicCircleTrapCollider : MonoBehaviour
    {
        /// <summary>
        /// 魔法陣の当たり判定
        /// </summary>
        Collider col;

        void Start()
        {
            col = GetComponent<Collider>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "EnemyBody")
            {
                this.transform.parent.GetComponent<MagicCircle>().TriggerTrap();
            }
        }
    }
}
