using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    internal class InputManager :ISingleton<InputManager>
    {
        EnumCollection.Input.INPUT_STATUS status;
        public InputActions inputActions;

        // 入力切替時のイベント
        UnityEvent SwitchInputStatusEvent;

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>UniTask</returns>
        public override async UniTask<bool> Initialize()
        {
            // 最初は操作不能状態にする
            status = EnumCollection.Input.INPUT_STATUS.UNCONTROLLABLE;

            inputActions = new InputActions();
            inputActions.Enable();

            if (SwitchInputStatusEvent == null)
            {
                SwitchInputStatusEvent = new UnityEvent();
            }

            Debug.Log("InputManager初期化");

            return true;
        }

        /// <summary>
        /// 操作状態を設定
        /// </summary>
        /// <param name="status">任意の操作状態</param>
        public void SetInputStatus(EnumCollection.Input.INPUT_STATUS status)
        {
            this.status = status;

            switch (status)
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

            // 入力状態切り替えイベントが登録されていたら発火
            if (SwitchInputStatusEvent != null)
            {
                SwitchInputStatusEvent.Invoke();
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

        /// <summary>
        /// 指定の状態と一致するか
        /// </summary>
        /// <param name="status">指定の状態</param>
        /// <returns>一致する場合、trueを返す</returns>
        public bool IsMatchInputStatus(EnumCollection.Input.INPUT_STATUS status)
        {
            return this.status == status;
        }

        /// <summary>
        /// 入力状態が切り替わった際のイベント登録
        /// 現在は戻り値は対応していません
        /// </summary>
        /// <param name="action">登録したいイベント</param>
        public void RegistSwitchInputStatusEvent(Action action)
        {
            SwitchInputStatusEvent.AddListener(() => { action(); });
        }
    }
}
