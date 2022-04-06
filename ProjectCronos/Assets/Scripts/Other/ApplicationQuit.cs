using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace ProjectCronos
{
    public class ApplicationQuit : MonoBehaviour
    {
        void Update()
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                Debug.Log("スタートボタンを押したよ！");
                ProductSound.Instance.Hello();
            }
        }
    }
}