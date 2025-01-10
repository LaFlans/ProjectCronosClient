using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    /// <summary>
    /// 宝箱クラス
    /// </summary>
    public class TresureBox : MonoBehaviour
    {
        /// <summary>
        /// 当たり判定
        /// </summary>
        Collider col;

        bool isOpen;
        [SerializeField] Animator anim;
        [SerializeField] int itemId;
        [SerializeField] int itemAmount;
        [SerializeField] DropItem dropItem;
        void Start()
        {
            col = GetComponent<Collider>();
            isOpen = false;
            anim.SetTrigger(isOpen ? "Open" : "Close");
        }

        void Open(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("OpenTresureBox");
            MainEntryPoint.guideView.HideControlGuide();
            InputManager.Instance.inputActions.Player.Action.performed -= Open;

            col.enabled = false;
            dropItem.Init(itemId, itemAmount);
            anim.SetTrigger("Open");
            isOpen = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!isOpen && other.gameObject.tag == "Player")
            {
                MainEntryPoint.guideView.ShowControlGuide(
                    "宝箱を開く",
                    EnumCollection.Input.INPUT_GAMEPAD_BUTTON.B);
                InputManager.Instance.inputActions.Player.Action.performed += Open;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!isOpen && other.gameObject.tag == "Player")
            {
                MainEntryPoint.guideView.HideControlGuide();
                InputManager.Instance.inputActions.Player.Action.performed -= Open;
            }
        }
    }
}
