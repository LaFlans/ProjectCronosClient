using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 魔法陣操作時用当たり判定
    /// </summary>
    class MagicCircleControlledCollider : MonoBehaviour
    {
        Material mat;
        bool isPutTrap;

        void Start()
        {
            mat = this.gameObject.GetComponent<MeshRenderer>().material;
            isPutTrap = true;
        }

        /// <summary>
        /// トラップを設置できるかどうかを返す
        /// </summary>
        /// <returns>設置できる場合、Trueを返す</returns>
        public bool IsPutTrap()
        {
            return isPutTrap;
        }

        private void OnTriggerEnter(Collider other)
        {
            UnityEngine.Debug.LogError($"入ったよ！({other.gameObject.name})");
            mat.SetColor("_EmissiveColor", Color.red);
            isPutTrap = false;
        }

        private void OnTriggerExit(Collider other)
        {
            UnityEngine.Debug.LogError($"出たよ！({other.gameObject.name})");
            mat.SetColor("_EmissiveColor", Color.green);
            isPutTrap = true;
        }
    }
}
