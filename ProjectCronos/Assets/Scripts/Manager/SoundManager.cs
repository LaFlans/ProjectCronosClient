using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    public class SoundManager : Singleton<SoundManager>
    {
        SoundPlayer player;
        Stack<string> soundKeys;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        public override async UniTask<bool> Initialize()
        {
            soundKeys = new Stack<string>();

            await AddressableManager.Instance.LoadInstance(
                "Assets/Prefabs/SoundPlayer.prefab",
                (obj) =>
                {
                    player = obj.GetComponent<SoundPlayer>();
                    player.Init();
                },
                this.transform);

            Debug.Log("SoundManager初期化");

            return true;
        }

        void Update()
        {
            if (soundKeys != null)
            {
                if (soundKeys.Count > 0)
                {
                    Play(soundKeys.Pop());
                }
            }
        }

        /// <summary>
        /// BGM再生
        /// </summary>
        /// <param name="key">再生したいサウンドリソースのキー</param>
        public void Play(string key)
        {
            //　プレイヤーがnullだったらスタックしておく
            if (player == null)
            {
                soundKeys.Push(key);
                return;
            }

            player.Play(key);
        }
    }
}
