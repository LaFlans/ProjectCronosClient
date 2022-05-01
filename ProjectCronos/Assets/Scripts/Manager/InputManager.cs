using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    internal class InputManager :ISingleton<InputManager>
    {
        EnumCollection.Input.INPUT_STATUS status;
        public InputActions inputActions;

        public override async UniTask<bool> Initialize()
        {
            // 最初は操作不能状態にする
            status = EnumCollection.Input.INPUT_STATUS.UNCONTROLLABLE;

            inputActions = new InputActions();
            inputActions.Enable();

            return true;
        }

        /// <summary>
        /// 操作状態を設定
        /// </summary>
        /// <param name="status">任意の操作状態</param>
        public void SetInputStatus(EnumCollection.Input.INPUT_STATUS status)
        {
            this.status = status;

            switch(status)
            {
                case EnumCollection.Input.INPUT_STATUS.UNCONTROLLABLE:
                    inputActions.Player.Disable();
                    inputActions.UI.Disable();
                    break;
                case EnumCollection.Input.INPUT_STATUS.PLAYER:
                    inputActions.Player.Enable();
                    break;
                case EnumCollection.Input.INPUT_STATUS.UI:
                    inputActions.Player.Disable();
                    break;
            }
        }

        /// <summary>
        /// 操作状態を取得
        /// </summary>
        /// <returns>現在の操作状態</returns>
        public EnumCollection.Input.INPUT_STATUS GetInputStatus()
        {
            return status;
        }
    }
}
