using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ProjectCronos
{
    public abstract class IEntryPoint : MonoBehaviour
    {
        protected EnumCollection.Scene.SCENE_LOAD_STATUS loadStatus;

        public async void Start()
        {
            loadStatus = EnumCollection.Scene.SCENE_LOAD_STATUS.LOADING;

            // ローディングシーンを読み込む
            // FIXME: もっといい感じのロード処理にする(ローディングシーンは常に存在する形がよさそう？)
            SceneLoader.LoadScene(EnumExtension.GetDescriptionFromValue(EnumCollection.Scene.SCENE_TYPE.LOADING));

            if (!Utility.IsAlreadyLoadScene("ManagerScene"))
            {
                Debug.Log("マネージャーシーンが読み込まれていないので、ゲームの初期化を行うよ");

                // マネージャーシーンが読み込まれていない場合、ゲームの初期化を行う
                await GameInitialize();
            }

            // タイムスケール設定
            TimeManager.Instance.InitTimeScale();

            // ダミー読み込み
            await DummyLoad();

            // 事前読み込み
            await PreLoadAsset();

            // シーン初期化
            await Initialize();

            // ローディングシーンをアンロード
            SceneLoader.UnloadScene(EnumExtension.GetDescriptionFromValue(EnumCollection.Scene.SCENE_TYPE.LOADING));

            loadStatus = EnumCollection.Scene.SCENE_LOAD_STATUS.COMPLETE;
        }

        /// <summary>
        /// ダミーロード処理
        /// シーンの準備等のためにダミーで遅延させる
        /// </summary>
        /// <returns></returns>
        async UniTask DummyLoad()
        {
            // シーン準備のため、最低でも一定秒数ダミーロードを行う
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }

        /// <summary>
        /// ゲーム全体の初期化処理
        /// 基本1回しか通らない
        /// </summary>
        /// <returns>UniTask</returns>
        async UniTask<bool> GameInitialize()
        {
            // マネージャーシーンを読み込む
            SceneLoader.LoadScene("ManagerScene");

            // マネージャーシーンの読み込みが終わるまで待つ
            while (!ManagerScene.isLaunch)
            {
                await UniTask.Yield();
            }

            return true;
        }

        /// <summary>
        /// シーンの初期化処理
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        public abstract UniTask<bool> Initialize();

        /// <summary>
        /// 事前に読み込んでおきたいアセットをここで読み込む
        /// </summary>
        /// <returns>UniTask</returns>
        public abstract UniTask PreLoadAsset();
    }
}
