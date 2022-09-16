using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;


namespace ProjectCronos
{
    class ManagerScene : MonoBehaviour
    {
        public static bool isLaunch;

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
#if UNITY_EDITOR
            this.gameObject.AddComponent<ProductDebug>();
#endif

            // マネージャーを初期化
            List<UniTask<bool>> task = new List<UniTask<bool>>();
            task.Add(AddressableManager.instance.Initialize());
            task.Add(MasterDataManager.instance.Initialize());
            task.Add(InputManager.instance.Initialize());
            task.Add(SoundManager.instance.Initialize());
            task.Add(PopupManager.instance.Initialize());
#if UNITY_EDITOR
            task.Add(ProductDebug.instance.Initialize());
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
            await MasterDataManager.instance.LoadSoundData();

            isLaunch = true;
        }
    }
}
