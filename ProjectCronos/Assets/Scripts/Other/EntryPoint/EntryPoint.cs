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

            // マネージャーシーンを読み込む
            ManagerSceneLoader.LoadManagerScene();

            // マネージャーシーンの読み込みが終わるまで待つ
            while (!ManagerScene.isLaunch)
            {
                yield return null;
            }

            SoundManager.Instance.Play("BGM1");

            loadStatus = EnumCollection.Scene.SCENE_LOAD_STATUS.COMPLETE;
        }
    }
}
