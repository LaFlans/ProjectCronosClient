using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// メインメニューのタブ表示クラス
    /// </summary>
    public class MainMenuCategoryTabView : MonoBehaviour
    {
        /// <summary>
        /// アイテムタブ
        /// </summary>
        [SerializeField]
        MainMenuCategoryTab itemTab;

        /// <summary>
        /// スキルタブ
        /// </summary>
        [SerializeField]
        MainMenuCategoryTab skillTab;

        /// <summary>
        /// マップタブ
        /// </summary>
        [SerializeField]
        MainMenuCategoryTab MapTab;

        /// <summary>
        /// 設定タブ
        /// </summary>
        [SerializeField]
        MainMenuCategoryTab SettingTab;

        public void Initialize()
        {
            itemTab.Initialize();
            skillTab.Initialize();
            MapTab.Initialize();
            SettingTab.Initialize();
        }

        public void UpdateView(EnumCollection.UI.MAIN_MENU_CATEGORY currentCategory)
        {
            itemTab.UpdateSelected(currentCategory == EnumCollection.UI.MAIN_MENU_CATEGORY.ITEM);
            skillTab.UpdateSelected(currentCategory == EnumCollection.UI.MAIN_MENU_CATEGORY.SKILL);
            MapTab.UpdateSelected(currentCategory == EnumCollection.UI.MAIN_MENU_CATEGORY.MAP);
            SettingTab.UpdateSelected(currentCategory == EnumCollection.UI.MAIN_MENU_CATEGORY.SETTING);
        }
    }
}
