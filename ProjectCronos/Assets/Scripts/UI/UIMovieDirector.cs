using UnityEngine;

namespace ProjectCronos
{
    public class UIMovieDirector : MonoBehaviour
    {
        /// <summary>
        /// ムービー再生時に非表示になるUIオブジェクト
        /// </summary>
        [SerializeField]
        GameObject[] hiddenObjcts;

        /// <summary>
        /// 黒帯アニメーション
        /// </summary>
        [SerializeField]
        Animator blackBeltAnim;

        public void StartMovie()
        {
            SetActiveUIObjects(false);
            blackBeltAnim.SetTrigger("Appear");
        }

        public void EndMovie()
        {
            SetActiveUIObjects(true);
            blackBeltAnim.SetTrigger("Hidden");
        }

        void SetActiveUIObjects(bool isShow)
        {
            foreach (GameObject obj in hiddenObjcts)
            {
                obj.SetActive(isShow);
            }
        }
    }
}
