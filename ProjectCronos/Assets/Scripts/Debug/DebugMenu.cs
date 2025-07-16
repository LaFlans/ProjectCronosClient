using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    public class DebugMenu : MonoBehaviour
    {
        /// <summary>
        /// デバックメニュー更新頻度
        /// </summary>
        const float updateSpan = 1f;
        bool isAutoUpdateView;
        float currentTime;

        [SerializeField]
        CanvasGroup parentCanvasGroup;

        [SerializeField]
        TextMeshProUGUI coinText;
        [SerializeField]
        TextMeshProUGUI playTimeText;
        [SerializeField]
        TextMeshProUGUI popupCountText;

        /// <summary>
        /// プレイヤーステータス参照
        /// </summary>
        PlayerStatus playerStatus;

        /// <summary>
        /// デバックメニューを表示するか
        /// </summary>
        bool isShow;

        [SerializeField]
        GameObject itemDebugCellParent;
        [SerializeField]
        ItemDebugCell itemDebugCellprefab;

        /// <summary>
        /// デバックメニュー表示切り替えボタン
        /// </summary>
        [SerializeField]
        Button debugMenuEnableButton;

        /// <summary>
        /// デバックメニューの親オブジェクト
        /// </summary>
        [SerializeField]
        Transform debugMenuParent;

        /// <summary>
        /// コイン関連のデバックメニュー
        /// </summary>
        [SerializeField]
        CoinDebugMenu coinDebugMenu;

        List<ItemDebugCell> itemDebugCells;

        void Start()
        {
            isShow = true;
            isAutoUpdateView = false;
            currentTime = 0f;
            parentCanvasGroup.alpha = isShow ? 1 : 0;
        }

        public void Initialize()
        {
            // プレイヤー取得、イベント登録
            playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
            playerStatus.ResisterStatusUpdateEvent(UpdateView);

            // ビュー更新
            UpdateView();

            // 自動ビュー更新をするようにする
            isAutoUpdateView = true;

            // アイテムデバック機能初期化
            InitializeItemDebugMenu();

            // 入力設定
            InputManager.Instance.inputActions.DebugActions.ShowDebugMenu.performed += OnShowDebugMenu;

            debugMenuEnableButton.onClick.AddListener(
                () =>
                {
                    // デバックメニューを開いた時にアイテム一覧の更新も行っておく
                    foreach (var item in MasterDataManager.DB.ItemDataTable.All)
                    {
                        UpdateItemDebugMenu(item.Id);
                    }

                    debugMenuParent.gameObject.SetActive(!debugMenuParent.gameObject.activeSelf);
                });

            coinDebugMenu.Initialize();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        void Update()
        {
            if (isAutoUpdateView)
            {
                currentTime += Time.unscaledDeltaTime;
                if (currentTime > updateSpan)
                {
                    UpdateView();
                    currentTime = 0f;
                }
            }
        }

        public void ShowTestSystemPopup()
        {
            PopupManager.Instance.ShowSystemPopup(new PopupBase.MessageParam("確認", "これはテスト用ポップアップです。", "はい"));
        }

        private void OnDestroy()
        {
            playerStatus.UnresisterStatusUpdateEvent(UpdateView);
            InputManager.Instance.inputActions.DebugActions.ShowDebugMenu.performed -= OnShowDebugMenu;
        }

        void UpdateView()
        {
            if (playerStatus != null)
            {
                coinText.text = $"コイン数:{playerStatus.coinNum}";
                playTimeText.text = $"プレイ時間:{TimeManager.Instance.GetPlayTimeString()}";
                popupCountText.text = $"ポップアップの数:{PopupManager.Instance.GetPopupCount()}";
            }
        }

        void OnShowDebugMenu(InputAction.CallbackContext context)
        {
            Debug.Log("デバックメニュー表示切り替え");
            isShow = !isShow;
            parentCanvasGroup.alpha = isShow ? 1 : 0;
        }

        /// <summary>
        /// アイテム関連デバック機能初期化
        /// </summary>
        void InitializeItemDebugMenu()
        {
            itemDebugCells = new List<ItemDebugCell>();
            var items = MasterDataManager.DB.ItemDataTable.All;
            foreach (var item in items)
            {
                var obj = Instantiate(itemDebugCellprefab, itemDebugCellParent.transform);
                itemDebugCells.Add(obj);
                if (obj != null)
                {
                    var count = playerStatus.itemHolder.GetHoldItemCount(item.Id);
                    obj.GetComponent<ItemDebugCell>().Initialize(
                        item.Id,
                        item.Name,
                        count,
                        () =>
                        {
                            SoundManager.Instance.Play("Button47");
                            playerStatus.itemHolder.AddItem(item.Id, 1);
                            UpdateItemDebugMenu(item.Id);
                        },
                        () =>
                        {
                            SoundManager.Instance.Play("Button30");
                            playerStatus.itemHolder.ConsumeItem(item.Id, 1);
                            UpdateItemDebugMenu(item.Id);
                        });
                }
            }
        }

        /// <summary>
        /// アイテム関連のデバック機能更新
        /// </summary>
        void UpdateItemDebugMenu(int updateItemId)
        {
            itemDebugCells[updateItemId].GetComponent<ItemDebugCell>().UpdateView(playerStatus.itemHolder.GetHoldItemCount(updateItemId));
        }
    }
}
