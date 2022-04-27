using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ProjectCronos
{
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// シーンロード
        /// </summary>
        /// <param name="sceneName">ロードしたいシーン名</param>
        public static void LoadScene(string sceneName)
        {
            if (!Utility.IsAlreadyLoadScene(sceneName))
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

        /// <summary>
        /// シーンアンロード
        /// </summary>
        /// <param name="sceneName">アンロードしたいシーン名</param>
        public static void UnloadScene(string sceneName)
        {
            if (Utility.IsAlreadyLoadScene(sceneName))
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }
}
