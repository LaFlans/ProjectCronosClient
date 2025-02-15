using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EgoParadise.Utility.Unity.Editor
{
    public class PresetAndNavigationWindow : EditorWindow
    {
        private ScenePresetAsset _sceneAsset = null;
        private ProjectFolderNavigationAsset _folderAsset = null;

        [MenuItem("Tools/EgoParadise/Window/PresetAndNavigation")]
        public static void Create()
        {
            var window = GetWindow<PresetAndNavigationWindow>();
            window.titleContent = new GUIContent("PresetAndNavigation");
        }

        private void OnGUI()
        {
            this._sceneAsset ??= FindAllByType<ScenePresetAsset>()
                .FirstOrDefault();
            this._sceneAsset = EditorGUILayout.ObjectField("ScenesAsset: ", this._sceneAsset, typeof(ScenePresetAsset), false) as ScenePresetAsset;
            if (this._sceneAsset == null)
                return;

            foreach (var item in this._sceneAsset.elements)
            {
                if (item == null || item.scenes == null)
                    continue;
                var _ = EditorGUILayout.BeginHorizontal();
                var sceneList = string.Join(',', item.scenes.Select(m => m).Where(m => m != null).Select(m => m.name));
                EditorGUILayout.LabelField($"{item.presetName}: [{sceneList}]");

                if (GUILayout.Button("Open"))
                {
                    this.Open(item);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            this._folderAsset ??= FindAllByType<ProjectFolderNavigationAsset>()
                .FirstOrDefault();
            this._folderAsset = EditorGUILayout.ObjectField("ProjectFoldersAsset: ", this._folderAsset, typeof(ProjectFolderNavigationAsset), false) as ProjectFolderNavigationAsset;
            if (this._folderAsset == null)
                return;

            foreach (var item in this._folderAsset.Folders)
            {
                if (item == null || item.Folder == null || string.IsNullOrEmpty(item.Folder.Guid))
                    continue;
                var _ = EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"[{item.Name}]");
                if (GUILayout.Button("Select"))
                {
                    var folderAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(item.Folder.Guid));
                    EditorUtility.FocusProjectWindow();
                    AssetDatabase.OpenAsset(folderAsset);
                    var mainThread = SynchronizationContext.Current;
                    Task.Run(
                        async () =>
                        {
                            await Task.Delay(10);
                            mainThread.Post((m) => { AssetDatabase.OpenAsset(folderAsset); }, null);
                        }
                    );
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void Open(ScenePresetElement sceneAsset)
        {
            var isInitial = false;
            foreach (var item in sceneAsset.scenes)
            {
                if (item == null)
                    continue;
                var hasSubFileInstance = AssetDatabase.TryGetGUIDAndLocalFileIdentifier(item.GetInstanceID(), out var guid2, out long _);
                if (hasSubFileInstance == false)
                    throw new InvalidOperationException();
                if (isInitial == false)
                {
                    _ = EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(guid2), OpenSceneMode.Single);
                    isInitial = true;
                    continue;
                }
                _ = EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(guid2), OpenSceneMode.Additive);
            }
        }

        public static IEnumerable<T> FindAllByType<T>() where T : UnityEngine.Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).FullName}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                yield return AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
            }
        }
    }
}

