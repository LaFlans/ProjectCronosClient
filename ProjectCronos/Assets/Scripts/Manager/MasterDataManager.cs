using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Generated;
using MessagePack.Resolvers;
using MessagePack;

namespace ProjectCronos
{
    internal class MasterDataManager : ISingleton<MasterDataManager>
    {
        private static MemoryDatabase _db;
        public static MemoryDatabase DB => _db;

        string masterDataPath = "Assets/MasterData/Generated/master-data.bytes";

        protected override bool Initialize()
        {
            Addressables.LoadAsset<TextAsset>(masterDataPath)
                        .Completed += op =>
                        {
                            _db = new MemoryDatabase(op.Result.bytes);
                        };

            return true;
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
