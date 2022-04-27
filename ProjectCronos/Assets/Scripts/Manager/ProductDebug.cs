using UnityEngine;
using System;
using System.Threading.Tasks;

namespace ProjectCronos
{
    class ProductDebug : ISingleton<ProductDebug>
    {
        bool isUse = true;

        GameObject graphy;
        string path = "Assets/Resources_moved/Prefabs/Graphy.prefab";

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        public override async Task<bool> Initialize()
        {
            if (isUse)
            {
                await AddressableManager.instance.LoadInstance(path,
                    (obj) =>
                    {
                        graphy = obj;
                    });
            }

            return true;
        }
    }
}
