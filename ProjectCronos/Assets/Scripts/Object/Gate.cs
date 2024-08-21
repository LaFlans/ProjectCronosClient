using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    /// <summary>
    /// 門クラス
    /// </summary>
    public class Gate : Gimmick
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
        /// 必要なアイテムのID
        /// </summary>
        [SerializeField]
        int needItemId;

        /// <summary>
        /// 門のアニメーター
        /// </summary>
        [SerializeField]
        Animator anim;

        public override void Initialize(EnumCollection.Stage.GIMMICK_STATUS status)
        {
            InitializeGimmickStatus(status);

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

            if (!isOpen)
            {
                switch(gimmickType)
                {
                    case EnumCollection.Stage.GIMMICK_TYPE.None:
                        isOpen = true;
                        anim.SetTrigger("Open");
                        SoundManager.Instance.Play("Button47");
                        MainEntryPoint.guideView.HideControlGuide();
                        InputManager.Instance.inputActions.Player.Action.performed -= OnOperateGate;
                        break;
                    case EnumCollection.Stage.GIMMICK_TYPE.Item:
                        var playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
                        if (playerStatus.itemHolder.ConsumeItem(needItemId, 1))
                        {
                            isOpen = true;
                            gimmickStatus = EnumCollection.Stage.GIMMICK_STATUS.TRIGGERED;
                            anim.SetTrigger("Open");
                            SoundManager.Instance.Play("Button47");


                            MainEntryPoint.guideView.HideControlGuide();
                            InputManager.Instance.inputActions.Player.Action.performed -= OnOperateGate;
                        }
                        else
                        {
                            Debug.Log("必要なアイテムを所持していません。");
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// ギミックの状態を初期化
        /// </summary>
        /// <param name="status">ギミックの状態</param>
        void InitializeGimmickStatus(EnumCollection.Stage.GIMMICK_STATUS status)
        {
            gimmickStatus = status;
            if (status == EnumCollection.Stage.GIMMICK_STATUS.TRIGGERED)
            {
                anim.SetTrigger("Open");
                isOpen = true;
            }
            else
            {
                anim.SetTrigger("Close");
                isOpen = false;
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
            if (!isOpen)
            {
                if (col.gameObject.tag == "Player")
                {
                    MainEntryPoint.guideView.ShowControlGuide(
                        "門を開く",
                        EnumCollection.Input.INPUT_GAMEPAD_BUTTON.B);
                    //saveAreaGuidText.SetActive(true);
                    InputManager.Instance.inputActions.Player.Action.performed += OnOperateGate;
                }
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (!isOpen)
            {
                if (col.gameObject.tag == "Player")
                {
                    MainEntryPoint.guideView.HideControlGuide();
                    //saveAreaGuidText.SetActive(false);
                    InputManager.Instance.inputActions.Player.Action.performed -= OnOperateGate;
                }
            }
        }
    }
}
