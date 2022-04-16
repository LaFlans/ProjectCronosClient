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

        public async void Load(string path, Action callback = null)
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
                            if (callback != null)
                            {
                                callback.Invoke();
                            }
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

        public GameObject InstantiateLoadedObject(string path, Transform parent = null)
        {
            if (loaded.ContainsKey(path))
            {
                if (parent != null)
                {
                    return Instantiate(loaded[path].Result, parent);
                }

                return Instantiate(loaded[path].Result);
            }

            Load(path);
            Debug.LogError("事前に読み込むようにしてね！");
            return null;
        }

        public T GetLoadedComponent<T>(string path)
        {
            if (loaded.ContainsKey(path))
            {
                if (typeof(T) == loaded[path].Result.GetType())
                {
                    return loaded[path].Result.GetComponent<T>();
                }
                else
                {
                    //Debug.LogError("読み込もうとしているアセットの型が一致しないよ！");
                    return default(T);
                }
            }

            Load(path);
            return default(T);
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
