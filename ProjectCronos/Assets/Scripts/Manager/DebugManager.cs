using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    /// <summary>
    /// デバック機能管理クラス
    /// </summary>
    class DebugManager : Singleton<DebugManager>
    {
        bool isUseGraphy = true;

        GameObject graphy;
        string path = "Assets/Resources_moved/Prefabs/Graphy.prefab";

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        public override async UniTask<bool> Initialize()
        {
            if (isUseGraphy)
            {
                await AddressableManager.instance.LoadInstance(path,
                    (obj) =>
                    {
                        graphy = obj;
                    });
            }

            Debug.Log("DebugManager初期化");

            InputManager.Instance.inputActions.Debug.ShowGraphy.performed += OnShowGraphy;


            return true;
        }

        void Update()
        {
            if (InputManager.Instance.inputActions.Debug.ShowGraphy.WasPressedThisFrame())
            {
                Debug.Log("Graphy表示切り替え(Update)");
                graphy.SetActive(!graphy.activeSelf);
            }
        }

        void OnShowGraphy(InputAction.CallbackContext context)
        {
            Debug.Log("Graphy表示切り替え");
            graphy.SetActive(!graphy.activeSelf);
        }
    }
}
