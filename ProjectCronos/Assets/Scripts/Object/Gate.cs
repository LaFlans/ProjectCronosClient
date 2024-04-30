using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    /// <summary>
    /// 門クラス
    /// </summary>
    public class Gate : MonoBehaviour
    {
        /// <summary>
        /// ロックされているかどうか
        /// trueの場合、門の開け閉めができない
        /// </summary>
        bool isLock;

        /// <summary>
        /// 門が空いているかどうか
        /// </summary>
        [SerializeField]
        bool isOpen;

        /// <summary>
        /// 門のアニメーター
        /// </summary>
        Animator anim;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            anim = GetComponent<Animator>();
            isLock = false;
        }

        /// <summary>
        /// 門の操作(開閉)を行う
        /// </summary>
        void OnOperateGate(InputAction.CallbackContext context)
        {
            if (isLock)
            {
                // ロックされていたら何もしない
                return;
            }

            isOpen = !isOpen;
            isLock = true;
            SoundManager.Instance.Play("Button47");
            Debug.Log("門" + (isOpen ? "開けます！" : "閉めます！"));

            if (isOpen)
            {
                anim.SetTrigger("Open");
            }
            else
            {
                anim.SetTrigger("Close");
            }
        }

        void OnAnimationFinish()
        {
            if (isLock)
            {
                isLock = false;
            }
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                Debug.Log("門の開閉範囲に入ったよ");
                //saveAreaGuidText.SetActive(true);
                InputManager.Instance.inputActions.Player.Action.performed += OnOperateGate;
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                Debug.Log("門の開閉範囲を出たよ");
                //saveAreaGuidText.SetActive(false);
                InputManager.Instance.inputActions.Player.Action.performed -= OnOperateGate;
            }
        }
    }
}
