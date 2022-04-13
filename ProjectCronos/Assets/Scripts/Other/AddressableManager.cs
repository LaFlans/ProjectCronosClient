using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ProjectCronos
{
    class AddressableManager : ISingleton<AddressableManager>
    {
        /// <summary>
        /// 読み込み済みパス
        /// </summary>
        Dictionary<string, AsyncOperationHandle<GameObject>> loaded;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        protected override bool Initialize()
        {
            loaded = new Dictionary<string, AsyncOperationHandle<GameObject>>();
            return true;
        }

        public async void Load(string path)
        {
            if (!loaded.ContainsKey(path))
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(path);
                await handle.Task;
                handle.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!loaded.ContainsKey(path))
                        {
                            loaded.Add(path, op);
                        }
                    }
                };
            }
            else
            {
                Debug.LogError($"path:{path}は既に読み込まれているよ！");
            }
        }

        public GameObject GetLoadedObject(string path)
        {
            if (loaded.ContainsKey(path))
            {
                return loaded[path].Result;
            }

            Load(path);
            Debug.LogError("事前に読み込むようにしてね！");
            return null;
        }

        /// <summary>
        /// オブジェクト破棄時に呼ばれる
        /// </summary>
        void OnDestroy()
        {
            // 読み込んだAddressbleオブジェクトを解放
            if (loaded != null)
            {
                foreach(var obj in loaded.Values)
                {
                    Addressables.Release(obj);
                }

                loaded.Clear();
            }
        }
    }
}
