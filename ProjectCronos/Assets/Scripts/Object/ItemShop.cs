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
            // 既にショップに入っている場合、何もしない
            if (isEnterShop)
            {
                return;
            }

            var scenarioView = GameObject.Find("ScenarioView");
            if (scenarioView != null)
            {
                shopCamera.Priority = 20;
                SoundManager.Instance.Play("Button47");
                isEnterShop = true;

                scenarioView.GetComponent<ScenarioView>().ShowScenario(
                "001",
                (customKey) =>
                {
                    if (customKey == 0)
                    {
                        // 0の場合、何もせずに会話終了
                        shopCamera.Priority = 0;
                        isEnterShop = false;
                    }
                    else
                    {
                        // 0出ない場合、ショップを表示する
                        var obj = PopupManager.Instance.GetPopupObject(
                            EnumCollection.Popup.POPUP_TYPE.SHOP);

                        if (obj != null)
                        {
                            obj.GetComponent<ShopPopup>().Apply(
                                () =>
                                {
                                    scenarioView.GetComponent<ScenarioView>().ShowScenario(
                                        "002",
                                        (customKey) =>
                                        {
                                            shopCamera.Priority = 0;
                                            isEnterShop = false;
                                        });
                                });
                        }
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
