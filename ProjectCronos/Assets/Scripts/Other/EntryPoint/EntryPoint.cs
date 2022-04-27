using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// シーンのエントリーポイント
    /// </summary>
    public class EntryPoint : MonoBehaviour
    {
        EnumCollection.Scene.SCENE_LOAD_STATUS loadStatus;

        void Start()
        {
            loadStatus = EnumCollection.Scene.SCENE_LOAD_STATUS.WAITING;

            // 初期化処理
            StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            loadStatus = EnumCollection.Scene.SCENE_LOAD_STATUS.LOADING;

            // ローディングシーンを読み込む
            // FIXME: もっといい感じのロード処理にする(ローディングシーンは常に存在する形がよさそう？)
            SceneLoader.LoadScene("LoadingScene");

            // マネージャーシーンを読み込む
            SceneLoader.LoadScene("ManagerScene");

            // マネージャーシーンの読み込みが終わるまで待つ
            while (!ManagerScene.isLaunch)
            {
                yield return null;
            }

            SoundManager.Instance.Play("BGM1");

            // ローディングシーンをアンロード
            SceneLoader.UnloadScene("LoadingScene");

            loadStatus = EnumCollection.Scene.SCENE_LOAD_STATUS.COMPLETE;
        }
    }
}
