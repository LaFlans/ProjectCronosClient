using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Cysharp.Threading.Tasks;
using Cinemachine;

namespace ProjectCronos
{
    public class ItemShop : MonoBehaviour
    {
        /// <summary>
        /// アイテムショップのID(ユニーク)
        /// FIXME: マスタで設定するようにする
        /// </summary>
        [SerializeField]
        int itemShopId;

        /// <summary>
        /// 自由視点カメラ
        /// </summary>
        [SerializeField]
        CinemachineFreeLook shopCamera;

        bool isEnterShop = false;

        public async UniTask<bool> Initialize()
        {
            return true;
        }

        void OnEnterShop(InputAction.CallbackContext context)
        {
            var scenarioView = GameObject.Find("ScenarioView");
            if (scenarioView != null)
            {
                shopCamera.Priority = 20;
                SoundManager.Instance.Play("Button47");
                isEnterShop = !isEnterShop;

                scenarioView.GetComponent<ScenarioView>().ShowScenario(
                "001",
                () =>
                {
                    var obj = PopupManager.Instance.GetPopupObject(
                        EnumCollection.Popup.POPUP_TYPE.SHOP);

                    if (obj != null)
                    {
                        obj.GetComponent<ShopPopup>().Apply(() => { shopCamera.Priority = 0; });
                    }
                });
            }
        }

        private void OnDestroy()
        {
            // 念のため入力イベントを削除しておく
            InputManager.Instance.inputActions.Player.Action.performed -= OnEnterShop;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                MainEntryPoint.guideView.ShowControlGuide(
                    "ショップ",
                    EnumCollection.Input.INPUT_GAMEPAD_BUTTON.B);
                InputManager.Instance.inputActions.Player.Action.performed += OnEnterShop;
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                MainEntryPoint.guideView.HideControlGuide();
                InputManager.Instance.inputActions.Player.Action.performed -= OnEnterShop;
            }
        }
    }
}
