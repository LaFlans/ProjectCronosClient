using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using Generated;
using MessagePack.Resolvers;
using MessagePack;


namespace ProjectCronos
{
    /// <summary>
    /// マスタデータの入力、保存、読み込み等を行うウィンドウクラス
    /// </summary>
    internal class MasterDataController : EditorWindow
    {
#if UNITY_EDITOR
        List<MasterDataScriptableObject> objects = new List<MasterDataScriptableObject>();
        List<string> options;
        int index = 0;

        /// <summary>
        /// 差分データ表示スクロールポジションキャッシュ
        /// </summary>
        private Vector2 diffDataScrollPosition = Vector2.zero;

        /// <summary>
        /// DBデータ表示スクロールポジションキャッシュ
        /// </summary>
        private Vector2 dataBaseScrollPosition = Vector2.zero;

        /// <summary>
        /// DBの変更前の差分を表示するかどうか
        /// </summary>
        bool isShowBeforeDiff = false;

        /// <summary>
        /// DBの変更差分の変更されていないデータも表示するかどうか
        /// </summary>
        bool isShowAllData = false;

        [MenuItem("Cronos/MasterDataController")]
        static void Open()
        {
            var window = GetWindow<MasterDataController>();
            window.titleContent = new GUIContent("MasterDataController");
        }

        void OnGUI()
        {
            var style = new GUIStyle(EditorStyles.label);
            style.richText = true;
            using (new EditorGUILayout.VerticalScope("BOX"))
            {
                GUILayout.Label("<b>コード生成</b>", style);
                using (new GUILayout.HorizontalScope())
                {
                    // MasterMemoryのコードを生成
                    if (GUILayout.Button("MasterMemoryCodeGenerate"))
                    {
                        MasterMemoryGenerator.GenerateMasterMemory();
                    }

                    // メッセージパックのコード生成
                    if (GUILayout.Button("MessagePackCodeGenerate"))
                    {
                        MessagePackGenerator.GenerateMessagePack();
                    }
                }
            }

            if (objects.Count() > 0)
            {
                using (new EditorGUILayout.VerticalScope("BOX"))
                {
                    GUILayout.Label("<b>ScriptableObject</b>", style);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        // ScriptableObjectを再読み込み
                        if (GUILayout.Button("ReLoadScriptableObject"))
                        {
                            LoadScriptableObject();
                        }

                        // ScriptableObjectを初期化
                        if (GUILayout.Button("Clear"))
                        {
                            objects.Clear();

                            // objectがない状態でそのまま進むのはダメなので抜ける
                            return;
                        }

                        // ScriptableObjectの値をバイナリにセーブ
                        if (GUILayout.Button("Save"))
                        {
                            LoadScriptableObject();
                            SaveMasterData();
                        }
                    }
                }

                using (new EditorGUILayout.VerticalScope("BOX"))
                {
                    GUILayout.Label("<b>変更差分</b>", style);

                    //　チェックボックス関連
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        isShowBeforeDiff = GUILayout.Toggle(isShowBeforeDiff, "変更前の差分を表示するか");
                        isShowAllData = GUILayout.Toggle(isShowAllData, "変更していないデータも表示するか");
                    }

                    // ここからスクロールエリア
                    diffDataScrollPosition = EditorGUILayout.BeginScrollView(diffDataScrollPosition);

                    var messages = new List<string>();
                    foreach (var obj in objects)
                    {
                        messages = obj.GetMasterDataDiffDebugMessage(isShowBeforeDiff, isShowAllData);

                        if (messages.Count() > 0)
                        {
                            //　仕切り線
                            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));

                            // データタイトル表示
                            GUILayout.Label(obj.GetMasterDataTitle(), style);

                            foreach (var message in messages)
                            {
                                GUILayout.Label(message, style);
                            }

                            //　仕切り線
                            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
                        }
                    }

                    // スクロールエリア終了
                    EditorGUILayout.EndScrollView();
                }

                using (new EditorGUILayout.VerticalScope("BOX"))
                {
                    GUILayout.Label("<b>現在DBに保存されているバイナリデータ</b>", style);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        index = EditorGUILayout.Popup("表示するデータ", index, objects.Select(o => o.GetType().Name).ToArray());

                        // 選択しているScriptableObjectを開く
                        if (GUILayout.Button("開く", GUILayout.Width(50)))
                        {
                            Selection.activeObject = objects[index];
                        }
                    }

                    // データタイトル表示
                    GUILayout.Label(objects[index].GetMasterDataTitle(), style);

                    // ここからスクロールエリア
                    dataBaseScrollPosition = EditorGUILayout.BeginScrollView(dataBaseScrollPosition);

                    var messages = objects[index].GetMasterDataDebugMessage();
                    foreach (var message in messages)
                    {
                        GUILayout.Label(message, style);
                    }

                    // スクロールエリア終了
                    EditorGUILayout.EndScrollView();
                }
            }
            else
            {
                // ScriptableObjectを読み込む
                if (GUILayout.Button("LoadScriptableObject"))
                {
                    LoadScriptableObject();
                }
            }
        }

        void LoadScriptableObject()
        {
            // オブジェクトの中身初期化
            objects.Clear();

            var assets = AssetDatabase.FindAssets(
                            "t:" + typeof(MasterDataScriptableObject).Name)
                            .Select(
                                guid =>
                                {
                                    var path = AssetDatabase.GUIDToAssetPath(guid);
                                    return AssetDatabase.LoadAssetAtPath<MasterDataScriptableObject>(path);
                                });

            foreach (var asset in assets)
            {
                objects.Add(asset);
            }

            // indexも初期化
            index = 0;
        }

        void SaveMasterData()
        {
            // MessagePackのResolverを設定
            try
            {
                StaticCompositeResolver.Instance.Register
                (
                    new IFormatterResolver[]
                    {
                    MasterMemoryResolver.Instance,
                    GeneratedResolver.Instance,
                    StandardResolver.Instance,
                    });
                var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
                MessagePackSerializer.DefaultOptions = options;
            }
            catch
            {
            }

            var builder = new DatabaseBuilder();
            foreach (var obj in objects)
            {
                obj.Save(builder);
            }

            byte[] data = builder.Build();
            Debug.Log("ConvertToJson : " + MessagePackSerializer.ConvertToJson(data));
            var resourcesDir = $"{Application.dataPath}/MasterData/Generated";
            Directory.CreateDirectory(resourcesDir);
            var filename = "/master-data.bytes";

            using (var fs = new FileStream(resourcesDir + filename, FileMode.Create))
            {
                fs.Write(data, 0, data.Length);
            }

            Debug.Log($"Write byte[] to: {resourcesDir + filename}");
            Debug.Log("Complete!");

            AssetDatabase.Refresh();

            // ScriptableObjectのDBキャッシュ更新
            foreach(var obj in objects)
            {
                obj.UpdateDBCache();
            }
        }
#endif
    }
}
