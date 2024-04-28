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
            if (!isLock)
            {
                isSwitchEnable = !isSwitchEnable;
                isLock = true;
                SoundManager.Instance.Play("Button47");
                Debug.Log("スイッチ" + (isSwitchEnable ? "オン！" : "オフ！"));

                if (isSwitchEnable)
                {
                    onSwitchEnableAction?.Invoke();
                    anim.SetTrigger("SwitchOn");
                }
                else
                {
                    onSwitchDisableAction?.Invoke();
                    anim.SetTrigger("SwitchOff");
                }
            }
            else
            {
                Debug.Log("ロックされているようだ");
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
                Debug.Log("レバー切り替え範囲に入ったよ");
                //saveAreaGuidText.SetActive(true);
                InputManager.Instance.inputActions.Player.Action.performed += OnSwitch;
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                Debug.Log("レバー切り替え範囲を出たよ");
                //saveAreaGuidText.SetActive(false);
                InputManager.Instance.inputActions.Player.Action.performed -= OnSwitch;
            }
        }
    }
}
