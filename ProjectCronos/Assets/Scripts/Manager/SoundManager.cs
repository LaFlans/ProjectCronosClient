using UnityEngine;

namespace ProjectCronos
{
    public class SoundManager : ISingleton<SoundManager>
    {
        SoundPlayer player;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        protected override bool Initialize()
        {
            AddressableManager.instance.LoadInstance(
                "Assets/Prefabs/SoundPlayer.prefab",
                (obj) =>
                {
                    player = obj.GetComponent<SoundPlayer>();
                    player.Init();
                },
                this.transform);

            return true;
        }

        /// <summary>
        /// BGM再生
        /// </summary>
        /// <param name="key">再生したいサウンドリソースのキー</param>
        public void Play(string key)
        {
            player?.Play(key);
        }
    }
}