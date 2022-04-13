using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    class ApplicationQuit : MonoBehaviour
    {
        bool isShowQuitPopup;

        void Start()
        {
            isShowQuitPopup = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Gamepad.current.startButton.wasPressedThisFrame)
            {
                if (!isShowQuitPopup)
                {
                    isShowQuitPopup = true;
                    var obj = PopupManager.Instance.GetPopupObject(
                        EnumCollection.Popup.POPUP_TYPE.QUIT_APPLICATION);

                    obj.GetComponent<PopupBase>().Setup(() => { isShowQuitPopup = false; });
                }
            }
        }
    }
}