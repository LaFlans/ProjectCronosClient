using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// InputSystem���̓e�X�g
/// </summary>
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

    [SerializeField,Range(0,100)]
    int testVal;

    void Update()
    {
        if (isShowKeyboard)
        {
            KeyboardInputTest();
        }
    }

    /// <summary>
    /// �L�[�{�[�h���̓e�X�g
    /// </summary>
    void KeyboardInputTest()
    {
        // A~Z�L�[
        for (char str = 'A'; str <= 'Z'; ++str)
        {
            // �����ꂽ1�t���[����������
            if (((KeyControl)Keyboard.current[str.ToString()]).wasPressedThisFrame)
            {

                Debug.Log(str + "�L�[�������ꂽ��I");
            }
        }

        // �����L�[
        for (int i = 0;i <= 9;i++)
        {
            // �����ꂽ1�t���[����������
            if (((KeyControl)Keyboard.current[i.ToString()]).wasPressedThisFrame)
            {

                Debug.Log(i + "�L�[�������ꂽ��I");
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

        // �}�E�X
        if (isShowMouse)
        {
            GUILayout.Label($"mouseLeft: {Mouse.current.leftButton.isPressed}");
            GUILayout.FlexibleSpace();
            GUILayout.Label($"mouseRight: {Mouse.current.rightButton.isPressed}");
            GUILayout.FlexibleSpace();
            GUILayout.Label($"mouseMiddle: {Mouse.current.middleButton.isPressed}");
        }

        // �R���g���[���[
        if (isShowController)
        {
            GUILayout.Label($"DPad: {Gamepad.current.dpad.ReadValue()}");
            GUILayout.Label($"leftStick: {Gamepad.current.leftStick.ReadValue()}");
            GUILayout.Label($"rightStick: {Gamepad.current.rightStick.ReadValue()}");
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
