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

        /// <summary>
        /// 指定のシーンに遷移
        /// </summary>
        /// <param name="scene">遷移したいシーン</param>
        public static void TransitionScene(EnumCollection.Scene.SCENE_TYPE scene)
        {
            var loadingScene = EnumExtension.GetDescriptionFromValue(EnumCollection.Scene.SCENE_TYPE.LOADING);
            var currentScene = EnumExtension.GetDescriptionFromValue(ManagerScene.GetCurrentScene());
            var nextScene = EnumExtension.GetDescriptionFromValue(scene);

            Debug.Log($"{currentScene}から{nextScene}に遷移するよ！");

            // ローディングシーンを読み込む
            LoadScene(loadingScene);

            // 遷移先のシーンを読み込む
            LoadScene(nextScene);

            // 現在のシーンをアンロード
            UnloadScene(currentScene);

            // ローディングシーンをアンロード
            UnloadScene(loadingScene);
        }
    }
}
