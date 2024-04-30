using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// メインメニューのタブクラス
    /// </summary>
    public class MainMenuCategoryTab : MonoBehaviour
    {
        /// <summary>
        /// 表示切替トグルオブジェクト
        /// </summary>
        [SerializeField]
        ToggleObject toggleObj;

        /// <summary>
        /// 選択状態かどうか
        /// </summary>
        bool isSelected = false;

        public void Initialize()
        {
            isSelected = false;
            toggleObj.SetToggle(isSelected);
        }

        public void UpdateSelected(bool isSelect)
        {
            this.isSelected = isSelect;
            toggleObj.SetToggle(isSelected);
        }
    }
}
