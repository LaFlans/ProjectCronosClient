using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    internal class InputManager :ISingleton<InputManager>
    {
        string playerInputPath = "Assets/Resources_moved/Prefabs/PlayerInput.prefab";
        PlayerInput pInput;

        public override async UniTask<bool> Initialize()
        {
            //AddressableManager.Instance.Load(
            //    playerInputPath,
            //    () =>
            //    {
            //        //AddressableManager.Instance.InstantiateLoadedObject(playerInputPath, this.transform);
            //    });

            return true;
        }
    }
}
