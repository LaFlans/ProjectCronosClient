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
        Dictionary<string, GameObject> loadedObjects;

        /// <summary>
        /// 読み込み済みマテリアルパス
        /// </summary>
        Dictionary<string, Material> loadedMaterials;

        /// <summary>
        /// 読み込み済みAudioClip
        /// </summary>
        Dictionary<string, AudioClip> loadedAudioClips;

        /// <summary>
        /// 読み込み済みAudioClipパス
        /// </summary>
        Dictionary<string, Texture2D> loadedTextures;

        /// <summary>
        /// 読み込み済みシナリオシーン
        /// </summary>
        Dictionary<string, ScenarioSceneScriptableObject> loadedScenarioScenes;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        public override async UniTask<bool> Initialize()
        {
            loadedObjects = new Dictionary<string, GameObject>();
            loadedMaterials = new Dictionary<string, Material>();
            loadedAudioClips = new Dictionary<string, AudioClip>();
            loadedTextures = new Dictionary<string, Texture2D>();
            loadedScenarioScenes = new Dictionary<string, ScenarioSceneScriptableObject>();

            Debug.Log("AddressableManager初期化");

            return true;
        }

        /// <summary>
        /// 全てのAddressableなアセットの読み込み
        /// </summary>
        /// <returns>UniTask.</returns>
        public async UniTask LoadAllAddressableAssets()
        {
            await LoadAllTextures();       // テクスチャ
            await LoadAllAudioClips();     // オーディオクリップ
            await LoadAllScenarioScenes(); // シナリオ
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
                            loadedObjects.Add(path, op.Result);
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

        /// <summary>
        /// 全てのオーディオクリップを読み込む
        /// </summary>
        /// <param name="callback">完了後の</param>
        /// <returns></returns>
        public async UniTask LoadAllAudioClips(Action callback = null)
        {
            var t = Addressables.LoadResourceLocationsAsync("Sound");
            await t.Task;
            var locations = t.Result;
            foreach(var location in locations)
            {
                var p = location.PrimaryKey;
            }

            var handle = Addressables.LoadAssetsAsync<AudioClip>("Sound", null);
            await handle.Task;

            handle.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var item in handle.Result)
                    {
                        if (!loadedAudioClips.ContainsKey(item.name))
                        {
                            Debug.Log($"path:{item.name}を読みこみ！");

                            loadedAudioClips.Add(item.name, item);

                            if (callback != null)
                            {
                                callback.Invoke();
                            }
                        }
                        else
                        {
                            Debug.Log($"path:{item.name}は既に読み込まれているよ！");
                        }
                    }
                }
            };
        }

        public async UniTask LoadAudioClip(string path, Action callback = null)
        {
            if (!loadedAudioClips.ContainsKey(path))
            {
                var handle = Addressables.LoadAssetAsync<AudioClip>(path);
                await handle.Task;
                handle.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!loadedAudioClips.ContainsKey(path))
                        {
                            loadedAudioClips.Add(path, op.Result);

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
            if (loadedAudioClips.ContainsKey(path))
            {
                return loadedAudioClips[path];
            }

            LoadAudioClip(path);
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
                            loadedMaterials.Add(path, op.Result);

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
                return loadedMaterials[path];
            }

            LoadMaterial(path);
            Debug.Log("事前に読み込むようにしてね！");
            return null;
        }

        /// <summary>
        /// 全てのテクスチャを読み込む
        /// </summary>
        /// <param name="callback">完了後の</param>
        /// <returns></returns>
        public async UniTask LoadAllTextures(Action callback = null)
        {
            var handle = Addressables.LoadAssetsAsync<Texture2D>("Image", null);
            await handle.Task;

            handle.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var item in handle.Result)
                    {
                        if (!loadedTextures.ContainsKey(item.name))
                        {
                            loadedTextures.Add(item.name, item);

                            if (callback != null)
                            {
                                callback.Invoke();
                            }
                        }
                        else
                        {
                            Debug.Log($"path:{item.name}は既に読み込まれているよ！");
                        }
                    }
                }
            };
        }


        public async UniTask LoadTexture(string path, Action callback = null)
        {
            if (!loadedTextures.ContainsKey(path))
            {
                var handle = Addressables.LoadAssetAsync<Texture2D>(path);
                await handle.Task;
                handle.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!loadedTextures.ContainsKey(path))
                        {
                            loadedTextures.Add(path, op.Result);

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

        public Texture2D GetLoadedTextures(string path)
        {
            if (loadedTextures.ContainsKey(path))
            {
                return loadedTextures[path];
            }

            LoadTexture(path);
            Debug.Log("事前に読み込むようにしてね！");
            return null;
        }

        /// <summary>
        /// 全てのシナリオシーンを読み込む
        /// </summary>
        /// <param name="callback">完了後の</param>
        /// <returns></returns>
        public async UniTask LoadAllScenarioScenes(Action callback = null)
        {
            var handle = Addressables.LoadAssetsAsync<ScenarioSceneScriptableObject>("ScenarioScene", null);
            await handle.Task;

            handle.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var item in handle.Result)
                    {
                        if (!loadedScenarioScenes.ContainsKey(item.name))
                        {
                            loadedScenarioScenes.Add(item.name, item);

                            if (callback != null)
                            {
                                callback.Invoke();
                            }
                        }
                        else
                        {
                            Debug.Log($"path:{item.name}は既に読み込まれているよ！");
                        }
                    }
                }
            };
        }

        public async UniTask LoadScenarioScenes(string path, Action callback = null)
        {
            if (!loadedScenarioScenes.ContainsKey(path))
            {
                var handle = Addressables.LoadAssetAsync<ScenarioSceneScriptableObject>(path);
                await handle.Task;
                handle.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!loadedScenarioScenes.ContainsKey(path))
                        {
                            loadedScenarioScenes.Add(path, op.Result);

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

        public List<string> GetLoadedScenarioScenes(string path)
        {
            if (loadedScenarioScenes.ContainsKey(path))
            {
                return loadedScenarioScenes[path].scenarioTexts;
            }

            LoadScenarioScenes(path);
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
                            loadedObjects.Add(path, op.Result);
                            if (callback != null)
                            {
                                if (parent != null)
                                {
                                    callback.Invoke(Instantiate(loadedObjects[path], parent));
                                    return;
                                }

                                callback.Invoke(Instantiate(loadedObjects[path]));
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
                return Instantiate(loadedObjects[path], transform);
            }

            Load(path);
            Debug.Log("事前に読み込むようにしてね！");
            return null;
        }

        public T GetLoadedComponent<T>(string path)
        {
            if (loadedObjects.ContainsKey(path))
            {
                if (typeof(T) == loadedObjects[path].GetType())
                {
                    return loadedObjects[path].GetComponent<T>();
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
            //// 読み込んだAddressbleオブジェクトを解放
            //if (loadedObjects != null)
            //{
            //    foreach(var obj in loadedObjects.Values)
            //    {
            //        Addressables.Release(obj);
            //    }

            //    loadedObjects.Clear();
            //}

            //// 読み込んだAddressbleオブジェクトを解放
            //if (loadedMaterials != null)
            //{
            //    foreach (var obj in loadedMaterials.Values)
            //    {
            //        Addressables.Release(obj);
            //    }

            //    loadedMaterials.Clear();
            //}

            //// 読み込んだAddressbleAudioClipオブジェクトを解放
            //if (loadedclips != null)
            //{
            //    foreach (var obj in loadedclips.Values)
            //    {
            //        Addressables.Release(obj);
            //    }

            //    loadedclips.Clear();
            //}

            //// 読み込んだAddressbleテクスチャオブジェクトを解放
            //if (loadedTextures != null)
            //{
            //    foreach (var obj in loadedTextures.Values)
            //    {
            //        Addressables.Release(obj);
            //    }

            //    loadedTextures.Clear();
            //}

            //// 読み込んだAddressbleシナリオシーンオブジェクトを解放
            //if (loadedScenarioScenes != null)
            //{
            //    foreach (var obj in loadedScenarioScenes)
            //    {
            //        Addressables.Release(obj.Value);
            //    }

            //    loadedScenarioScenes.Clear();
            //}
        }
    }
}
