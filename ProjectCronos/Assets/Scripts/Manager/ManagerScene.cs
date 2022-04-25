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
            // AddressableManagerとMasterDataManagerは最優先で生成
            this.gameObject.AddComponent<AddressableManager>();
            this.gameObject.AddComponent<MasterDataManager>();

            // 常に存在するべき物を生成
            new GameObject("InputManager").AddComponent<InputManager>();
            new GameObject("SoundManager").AddComponent<SoundManager>();
            this.gameObject.AddComponent<PopupManager>();
#if UNITY_EDITOR
            this.gameObject.AddComponent<ProductDebug>();
#endif
        }
    }
}
