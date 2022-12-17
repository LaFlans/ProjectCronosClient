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
        GameObject graphy;
        string path = "Assets/Resources_moved/Prefabs/Graphy.prefab";

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        public override async UniTask<bool> Initialize()
        {
            await AddressableManager.Instance.LoadInstance(path,
                (obj) =>
                {
                    graphy = obj;
                });

            Debug.Log("DebugManager初期化");

            return true;
        }

        void Update()
        {
        }
    }
}
