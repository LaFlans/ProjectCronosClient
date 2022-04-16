using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;

namespace ProjectCronos
{
    internal class MasterDataManager : ISingleton<MasterDataManager>
    {
        string masterDataPath = "Assets/MasterData/master-data.bytes";
        TextAsset asset = null;
        Byte[] bytes = null;

        protected override bool Initialize()
        {
            Addressables.LoadAsset<TextAsset>(masterDataPath)
                        .Completed += op => 
                        {
                            Debug.Log("マスタテスト");
                            asset = op.Result;
                            bytes = asset.bytes;
                        };

            return true;
        }
    }
}
