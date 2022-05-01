using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Generated;
using Cysharp.Threading.Tasks;
using MessagePack.Resolvers;
using MessagePack;

namespace ProjectCronos
{
    internal class MasterDataManager : ISingleton<MasterDataManager>
    {
        private static MemoryDatabase _db;
        public static MemoryDatabase DB => _db;

        string masterDataPath = "Assets/MasterData/Generated/master-data.bytes";

        public override async UniTask<bool> Initialize()
        {
            Addressables.LoadAsset<TextAsset>(masterDataPath)
                        .Completed += op =>
                        {
                            _db = new MemoryDatabase(op.Result.bytes);
                        };

            return true;
        }

        /// <summary>
        /// マスタデータに登録されているサウンドデータの読み込み
        /// 基本的にSoundPlayerしか使用しない
        /// </summary>
        public async UniTask LoadSoundData()
        {
            foreach (var item in DB.SoundTable.All)
            {
                await AddressableManager.instance.LoadClip(item.Path);
            }
        }

        /// <summary>
        /// 文言取得
        /// </summary>
        /// <param name="Key">キーとなる文字列</param>
        /// <returns>設定されている文字列</returns>
        public string GetDic(string Key)
        {
            return DB.DictionaryTable.FindByKey(Key).Message;
        }
    }
}
