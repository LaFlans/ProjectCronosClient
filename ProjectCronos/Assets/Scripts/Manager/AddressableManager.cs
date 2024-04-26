using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ProjectCronos
{
    /// <summary>
    /// AddressableAssetを管理するクラス
    /// </summary>
    class AddressableManager : Singleton<AddressableManager>
    {
        /// <summary>
        /// 読み込み済みオブジェクトパス
        /// </summary>
        Dictionary<string, AsyncOperationHandle<GameObject>> loadedObjects;

        /// <summary>
        /// 読み込み済みマテリアルパス
        /// </summary>
        Dictionary<string, AsyncOperationHandle<Material>> loadedMaterials;

        /// <summary>
        /// 読み込み済みAudioClipパス
        /// </summary>
        Dictionary<string, AsyncOperationHandle<AudioClip>> loadedclips;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        public override async UniTask<bool> Initialize()
        {
            loadedObjects = new Dictionary<string, AsyncOperationHandle<GameObject>>();
            loadedMaterials = new Dictionary<string, AsyncOperationHandle<Material>>();
            loadedclips = new Dictionary<string, AsyncOperationHandle<AudioClip>>();

            Debug.Log("AddressableManager初期化");

            return true;
        }

        /// <summary>
        /// 読み込み処理
        /// </summary>
        /// <param name="path">AddressableのPath</param>
        /// <param name="callback">読み込み成功時に行うコールバック処理</param>
        /// <returns></returns>
        public async UniTask Load(string path, Action callback = null)
        {
            if (!loadedObjects.ContainsKey(path))
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(path);
                await handle.Task;
                handle.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!loadedObjects.ContainsKey(path))
                        {
                            loadedObjects.Add(path, op);
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
                Debug.Log($"path:{path}は既に読み込まれているよ！");
            }
        }

        public async UniTask LoadClip(string path, Action callback = null)
        {
            if (!loadedclips.ContainsKey(path))
            {
                var handle = Addressables.LoadAssetAsync<AudioClip>(path);
                await handle.Task;
                handle.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!loadedclips.ContainsKey(path))
                        {
                            loadedclips.Add(path, op);

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
                Debug.Log($"path:{path}のクリップは既に読み込まれているよ！");
            }
        }

        public AudioClip GetLoadedClip(string path)
        {
            if (loadedclips.ContainsKey(path))
            {
                return loadedclips[path].Result;
            }

            LoadClip(path);
            return null;
        }

        public async UniTask LoadMaterial(string path, Action callback = null)
        {
            if (!loadedMaterials.ContainsKey(path))
            {
                var handle = Addressables.LoadAssetAsync<Material>(path);
                await handle.Task;
                handle.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!loadedMaterials.ContainsKey(path))
                        {
                            loadedMaterials.Add(path, op);

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
                Debug.Log($"path:{path}のクリップは既に読み込まれているよ！");
            }
        }

        public Material GetLoadedMaterial(string path)
        {
            if (loadedMaterials.ContainsKey(path))
            {
                return loadedMaterials[path].Result;
            }

            LoadMaterial(path);
            Debug.Log("事前に読み込むようにしてね！");
            return null;
        }

        public async void Load<T>(string path, Action callback = null)
        {
            if (!loadedObjects.ContainsKey(path))
            {
                var handle = Addressables.LoadAssetAsync<T>(path);
                await handle.Task;
                handle.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!loadedObjects.ContainsKey(path))
                        {
                            //loaded.Add(path, op);
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
                Debug.Log($"path:{path}は既に読み込まれているよ！");
            }
        }

        public async UniTask LoadInstance(string path, Action<GameObject> callback = null, Transform parent = null)
        {
            if (!loadedObjects.ContainsKey(path))
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(path);
                await handle.Task;
                handle.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!loadedObjects.ContainsKey(path))
                        {
                            loadedObjects.Add(path, op);
                            if (callback != null)
                            {
                                if (parent != null)
                                {
                                    callback.Invoke(Instantiate(loadedObjects[path].Result, parent));
                                    return;
                                }

                                callback.Invoke(Instantiate(loadedObjects[path].Result));
                            }
                        }
                    }
                };
            }
            else
            {
                Debug.Log($"path:{path}は既に読み込まれているよ！");
            }
        }

        public GameObject GetLoadedObject(string path)
        {
            if (loadedObjects.ContainsKey(path))
            {
                return Instantiate(loadedObjects[path].Result, transform);
            }

            Load(path);
            Debug.Log("事前に読み込むようにしてね！");
            return null;
        }

        public T GetLoadedComponent<T>(string path)
        {
            if (loadedObjects.ContainsKey(path))
            {
                if (typeof(T) == loadedObjects[path].Result.GetType())
                {
                    return loadedObjects[path].Result.GetComponent<T>();
                }
                else
                {
                    //Debug.Log("読み込もうとしているアセットの型が一致しないよ！");
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
            if (loadedObjects != null)
            {
                foreach(var obj in loadedObjects.Values)
                {
                    Addressables.Release(obj);
                }

                loadedObjects.Clear();
            }

            // 読み込んだAddressbleオブジェクトを解放
            if (loadedMaterials != null)
            {
                foreach (var obj in loadedMaterials.Values)
                {
                    Addressables.Release(obj);
                }

                loadedMaterials.Clear();
            }

            // 読み込んだAddressbleAudioClipオブジェクトを解放
            if (loadedclips != null)
            {
                foreach (var obj in loadedclips.Values)
                {
                    Addressables.Release(obj);
                }

                loadedclips.Clear();
            }
        }
    }
}
