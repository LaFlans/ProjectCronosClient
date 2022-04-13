using UnityEngine;
using System;

namespace ProjectCronos
{
    class ProductDebug : ISingleton<ProductDebug>
    {
        GameObject graphy;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        protected override bool Initialize()
        {
            Utility.CreateObject("Assets/Resources_moved/Prefabs/Graphy.prefab");
            return true;
        }
    }
}
