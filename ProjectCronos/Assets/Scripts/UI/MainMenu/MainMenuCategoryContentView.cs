using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// メインメニューのコンテンツビュークラス
    /// </summary>
    public class MainMenuCategoryContentView : MonoBehaviour
    {
        /// <summary>
        /// アイテムビュー
        /// </summary>
        [SerializeField]
        MainMenuItemView itemView;

        /// <summary>
        /// スキルビュー
        /// </summary>
        [SerializeField]
        MainMenuSkillView skillView;

        /// <summary>
        /// マップビュー
        /// </summary>
        [SerializeField]
        MainMenuMapView mapView;

        /// <summary>
        /// 設定ビュー
        /// </summary>
        [SerializeField]
        MainMenuSettingView settingView;

        /// <summary>
        /// 事前読み込み
        /// マネージャー系生成後に呼ばれる
        /// </summary>
        /// <returns>UniTask</returns>
        public async UniTask PreLoadAsync()
        {
            // ここで事前に必要な素材を読み込む
            await itemView.PreLoadAsync();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            itemView.gameObject.SetActive(false);
            skillView.gameObject.SetActive(false);
            mapView.gameObject.SetActive(false);
            settingView.gameObject.SetActive(false);

            itemView.Initialize();
        }

        /// <summary>
        /// ビューの更新
        /// </summary>
        public void UpdateView(EnumCollection.UI.MAIN_MENU_CATEGORY currentCategory)
        {
            itemView.gameObject.SetActive(currentCategory == EnumCollection.UI.MAIN_MENU_CATEGORY.ITEM);
            skillView.gameObject.SetActive(currentCategory == EnumCollection.UI.MAIN_MENU_CATEGORY.SKILL);
            mapView.gameObject.SetActive(currentCategory == EnumCollection.UI.MAIN_MENU_CATEGORY.MAP);
            settingView.gameObject.SetActive(currentCategory == EnumCollection.UI.MAIN_MENU_CATEGORY.SETTING);
        }

        public void RegisterInputActions()
        {
            Debug.Log("コンテンツ画面の入力登録");
            itemView.RegisterInputActions();
        }

        public void UnregisterInputActions()
        {
            Debug.Log("コンテンツ画面の入力解除");
            itemView.UnregisterInputActions();
        }

        /// <summary>
        /// メニューを閉じることができるか
        /// </summary>
        /// <returns></returns>
        public bool IsCloseMenu()
        {
            if (itemView.IsCloseMenu())
            {
                return true;
            }

            return false;
        }
    }
}
