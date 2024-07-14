using UnityEngine;

namespace ProjectCronos
{
    public class PopupView : MonoBehaviour
    {
        /// <summary>
        /// ポップアップを開いた時に非表示になるUIオブジェクト
        /// </summary>
        [SerializeField]
        GameObject[] hiddenObjcts;

        public void SetActiveUIObjects(bool isShow)
        {
            foreach (GameObject obj in hiddenObjcts)
            {
                obj.SetActive(isShow);
            }
        }
    }
}
