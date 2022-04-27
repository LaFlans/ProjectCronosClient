using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

namespace ProjectCronos
{
    internal class InputManager :ISingleton<InputManager>
    {
        string playerInputPath = "Assets/Resources_moved/Prefabs/PlayerInput.prefab";
        PlayerInput pInput;

        public override async Task<bool> Initialize()
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
