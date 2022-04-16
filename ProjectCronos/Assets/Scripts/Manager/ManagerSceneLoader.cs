using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

namespace ProjectCronos
{
    public class ManagerSceneLoader : MonoBehaviour
    {
        private static bool Loaded { get; set; }

        private void Awake()
        {
            var sceneName = "ManagerScene";
            if (!Loaded && !Utility.IsAlreadyLoadScene(sceneName))
            {
                Loaded = true;
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

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
