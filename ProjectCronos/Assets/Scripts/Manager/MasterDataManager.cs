using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Generated;

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
                            Debug.Log("マスタの読み込みが終わったよ！");
                            _db = new MemoryDatabase(op.Result.bytes);
                        };

            return true;
        }
    }
}
