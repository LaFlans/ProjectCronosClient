using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// InputSystem入力テスト
/// </summary>
namespace ProjectCronos
{
    public class InputSystemTest : MonoBehaviour
    {
        [SerializeField]
        bool isShowGUI;

        [SerializeField]
        bool isShowKeyboard;

        [SerializeField]
        bool isShowMouse;

        [SerializeField]
        bool isShowController;

        [SerializeField, Range(0, 100)]
        int testVal;

        void Update()
        {
            if (isShowKeyboard)
            {
                KeyboardInputTest();
            }
        }

        /// <summary>
        /// キーボード入力テスト
        /// </summary>
        void KeyboardInputTest()
        {
            // A~Zキー
            for (char str = 'A'; str <= 'Z'; ++str)
            {
                // 押された1フレームだけ判定
                if (((KeyControl)Keyboard.current[str.ToString()]).wasPressedThisFrame)
                {

                    Debug.Log(str + "キーが押されたよ！");
                }
            }

            // 数字キー
            for (int i = 0; i <= 9; i++)
            {
                // 押された1フレームだけ判定
                if (((KeyControl)Keyboard.current[i.ToString()]).wasPressedThisFrame)
                {

                    Debug.Log(i + "キーが押されたよ！");
                }
            }
        }

        /// <summary>
        /// GUI
        /// </summary>
        void OnGUI()
        {
            if (!isShowGUI) return;
            if (Gamepad.current == null) return;

            GUI.color = Color.green;

            // マウス
            if (isShowMouse)
            {
                GUILayout.Label($"mouseLeft: {Mouse.current.leftButton.isPressed}");
                GUILayout.FlexibleSpace();
                GUILayout.Label($"mouseRight: {Mouse.current.rightButton.isPressed}");
                GUILayout.FlexibleSpace();
                GUILayout.Label($"mouseMiddle: {Mouse.current.middleButton.isPressed}");
            }

            // コントローラー
            if (isShowController)
            {
                GUILayout.Label($"DPad: {Gamepad.current.dpad.ReadValue()}");
                GUILayout.Label($"leftStick: {Gamepad.current.leftStick.ReadValue()}");
                GUILayout.Label($"buttonLeftStick: {Gamepad.current.leftStickButton.wasPressedThisFrame}");
                GUILayout.Label($"rightStick: {Gamepad.current.rightStick.ReadValue()}");
                GUILayout.Label($"buttonRightStick: {Gamepad.current.rightStickButton.wasPressedThisFrame}");
                GUILayout.Label($"buttonNorth: {Gamepad.current.buttonNorth.isPressed}");
                GUILayout.Label($"buttonSouth: {Gamepad.current.buttonSouth.isPressed}");
                GUILayout.Label($"buttonEast: {Gamepad.current.buttonEast.isPressed}");
                GUILayout.Label($"buttonWest: {Gamepad.current.buttonWest.isPressed}");
                GUILayout.Label($"leftShoulder: {Gamepad.current.leftShoulder.ReadValue()}");
                GUILayout.Label($"leftTrigger: {Gamepad.current.leftTrigger.ReadValue()}");
                GUILayout.Label($"rightShoulder: {Gamepad.current.rightShoulder.ReadValue()}");
                GUILayout.Label($"rightTrigger: {Gamepad.current.rightTrigger.ReadValue()}");
                GUILayout.Label($"startShoulder: {Gamepad.current.startButton.isPressed}");
                GUILayout.Label($"selectTrigger: {Gamepad.current.selectButton.isPressed}");
            }
        }
    }
}
