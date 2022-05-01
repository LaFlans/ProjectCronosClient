using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    /// <summary>
    /// シーンのエントリーポイント
    /// </summary>
    public class EntryPoint : MonoBehaviour
    {
        EnumCollection.Scene.SCENE_LOAD_STATUS loadStatus;

        [SerializeField]
        Player player;

        async void Start()
        {
            loadStatus = EnumCollection.Scene.SCENE_LOAD_STATUS.WAITING;

            if (!await Initialize())
            {
                Debug.Log("シーンの読み込みに失敗したよ…");
            }
        }

        async UniTask<bool> Initialize()
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
                await UniTask.Yield();
            }

            //　シーンの事前読み込み
            await PreLoadAsset();

            SoundManager.Instance.Play("BGM2");

            // ローディングシーンをアンロード
            SceneLoader.UnloadScene("LoadingScene");

            loadStatus = EnumCollection.Scene.SCENE_LOAD_STATUS.COMPLETE;

            return true;
        }

        /// <summary>
        /// 事前に読み込んでおきたいアセットをここで読み込む
        /// </summary>
        /// <returns>UniTask</returns>
        async UniTask PreLoadAsset()
        {
            // 事前読み込み
            await player.PreLoadAsync();
        }
    }
}
