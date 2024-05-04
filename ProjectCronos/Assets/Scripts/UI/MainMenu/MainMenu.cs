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

        /// <summary>
        /// 左選択
        /// </summary>
        /// <param name="context"></param>
        void OnLeft(InputAction.CallbackContext context)
        {
            //SoundManager.Instance.Play("Button47");
        }

        /// <summary>
        /// 右選択
        /// </summary>
        /// <param name="context"></param>
        void OnRight(InputAction.CallbackContext context)
        {
            //SoundManager.Instance.Play("Button47");
        }

        /// <summary>
        /// 決定処理
        /// </summary>
        /// <param name="context"></param>
        void OnSubmit(InputAction.CallbackContext context)
        {
            //SoundManager.Instance.Play("Button47");
        }

        void RegisterInputActions()
        {
            InputManager.Instance.inputActions.UI.Submit.performed += OnSubmit;
            InputManager.Instance.inputActions.UI.Left.performed += OnLeft;
            InputManager.Instance.inputActions.UI.Right.performed += OnRight;
            InputManager.Instance.inputActions.UI.RightBumper.performed += OnToggleTabRight;
            InputManager.Instance.inputActions.UI.LeftBumper.performed += OnToggleTabLeft;
        }

        void UnregisterInputActions()
        {
            InputManager.Instance.inputActions.UI.Submit.performed -= OnSubmit;
            InputManager.Instance.inputActions.UI.Left.performed -= OnLeft;
            InputManager.Instance.inputActions.UI.Right.performed -= OnRight;
            InputManager.Instance.inputActions.UI.RightBumper.performed -= OnToggleTabRight;
            InputManager.Instance.inputActions.UI.LeftBumper.performed -= OnToggleTabLeft;
        }
    }
}
