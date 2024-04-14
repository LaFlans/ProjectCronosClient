using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    /// <summary>
    /// すべてのマネージャーを管理するクラス
    /// </summary>
    class ManagerScene : MonoBehaviour
    {
        /// <summary>
        /// 一度でも起動したか
        /// </summary>
        public static bool isLaunch;

        /// <summary>
        /// 現在のシーン
        /// </summary>
        static EnumCollection.Scene.SCENE_TYPE currentScene;

        /// <summary>
        /// ゲーム状態
        /// </summary>
        static EnumCollection.Game.GAME_STATUS gameStatus;

        void Awake()
        {
            isLaunch = false;
            FirstLaunch();
        }

        /// <summary>
        /// 起動時処理
        /// </summary>
        public async void FirstLaunch()
        {
            // AddressableManagerとMasterDataManagerは最優先で生成
            this.gameObject.AddComponent<AddressableManager>();
            this.gameObject.AddComponent<MasterDataManager>();

            // 常に存在するべき物を生成
            new GameObject("InputManager").AddComponent<InputManager>();
            new GameObject("SoundManager").AddComponent<SoundManager>();
            this.gameObject.AddComponent<PopupManager>();
            this.gameObject.AddComponent<TimeManager>();
            this.gameObject.AddComponent<SaveManager>();
#if UNITY_EDITOR
            //this.gameObject.AddComponent<DebugManager>();
#endif
            // マネージャーを初期化
            List<UniTask<bool>> task = new List<UniTask<bool>>();
            task.Add(AddressableManager.Instance.Initialize());
            task.Add(MasterDataManager.Instance.Initialize());
            task.Add(InputManager.Instance.Initialize());
            task.Add(SoundManager.Instance.Initialize());
            task.Add(PopupManager.Instance.Initialize());
            task.Add(TimeManager.Instance.Initialize());
            task.Add(SaveManager.Instance.Initialize());

#if UNITY_EDITOR
            //task.Add(DebugManager.Instance.Initialize());
#endif
            foreach (var item in task)
            {
                if (!await item)
                {
                    Debug.Log("マネージャーシーンの初期化に失敗したよ…");
                    break;
                }
            }

            // その他データ読み込み
            // マスタデータに登録されているサウンドデータを読み込む
            await MasterDataManager.Instance.LoadSoundData();

            isLaunch = true;
        }

        /// <summary>
        /// 現在のシーンを設定
        /// </summary>
        /// <param name="scene">設定するシーン</param>
        public static void SetCurrentScene(EnumCollection.Scene.SCENE_TYPE scene)
        {
            currentScene = scene;
        }

        /// <summary>
        /// 現在のシーンを返す
        /// </summary>
        /// <returns>現在のシーン</returns>
        public static EnumCollection.Scene.SCENE_TYPE GetCurrentScene()
        {
            return currentScene;
        }

        /// <summary>
        /// ゲーム状態を設定
        /// </summary>
        /// <param name="scene">設定するゲーム状態</param>
        public static void SetGameStatus(EnumCollection.Game.GAME_STATUS status)
        {
            gameStatus = status;
        }

        /// <summary>
        /// 現在のゲーム状態を返す
        /// </summary>
        /// <returns>現在のゲーム状態</returns>
        public static EnumCollection.Game.GAME_STATUS GetGameStatus()
        {
            return gameStatus;
        }
    }
}
