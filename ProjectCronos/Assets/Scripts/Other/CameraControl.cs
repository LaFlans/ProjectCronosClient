using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    Vector2 cameraRatationInput;

    private void Start()
    {
        cameraRatationInput = Vector2.zero;
    }

    void Look(Vector2 input)
    {
        cameraRatationInput = input;
    }

    void OnLook(InputValue value)
    {
        Look(value.Get<Vector2>());
    }
}
