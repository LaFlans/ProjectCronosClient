using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

namespace ProjectCronos
{
    public class ManagerSceneLoader : MonoBehaviour
    {
        private static bool Loaded { get; set; }

        /// <summary>
        /// マネージャーシーンを読み込む
        /// 既に読み込まれている場合は特に何もしない
        /// </summary>
        public static void LoadManagerScene()
        {
            var sceneName = "ManagerScene";
            if (!Loaded && !Utility.IsAlreadyLoadScene(sceneName))
            {
                Loaded = true;
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }
    }
}
