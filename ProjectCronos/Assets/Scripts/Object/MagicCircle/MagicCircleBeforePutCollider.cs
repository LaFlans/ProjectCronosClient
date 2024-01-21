using UnityEngine;

namespace ProjectCronos
{
    class MagicCircleBeforePutCollider : MonoBehaviour
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
            //UnityEngine.Debug.LogError("入ったよ！");
            mat.SetColor("_EmissiveColor", Color.red);
            isPutTrap = false;
        }

        private void OnTriggerExit(Collider other)
        {
            //UnityEngine.Debug.LogError("出たよ！");
            mat.SetColor("_EmissiveColor", Color.green);
            isPutTrap = true;
        }
    }
}
