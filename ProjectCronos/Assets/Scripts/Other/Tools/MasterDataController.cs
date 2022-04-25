using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Data;
using System.Reflection;
using System.Linq;
using Generated;
using MessagePack.Resolvers;
using MessagePack;
using UnityEditor.IMGUI.Controls;


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

        [MenuItem("Cronos/MasterDataController")]
        static void Open()
        {
            var window  = GetWindow<MasterDataController>();
            window.titleContent = new GUIContent("MasterDataController");
        }

        void OnGUI()
        {
            var style = new GUIStyle(EditorStyles.label);
            style.richText = true;
            GUILayout.Label("<b><size=25>コード生成</size></b>", style);
            using (new GUILayout.HorizontalScope())
            {
                //　MasterMemoryのコードを生成
                if (GUILayout.Button("MasterMemoryCodeGenerate"))
                {
                    MasterMemoryGenerator.GenerateMasterMemory();
                }

                //　メッセージパックのコード生成
                if (GUILayout.Button("MessagePackCodeGenerate"))
                {
                    MessagePackGenerator.GenerateMessagePack();
                }
            }

            GUILayout.Label("<b><size=25>ScriptableObject</size></b>", style);
            if (objects.Count() > 0)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    //　ScriptableObjectを再読み込み
                    if (GUILayout.Button("ReLoadScriptableObject"))
                    {
                        LoadScriptableObject();
                    }

                    //　ScriptableObjectを初期化
                    if (GUILayout.Button("Clear"))
                    {
                        objects.Clear();
                    }
                }

                //　ScriptableObjectの値をバイナリにセーブ
                if (GUILayout.Button("Save"))
                {
                    LoadScriptableObject();
                    SaveMasterData();
                }

                foreach(var obj in objects)
                {

                }

                index = EditorGUILayout.Popup("表示するデータ", index, objects.Select(o => o.GetType().Name).ToArray());
                var messages = objects[index].GetMasterDataDebugMessage();
                foreach (var message in messages)
                {
                    GUILayout.Label(message, style);
                }
            }
            else
            {
                //　ScriptableObjectを読み込む
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
                                guid => {
                                    var path = AssetDatabase.GUIDToAssetPath(guid);
                                    return AssetDatabase.LoadAssetAtPath<MasterDataScriptableObject>(path);
                                        });

            foreach(var asset in assets)
            {
                objects.Add(asset);
            }
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
        }
#endif
    }
}
