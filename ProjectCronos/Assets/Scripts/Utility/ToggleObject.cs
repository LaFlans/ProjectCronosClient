using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// ２つのオブジェクトの表示を切り替える便利クラス
    /// </summary>
    public class ToggleObject : MonoBehaviour
    {
        [SerializeField]
        GameObject onObj;

        [SerializeField]
        GameObject offObj;

        [SerializeField]
        bool isOn;

        void Start()
        {
            UpdateToggle();
        }

        /// <summary>
        /// トグルの状態を設定
        /// </summary>
        /// <param name="toggleStatus"></param>
        public void SetToggle(bool toggleStatus)
        {
            isOn = toggleStatus;
            UpdateToggle();
        }


        /// <summary>
        /// トグルの状態をスイッチ
        /// </summary>
        public void SwitchToggle()
        {
            onObj.SetActive(!onObj.activeSelf);
            offObj.SetActive(!offObj.activeSelf);
        }

        /// <summary>
        /// トグルの状態を更新
        /// </summary>
        void UpdateToggle()
        {
            onObj.SetActive(isOn);
            offObj.SetActive(!isOn);
        }
    }
}
