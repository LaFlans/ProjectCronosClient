using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Generated;
using Cysharp.Threading.Tasks;
using MessagePack.Resolvers;
using MessagePack;

namespace ProjectCronos
{
    internal class MasterDataManager : Singleton<MasterDataManager>
    {
        private static MemoryDatabase _db;
        public static MemoryDatabase DB => _db;

        string masterDataPath = "Assets/MasterData/Generated/master-data.bytes";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeMessagePack()
        {
            StaticCompositeResolver.Instance.Register
            (
                MasterMemoryResolver.Instance,
                GeneratedResolver.Instance,
                StandardResolver.Instance
            );

            var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
            MessagePackSerializer.DefaultOptions = options;
        }

        public override async UniTask<bool> Initialize()
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(masterDataPath);
            await handle.Task;

            handle.Completed += op =>
            {
                _db = new MemoryDatabase(op.Result.bytes);
            };

            Debug.Log("MasterDataManager初期化");

            return true;
        }

        /// <summary>
        /// マスタデータに登録されているサウンドデータの読み込み
        /// 基本的にSoundPlayerしか使用しない
        /// </summary>
        public async UniTask LoadSoundData()
        {
            if (DB == null)
            {
                Debug.Log("DBがnullだよ！");
                return;
            }

            foreach (var item in DB.SoundTable.All)
            {
                await AddressableManager.instance.LoadClip(item.Path);
            }

            Debug.Log("Soundデータの読み込み");
        }

        /// <summary>
        /// 文言取得
        /// </summary>
        /// <param name="key">キーとなる文字列</param>
        /// <returns>設定されている文字列</returns>
        public string GetDic(string key)
        {
            if (DB == null)
            {
                return $"{key}は存在していません";
            }

            return DB.DictionaryTable.FindByKey(key).Message;
        }
    }
}
