using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectCronos
{
    public class ProjectCronosSettings : ScriptableObject
    {
        public const string settingsPath = "Assets/Editor/Settings.asset";
        [SerializeField]
        private string colorMapTextureIdentifer;
        [SerializeField]
        private string maskMapTextureIdentifer;
        [SerializeField]
        private string normalMapTextureIdentifer;
        [SerializeField]
        private string emissiveMapTextureIdentifer;
        [SerializeField]
        private string specularMapTextureIdentifer;
        [SerializeField]
        private string colorMapShaderIdentifer;
        [SerializeField]
        private string maskMapShaderIdentifer;
        [SerializeField]
        private string normalMapShaderIdentifer;
        [SerializeField]
        private string emissiveMapShaderIdentifer;
        [SerializeField]
        private string specularMapShaderIdentifer;
        [SerializeField]
        private string substanceTexturesPath;
        [SerializeField]
        private int generateMaterialMax;
        [SerializeField]
        public bool IsMuteSound;

        static ProjectCronosSettings GetSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<ProjectCronosSettings>(settingsPath);
            if (settings == null)
            {
                settings = CreateInstance<ProjectCronosSettings>();
                settings.colorMapTextureIdentifer = "_BaseMap";
                settings.maskMapTextureIdentifer = "_Mask";
                settings.normalMapTextureIdentifer = "_Normal";
                settings.emissiveMapTextureIdentifer = "_Emissive";
                settings.specularMapTextureIdentifer = "_Specular";
                settings.colorMapShaderIdentifer = "_BaseColorMap";
                settings.maskMapShaderIdentifer = "_MaskMap";
                settings.normalMapShaderIdentifer = "_NormalMap";
                settings.emissiveMapShaderIdentifer = "_EmissiveMap";
                settings.specularMapShaderIdentifer = "_SpecularMap";
                settings.substanceTexturesPath = "Assets/ProjectCronosAssets/Textures";
                settings.generateMaterialMax = 10;
                settings.IsMuteSound = false;
                AssetDatabase.CreateAsset(settings, settingsPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetSettings());
        }
    }

    static class ProjectCronosSettingsIMGUIRegister
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Project/CustomIMGUISettings", SettingsScope.Project)
            {
                label = "MaterialGenerator",
                guiHandler = (searchContext) =>
                {
                    var settings = ProjectCronosSettings.GetSerializedSettings();
                    using (new EditorGUILayout.VerticalScope("HelpBox"))
                    {
                        EditorGUILayout.PropertyField(settings.FindProperty("substanceTexturesPath"), new GUIContent("SubstanceTexturePath"), GUILayout.Height(19));
                        if (GUILayout.Button("SearchFolder"))
                        {
                            var fullPath = EditorUtility.OpenFolderPanel("フォルダを選択してください", Application.dataPath, string.Empty);
                            settings.FindProperty("substanceTexturesPath").stringValue = FileUtil.GetProjectRelativePath(fullPath);
                        }
                    }

                    using (new EditorGUILayout.HorizontalScope("HelpBox"))
                    {
                        EditorGUILayout.PropertyField(settings.FindProperty("generateMaterialMax"), new GUIContent("GenerateMaterialMax"), GUILayout.Height(19));
                    }

                    GUILayout.Label("ColorIdentifer");
                    using (new EditorGUILayout.HorizontalScope("HelpBox"))
                    {
                        EditorGUILayout.PropertyField(settings.FindProperty("colorMapTextureIdentifer"), new GUIContent("TextureName"), GUILayout.Height(19));
                        EditorGUILayout.PropertyField(settings.FindProperty("colorMapShaderIdentifer"), new GUIContent("ShaderParameter"), GUILayout.Height(19));
                    }
                    GUILayout.Label("MaskIdentifer");
                    using (new EditorGUILayout.HorizontalScope("HelpBox"))
                    {
                        EditorGUILayout.PropertyField(settings.FindProperty("maskMapTextureIdentifer"), new GUIContent("TextureName"), GUILayout.Height(19));
                        EditorGUILayout.PropertyField(settings.FindProperty("maskMapShaderIdentifer"), new GUIContent("ShaderParameter"), GUILayout.Height(19));
                    }
                    GUILayout.Label("NormalIdentifer");
                    using (new EditorGUILayout.HorizontalScope("HelpBox"))
                    {
                        EditorGUILayout.PropertyField(settings.FindProperty("normalMapTextureIdentifer"), new GUIContent("TextureName"), GUILayout.Height(19));
                        EditorGUILayout.PropertyField(settings.FindProperty("normalMapShaderIdentifer"), new GUIContent("ShaderParameter"), GUILayout.Height(19));
                    }
                    GUILayout.Label("EmissiveIdentifer");
                    using (new EditorGUILayout.HorizontalScope("HelpBox"))
                    {
                        EditorGUILayout.PropertyField(settings.FindProperty("emissiveMapTextureIdentifer"), new GUIContent("TextureName"), GUILayout.Height(19));
                        EditorGUILayout.PropertyField(settings.FindProperty("emissiveMapShaderIdentifer"), new GUIContent("ShaderParameter"), GUILayout.Height(19));
                    }
                    GUILayout.Label("SpecularIdentifer");
                    using (new EditorGUILayout.HorizontalScope("HelpBox"))
                    {
                        EditorGUILayout.PropertyField(settings.FindProperty("specularMapTextureIdentifer"), new GUIContent("TextureName"), GUILayout.Height(19));
                        EditorGUILayout.PropertyField(settings.FindProperty("specularMapShaderIdentifer"), new GUIContent("ShaderParameter"), GUILayout.Height(19));
                    }

                    settings.ApplyModifiedPropertiesWithoutUndo();
                },

                // 検索する時のキーワード
                keywords = new HashSet<string>(new[] { "MaterialCreator" })
            };

            return provider;
        }
    }
}
