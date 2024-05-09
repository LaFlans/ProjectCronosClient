using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    /// <summary>
    /// メインメニューのアイテムビュークラス
    /// </summary>
    public class MainMenuItemView : MonoBehaviour
    {
        /// <summary>
        /// アイテムの種類一覧画面
        /// </summary>
        [SerializeField]
        ItemCategoryListView itemCategoryListView;

        /// <summary>
        /// アイテム一覧画面
        /// </summary>
        [SerializeField]
        ItemListView itemListView;

        /// <summary>
        /// アイテム詳細画面
        /// </summary>
        [SerializeField]
        ItemDetailView itemDetailView;

        /// <summary>
        /// 現在選択中のアイテムカテゴリー
        /// </summary>
        EnumCollection.Item.ITEM_CATEGORY currentItemCategory;

        /// <summary>
        /// 事前読み込み
        /// マネージャー系生成後に呼ばれる
        /// </summary>
        /// <returns>UniTask</returns>
        public async UniTask PreLoadAsync()
        {
            // ここで事前に必要な素材を読み込む

            // ここでアイテム画面に必要なリソースをすべて読み込んでおく

        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            itemCategoryListView.Initialize();
            itemListView.Initialize();

            currentItemCategory = EnumCollection.Item.ITEM_CATEGORY.NORMAL;
            UpdateItemCategoryView();
        }

        void RegisterItemCategoryInputActions()
        {
            InputManager.Instance.inputActions.MainMenu.Up.performed += OnUp;
            InputManager.Instance.inputActions.MainMenu.Down.performed += OnDown;
            InputManager.Instance.inputActions.MainMenu.Submit.performed += OnSubmit;
        }

        void UnregisterItemCategoryInputActions()
        {
            InputManager.Instance.inputActions.MainMenu.Up.performed -= OnUp;
            InputManager.Instance.inputActions.MainMenu.Down.performed -= OnDown;
            InputManager.Instance.inputActions.MainMenu.Submit.performed -= OnSubmit;
        }

        void OnSubmit(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Button30");
        }

        void OnUp(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Button47");

            currentItemCategory--;
            if (currentItemCategory < 0)
            {
                currentItemCategory = EnumCollection.Item.ITEM_CATEGORY.MAXMUM -1;
            }

            UpdateItemCategoryView();
        }

        void OnDown(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Button47");

            currentItemCategory++;
            if (currentItemCategory == EnumCollection.Item.ITEM_CATEGORY.MAXMUM)
            {
                currentItemCategory = 0;
            }

            UpdateItemCategoryView();
        }

        /// <summary>
        /// アイテムの種類画面の更新
        /// </summary>
        public void UpdateItemCategoryView()
        {
            itemCategoryListView.UpdateView(currentItemCategory);
        }

        public void RegisterInputActions()
        {
            Debug.Log("コンテンツ画面の入力登録");
            RegisterItemCategoryInputActions();
        }
        public void UnregisterInputActions()
        {
            Debug.Log("コンテンツ画面の入力解除");
            UnregisterItemCategoryInputActions();
        }
    }
}
