using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    public class SaveArea : MonoBehaviour
    {
        [SerializeField]
        GameObject savePopup;

        bool canSave;
        bool isActive;

        [SerializeField]
        Transform respawnTransform;

        /// <summary>
        /// セーブポイントのID(ユニーク)
        /// FIXME: マスタで設定するようにする
        /// </summary>
        [SerializeField]
        int saveAreaId;

        /// <summary>
        /// セーブポイント名
        /// </summary>
        string savePointName;

        void Start()
        {
            canSave = false;
            isActive = false;
        }

        public async UniTask<bool> Initialize()
        {
            isActive = true;

            return true;
        }

        void OnSave(InputAction.CallbackContext context)
        {
            if (isActive && canSave)
            {
                //Debug.Log("Save！！");
                //savePopup.GetComponent<SavePopup>().Apply(new SaveAreaInfo(saveAreaId, respawnTransform));
                //savePopup.SetActive(true);

                var obj = PopupManager.Instance.GetPopupObject(
                    EnumCollection.Popup.POPUP_TYPE.SAVE);

                if (obj != null)
                {
                    obj.GetComponent<SavePopup>().Apply(
                        new SaveAreaInfo(
                            saveAreaId,
                            respawnTransform.position,
                            respawnTransform.rotation));
                }
            }
        }

        private void OnDestroy()
        {
            // 念のため入力イベントを削除しておく
            InputManager.Instance.inputActions.Player.Action.performed -= OnSave;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                if (!isActive)
                {
                    // 非アクティブ中は何もしない
                    return;
                }

                MainEntryPoint.guideView.ShowControlGuide(
                    "セーブする？",
                    EnumCollection.Input.INPUT_GAMEPAD_BUTTON.B);
                canSave = true;
                InputManager.Instance.inputActions.Player.Action.performed += OnSave;
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                if (!isActive)
                {
                    // 非アクティブ中は何もしない
                    return;
                }

                //Debug.Log("SaveAreaを出たよ");
                MainEntryPoint.guideView.HideControlGuide();
                canSave = false;
                InputManager.Instance.inputActions.Player.Action.performed -= OnSave;
            }
        }
    }
}
