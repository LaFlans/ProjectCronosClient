using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace ProjectCronos
{
    /// <summary>
    /// レバー
    /// </summary>
    public class Lever : MonoBehaviour
    {
        [SerializeField]
        UnityEvent onSwitchEnableAction;

        [SerializeField]
        UnityEvent onSwitchDisableAction;

        /// <summary>
        /// ロックされているかどうか
        /// trueの場合、オンオフの切り替えができない
        /// </summary>
        bool isLock;

        /// <summary>
        /// レバーのオンオフ状態
        /// </summary>
        bool isSwitchEnable;

        /// <summary>
        /// レバーアニメーター
        /// </summary>
        Animator anim;

        GameObject guideView;

        void Start()
        {
            isLock = false;
            isSwitchEnable = false;

            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
        }

        void OnSwitch(InputAction.CallbackContext context)
        {
            if (isLock)
            {
                // ロックされていたら何もしない
                Debug.Log("ロックされているようだ");
                return;
            }

            isSwitchEnable = !isSwitchEnable;
            isLock = true;
            SoundManager.Instance.Play("Button47");
            Debug.Log("スイッチ" + (isSwitchEnable ? "オン！" : "オフ！"));

            if (isSwitchEnable)
            {
                anim.SetTrigger("SwitchOn");
            }
            else
            {
                anim.SetTrigger("SwitchOff");
            }
        }

        void OnAnimationFinish()
        {
            if (isLock)
            {
                // レバーアニメーションが終わったタイミングで登録されているアクションを行う
                if (isSwitchEnable)
                {
                    onSwitchEnableAction?.Invoke();
                }
                else
                {
                    onSwitchDisableAction?.Invoke();
                }

                isLock = false;
            }
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                MainEntryPoint.guideView.ShowControlGuide(
                    "レバーを操作する",
                    EnumCollection.Input.INPUT_GAMEPAD_BUTTON.B);

                InputManager.Instance.inputActions.Player.Action.performed += OnSwitch;
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                MainEntryPoint.guideView.HideControlGuide();
                InputManager.Instance.inputActions.Player.Action.performed -= OnSwitch;
            }
        }
    }
}
