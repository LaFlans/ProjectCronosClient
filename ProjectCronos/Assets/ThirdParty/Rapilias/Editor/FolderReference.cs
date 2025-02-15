using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EgoParadise.Utility.Unity.Editor
{
    [Serializable]
    public class FolderReference
    {
        [SerializeField]
        public string Guid = string.Empty;
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(FolderReference))]
    public class FolderReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var guid = property.FindPropertyRelative("Guid");
            var loadedPath = string.Empty;
            try
            {
                loadedPath = AssetDatabase.GUIDToAssetPath(guid.stringValue);
            }
            catch (Exception)
            {
                // ignored
            }

            var obj = AssetDatabase.AssetPathExists(loadedPath) ? AssetDatabase.LoadAssetAtPath<Object>(loadedPath) : null;
            var guiContent = EditorGUIUtility.ObjectContent(obj, type: typeof(DefaultAsset));
            var r = EditorGUI.PrefixLabel(position, label);

            var textFieldRect = r;
            textFieldRect.width -= 19f;

            var textFieldStyle = new GUIStyle("TextField")
            {
                imagePosition = obj
                    ? ImagePosition.ImageLeft
                    : ImagePosition.TextOnly,
            };

            if (GUI.Button(textFieldRect, guiContent, textFieldStyle) && obj)
                EditorGUIUtility.PingObject(obj);

            if (textFieldRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    var reference = DragAndDrop.objectReferences[0];
                    var path = AssetDatabase.GetAssetPath(reference);
                    DragAndDrop.visualMode = Directory.Exists(path)
                        ? DragAndDropVisualMode.Copy
                        : DragAndDropVisualMode.Rejected;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragPerform)
                {
                    var reference = DragAndDrop.objectReferences[0];
                    var path = AssetDatabase.GetAssetPath(reference);
                    if (Directory.Exists(path))
                    {
                        // obj = reference;
                        guid.stringValue = AssetDatabase.AssetPathToGUID(path);
                    }
                    Event.current.Use();
                }
            }

            var objectFieldRect = r;
            objectFieldRect.x = textFieldRect.xMax + 1f;
            objectFieldRect.width = 19f;

            if (GUI.Button(objectFieldRect, "", style: GUI.skin.GetStyle("IN ObjectField")))
            {
                var path = EditorUtility.OpenFolderPanel("Select a folder", "Assets", "");
                if (path.Contains(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                    obj = AssetDatabase.LoadAssetAtPath(path, type: typeof(DefaultAsset));
                    guid.stringValue = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
                }
                else
                {
                }
            }
        }
    }
#endif
}
