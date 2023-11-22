using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProjectCronos
{
    internal class MaterialCreator : EditorWindow
    {
#if UNITY_EDITOR
        private const string defaultMaterialPath = "Assets/ProjectCronosAssets/Materials/Default/DefaultMat.mat";
        private static string[] extensions = { ".png" };
        private int index = 0;
        private string[] folderPaths;
        private string[] folderNames;
        private List<string> textureNames = new List<string>();

        private bool isShowGenerateLog = false;
        private bool isForce = false;

        Material selectMat;

        class TextureIdentifer
        {
            public string materialValue;
            public string shaderProperty;
            public TextureIdentifer(string materialValue, string shaderProperty)
            {
                this.materialValue = materialValue;
                this.shaderProperty = shaderProperty;
            }
        }

        private List<TextureIdentifer> textureIdentifers = new List<TextureIdentifer>();

        [MenuItem("Tools/MaterialGenerator")]
        static void Open()
        {
            var window = GetWindow<MaterialCreator>();
            window.titleContent = new GUIContent("MaterialGenerator");
        }

        private void OnGUI()
        {
            var obj = ProjectCronosSettings.GetSerializedSettings();
            var prop = obj.FindProperty("substanceTexturesPath");
            if (prop.stringValue == string.Empty)
            {
                return;
            }

            // Substanceで生成したテクスチャのフォルダ指定
            using (new EditorGUILayout.VerticalScope("BOX"))
            {
                folderPaths = AssetDatabase.GetSubFolders(prop.stringValue);

                if (folderPaths.Length > 0)
                {
                    folderNames = folderPaths.Select(s => Path.GetFileName(s)).ToArray();
                    index = EditorGUILayout.Popup("指定フォルダ", index, folderNames);

                    // アタッチされたゲームオブジェクト
                    selectMat = (Material)EditorGUILayout.ObjectField("Material", selectMat, typeof(Material), true);

                    if (GUILayout.Button("CreateMaterial"))
                    {
                        // テクスチャ識別子設定
                        textureIdentifers.Clear();
                        textureIdentifers.Add(new TextureIdentifer(obj.FindProperty("colorMapTextureIdentifer").stringValue, obj.FindProperty("colorMapShaderIdentifer").stringValue));
                        textureIdentifers.Add(new TextureIdentifer(obj.FindProperty("maskMapTextureIdentifer").stringValue, obj.FindProperty("maskMapShaderIdentifer").stringValue));
                        textureIdentifers.Add(new TextureIdentifer(obj.FindProperty("normalMapTextureIdentifer").stringValue, obj.FindProperty("normalMapShaderIdentifer").stringValue));
                        textureIdentifers.Add(new TextureIdentifer(obj.FindProperty("emissiveMapTextureIdentifer").stringValue, obj.FindProperty("emissiveMapShaderIdentifer").stringValue));
                        textureIdentifers.Add(new TextureIdentifer(obj.FindProperty("specularMapTextureIdentifer").stringValue, obj.FindProperty("specularMapShaderIdentifer").stringValue));

                        // マテリアル生成
                        GenerateMaterial(obj.FindProperty("generateMaterialMax").intValue);
                    }
                }
                else
                {
                    GUILayout.Label("ProjectSettingsにて、正しいフォルダパスを設定してください");
                    index = 0;
                }
            }

            using (new EditorGUILayout.VerticalScope("BOX"))
            {
                GUILayout.Label("その他");
                if (GUILayout.Button("ProjectSettingsを開く"))
                {
                    SettingsService.OpenProjectSettings("Project/CustomIMGUISettings");
                }

                isShowGenerateLog = GUILayout.Toggle(isShowGenerateLog, "ログを表示");
                isForce = GUILayout.Toggle(isForce, "強制的に上書きして生成するか");
            }
        }

        void GenerateMaterial(int generateMaterialMax)
        {
            textureNames.Clear();

            using (new EditorGUILayout.VerticalScope())
            {
                string[] guids = AssetDatabase.FindAssets("", new string[] { folderPaths[index] });
                string[] paths = guids.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();

                // テクスチャ
                foreach (var path in paths)
                {
                    if (!extensions.Contains(Path.GetExtension(path)))
                    {
                        continue;
                    }

                    foreach (var identifer in textureIdentifers)
                    {
                        if (path.Contains(identifer.materialValue))
                        {
                            var s = path.Replace(identifer.materialValue, "");
                            s = Path.GetFileNameWithoutExtension(s);
                            if (!textureNames.Contains(s))
                            {
                                textureNames.Add(s);
                            }
                        }
                    }
                }

                // 生成しなければいけないマテリアルの数が設定されている最大を超えた場合、警告をして何も行わない
                if (textureNames.Count > generateMaterialMax)
                {
                    Debug.Log("生成できるマテリアル最大数を超えているので生成できません。テクスチャを整理してからもう一度試してください。");
                    return;
                }

                // マテリアル生成
                string matPath = string.Empty;
                Material mat = null;
                foreach (var name in textureNames)
                {
                    matPath = folderPaths[index] + "/" + name + ".mat";

                    // 指定のマテリアルが存在しないもしくは強制的に生成する状態の時
                    if (!File.Exists(matPath) || isForce)
                    {
                        if (isShowGenerateLog)
                        {
                            if (isForce)
                            {
                                Debug.Log($"{matPath}は既に存在していますが、強制的に上書き生成します");
                            }
                            else
                            {
                                Debug.Log($"{matPath}は存在していないので生成します");
                            }
                        }

                        // マテリアルをコピーして生成
                        // マテリアルが指定されていない場合、デフォルトのマテリアルを使用する
                        AssetDatabase.CopyAsset(selectMat == null ? defaultMaterialPath : AssetDatabase.GetAssetPath(selectMat), matPath);
                    }
                    else
                    {
                        if (isShowGenerateLog)
                        {
                            Debug.Log($"{matPath}は既に存在しているので、テクスチャのみ更新します");
                        }
                    }

                    mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);

                    foreach (var path in paths)
                    {
                        foreach (var identifer in textureIdentifers)
                        {
                            if (path.Contains(name + identifer.materialValue) &&
                                mat.HasProperty(identifer.shaderProperty))
                            {
                                mat.SetTexture(identifer.shaderProperty, AssetDatabase.LoadAssetAtPath<Texture2D>(path));
                            }
                        }
                    }
                }

                var tex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ProjectCronosAssets/Textures/Test/Test_20220206_Accessory_Normal.png");

                AssetDatabase.SaveAssets();
            }
        }
#endif
    }
}
