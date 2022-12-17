using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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

            // 入力設定
            InputManager.Instance.inputActions.Debug.ShowDebugMenu.performed += OnShowDebugMenu;
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

        private void OnDestroy()
        {
            playerStatus.UnresisterStatusUpdateEvent(UpdateView);
            InputManager.Instance.inputActions.Debug.ShowDebugMenu.performed -= OnShowDebugMenu;
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
    }
}
