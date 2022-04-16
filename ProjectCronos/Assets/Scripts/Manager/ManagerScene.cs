using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System;

namespace ProjectCronos
{
    class ManagerScene : MonoBehaviour
    {
        void Awake()
        {
            FirstLaunch();
        }

        /// <summary>
        /// 起動時処理
        /// </summary>
        void FirstLaunch()
        {
            // AddressableManagerは最優先で生成
            this.gameObject.AddComponent<AddressableManager>();

            // 常に存在するべき物を生成
            new GameObject("InputManager").AddComponent<InputManager>();
            this.gameObject.AddComponent<SoundManager>();
            this.gameObject.AddComponent<PopupManager>();
            this.gameObject.AddComponent<MasterDataManager>();
#if UNITY_EDITOR
            this.gameObject.AddComponent<ProductDebug>();
#endif
        }
    }
}
