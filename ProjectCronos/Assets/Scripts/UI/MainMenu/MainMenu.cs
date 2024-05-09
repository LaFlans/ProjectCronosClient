using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    /// <summary>
    /// メインメニュークラス
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// 現在選択されているカテゴリー
        /// </summary>
        EnumCollection.UI.MAIN_MENU_CATEGORY currentCategory;

        /// <summary>
        /// コンテンツビュー
        /// </summary>
        [SerializeField]
        MainMenuCategoryContentView contentView;

        /// <summary>
        /// タブビュー
        /// </summary>
        [SerializeField]
        MainMenuCategoryTabView tabView;

        /// <summary>
        /// 事前読み込み
        /// マネージャー系生成後に呼ばれる
        /// </summary>
        /// <returns>UniTask</returns>
        public async UniTask PreLoadAsync()
        {
            // ここで事前に必要な素材を読み込む
            contentView.PreLoadAsync();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            currentCategory = EnumCollection.UI.MAIN_MENU_CATEGORY.ITEM;
            tabView.Initialize();
            contentView.Initialize();

            UpdateView();

            // 入力イベントを登録
            RegisterInputActions();
        }

        /// <summary>
        /// ビューの更新
        /// </summary>
        void UpdateView()
        {
            tabView.UpdateView(currentCategory);
            contentView.UpdateView(currentCategory);
        }

        /// <summary>
        /// 左にタブを切り替える選択
        /// </summary>
        /// <param name="context"></param>
        void OnToggleTabLeft(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Button47");
            currentCategory--;
            if (currentCategory < 0)
            {
                currentCategory = EnumCollection.UI.MAIN_MENU_CATEGORY.MAXMUM - 1;
            }

            UpdateView();
        }

        /// <summary>
        /// 右にタブを切り替える選択
        /// </summary>
        /// <param name="context"></param>
        void OnToggleTabRight(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Button47");
            currentCategory++;
            if (currentCategory == EnumCollection.UI.MAIN_MENU_CATEGORY.MAXMUM)
            {
                currentCategory = 0;
            }

            UpdateView();
        }

        void RegisterInputActions()
        {
            contentView.RegisterInputActions();
            InputManager.Instance.inputActions.MainMenu.RightBumper.performed += OnToggleTabRight;
            InputManager.Instance.inputActions.MainMenu.LeftBumper.performed += OnToggleTabLeft;
            InputManager.Instance.inputActions.MainMenu.Close.performed += OnCloseMenu;
        }

        void UnregisterInputActions()
        {
            contentView.UnregisterInputActions();
            InputManager.Instance.inputActions.MainMenu.RightBumper.performed -= OnToggleTabRight;
            InputManager.Instance.inputActions.MainMenu.LeftBumper.performed -= OnToggleTabLeft;
            InputManager.Instance.inputActions.MainMenu.Close.performed -= OnCloseMenu;
        }

        private void OnDestroy()
        {
            UnregisterInputActions();
        }

        void OnCloseMenu(InputAction.CallbackContext context)
        {
            UnregisterInputActions();

            this.gameObject.SetActive(false);
            InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER);
            TimeManager.Instance.ApplyTimeScale();
        }
    }
}
