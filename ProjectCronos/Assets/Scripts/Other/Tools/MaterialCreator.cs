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
        private const string settingsPath = "Assets/Editor/Settings.asset";
        private const string defaultMaterialPath = "Assets/ProjectCronosAssets/Materials/Default/DefaultMat.mat";
        private static string[] extensions = { ".png" };
        private int index = 0;
        private string[] folderPaths;
        private string[] folderNames;
        private List<string> textureNames = new List<string>();

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

        [MenuItem("Tools/MaterialCreator")]
        static void Open()
        {
            var window = GetWindow<MaterialCreator>();
            window.titleContent = new GUIContent("MaterialCreator");
        }

        private void OnGUI()
        {
            var settings = AssetDatabase.LoadAllAssetsAtPath(settingsPath).FirstOrDefault();
            if (settings == null)
            {
                ProjectCronosSettings.GetSettings();
            }

            var obj = new SerializedObject(settings);
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
                        CreateMaterial();
                    }
                }
                else
                {
                    GUILayout.Label("ProjectSettingsにて、正しいフォルダパスを設定してください");
                }
            }

            using (new EditorGUILayout.VerticalScope("BOX"))
            {
                GUILayout.Label("その他");
                if (GUILayout.Button("ProjectSettingsを開く"))
                {
                    SettingsService.OpenProjectSettings("Project/CustomIMGUISettings");
                }
            }
        }

        void CreateMaterial()
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

                // マテリアル生成
                string matPath = string.Empty;
                Material mat = null;
                foreach (var name in textureNames)
                {
                    matPath = folderPaths[index] + "/" + name + ".mat";

                    if (!File.Exists(matPath))
                    {
                        Debug.Log($"{matPath}は存在していないので生成します");
                        AssetDatabase.CopyAsset(defaultMaterialPath, matPath);
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

                AssetDatabase.SaveAssets();
            }
        }
#endif
    }
}
