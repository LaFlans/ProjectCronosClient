using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    /// <summary>
    /// 宝箱クラス
    /// </summary>
    public class TresureBox : MonoBehaviour
    {
        bool isOpen;

        [SerializeField]
        Animator anim;

        [SerializeField]
        int itemId;
        [SerializeField]
        int itemAmount;

        private void Start()
        {
            isOpen = false;
            anim.SetTrigger(isOpen ? "Open" : "Close");
        }

        void Open(InputAction.CallbackContext context)
        {
            if (isOpen)
            {
                anim.SetTrigger("Close");
                isOpen = !isOpen;
            }
            else
            {
                Debug.Log("宝箱を開けました！");
                var playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
                if (playerStatus != null)
                {
                    Debug.Log("アイテムを追加しました！");
                    playerStatus.itemHolder.AddItem(itemId, itemAmount);
                }

                anim.SetTrigger("Open");
                isOpen = !isOpen;

                MainEntryPoint.guideView.HideControlGuide();
                InputManager.Instance.inputActions.Player.Action.performed -= Open;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (!isOpen)
            {
                MainEntryPoint.guideView.ShowControlGuide(
                    "宝箱を開く",
                    EnumCollection.Input.INPUT_GAMEPAD_BUTTON.B);
                InputManager.Instance.inputActions.Player.Action.performed += Open;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!isOpen)
            {
                MainEntryPoint.guideView.HideControlGuide();
                InputManager.Instance.inputActions.Player.Action.performed -= Open;
            }
        }
    }
}
