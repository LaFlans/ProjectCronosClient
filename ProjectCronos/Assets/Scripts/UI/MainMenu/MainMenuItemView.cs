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
        /// 現在のアイテムメニュー操作状態
        /// </summary>
        EnumCollection.Item.MENU_ITEM_CONTROL_STATUS currentItemMenuControlStatus;

        ItemHolder playerItemHolder;

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

            // カテゴリー選択前はアイテム一覧と詳細は非表示
            itemListView.gameObject.SetActive(false);
            itemDetailView.gameObject.SetActive(false);

            currentItemCategory = EnumCollection.Item.ITEM_CATEGORY.NORMAL;
            currentItemMenuControlStatus = EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.CATEGORY;
            UpdateItemCategoryView();

            // プレイヤーのアイテム情報を取得
            playerItemHolder = GameObject.FindGameObjectWithTag("Player").
                GetComponent<PlayerStatus>().itemHolder;
        }

        void OnSubmit(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Button30");

            switch (currentItemMenuControlStatus)
            {
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.CATEGORY:
                    itemListView.Initialize(playerItemHolder);
                    itemListView.gameObject.SetActive(true);
                    itemDetailView.gameObject.SetActive(true);
                    currentItemMenuControlStatus = EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.LIST;
                    UpdateItemDetailView();
                    break;
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.LIST:
                    break;
                default:
                    break;
            }
        }

        void OnUp(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Button47");

            switch (currentItemMenuControlStatus)
            {
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.CATEGORY:
                    currentItemCategory--;
                    if (currentItemCategory < 0)
                    {
                        currentItemCategory = EnumCollection.Item.ITEM_CATEGORY.MAXMUM - 1;
                    }

                    UpdateItemCategoryView();
                    break;
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.LIST:
                    itemListView.OnUp();
                    UpdateItemDetailView();
                    break;
                default:
                    break;
            }
        }

        void OnDown(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Button47");

            switch (currentItemMenuControlStatus)
            {
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.CATEGORY:
                    currentItemCategory++;
                    if (currentItemCategory == EnumCollection.Item.ITEM_CATEGORY.MAXMUM)
                    {
                        currentItemCategory = 0;
                    }

                    UpdateItemCategoryView();
                    break;
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.LIST:
                    itemListView.OnDown();
                    UpdateItemDetailView();
                    break;
                default:
                    break;
            }
        }

        void OnLeft(InputAction.CallbackContext context)
        {
            switch (currentItemMenuControlStatus)
            {
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.CATEGORY:
                    break;
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.LIST:
                    SoundManager.Instance.Play("Button47");
                    itemListView.OnLeft();
                    UpdateItemDetailView();
                    break;
                default:
                    break;
            }
        }

        void OnRight(InputAction.CallbackContext context)
        {
            switch (currentItemMenuControlStatus)
            {
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.CATEGORY:
                    break;
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.LIST:
                    SoundManager.Instance.Play("Button47");
                    itemListView.OnRight();
                    UpdateItemDetailView();
                    break;
                default:
                    break;
            }
        }

        public bool IsCloseMenu()
        {
            switch (currentItemMenuControlStatus)
            {
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.CATEGORY:
                    return true;
                case EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.LIST:
                    itemListView.gameObject.SetActive(false);
                    itemDetailView.gameObject.SetActive(false);
                    currentItemMenuControlStatus = EnumCollection.Item.MENU_ITEM_CONTROL_STATUS.CATEGORY;
                    return false;
                default:
                    break;
            }

            // 来ることはないと思うけど念のため
            return false;
        }

        void UpdateItemDetailView()
        {
            // アイテム詳細を更新
            var id = itemListView.GetSelectedItemId();
            var info = playerItemHolder.GetItemDetailInfo(id);
            itemDetailView.UpdateView(info, playerItemHolder.ownItems[id]);
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

        void RegisterItemCategoryInputActions()
        {
            InputManager.Instance.inputActions.MainMenu.Up.performed += OnUp;
            InputManager.Instance.inputActions.MainMenu.Down.performed += OnDown;
            InputManager.Instance.inputActions.MainMenu.Right.performed += OnRight;
            InputManager.Instance.inputActions.MainMenu.Left.performed += OnLeft;
            InputManager.Instance.inputActions.MainMenu.Submit.performed += OnSubmit;
        }

        void UnregisterItemCategoryInputActions()
        {
            InputManager.Instance.inputActions.MainMenu.Up.performed -= OnUp;
            InputManager.Instance.inputActions.MainMenu.Down.performed -= OnDown;
            InputManager.Instance.inputActions.MainMenu.Right.performed -= OnRight;
            InputManager.Instance.inputActions.MainMenu.Left.performed -= OnLeft;
            InputManager.Instance.inputActions.MainMenu.Submit.performed -= OnSubmit;
        }
    }
}
