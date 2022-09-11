using EgoParadise.Unity.Utility.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Rapilias.Editor
{
    public class ComponentCopyWindow : EditorWindow
    {
        public class SelectingInfo
        {
            public Component component { get; set; } = null;
            public bool enableCopy { get; set; } = true;
            public bool isOverride { get; set; } = false;
        }

        public List<SelectingInfo> selectingInfo = new List<SelectingInfo>();
        public List<SelectingInfo> readyInfo = new List<SelectingInfo>();

        [MenuItem("Rapilias/Window/ComponentCopy")]
        private static void Create()
        {
            var window = GetWindow<ComponentCopyWindow>();
            window.titleContent = new GUIContent("Component Copy");
            Selection.selectionChanged += () =>
            {
                window.OnCopyTargetChanged();
                window.Repaint();
            };
        }

        private void OnGUI()
        {
            var isSelecting = Selection.activeGameObject != null;
            using (var scope = new EditorDisableGroup(true))
            {
                EditorGUILayout.ObjectField("From: ", Selection.activeGameObject, typeof(GameObject), true);
            }

            this.SelectUnselectGUI();
            this.ComponentInfoListGUI(isSelecting, this.selectingInfo);

            using (var scope = new EditorDisableGroup(!isSelecting))
            {
                if (GUILayout.Button("Copy"))
                    this.CopySelectedComponents();
            }

            using (var scope = new EditorDisableGroup(true))
            {
                var item = this.readyInfo.FirstOrDefault();
                if (item != null)
                    EditorGUILayout.ObjectField("Copy From: ", item.component.gameObject, typeof(GameObject), true);
            }
            this.ComponentInfoListGUI(true, this.readyInfo);

            using (var scope = new EditorDisableGroup(!isSelecting))
            {
                if (GUILayout.Button("Paste"))
                    this.PasteSelectedComponents();
            }
        }

        private void SelectUnselectGUI()
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Select All"))
                {
                    foreach (var item in this.selectingInfo)
                        item.enableCopy = true;
                }
                if (GUILayout.Button("UnSelect All"))
                {
                    foreach (var item in this.selectingInfo)
                        item.enableCopy = false;
                }
            }
        }

        private void ComponentInfoListGUI(bool isSelecting, IEnumerable<SelectingInfo> infos)
        {
            if (isSelecting)
            {
                var noSpaceBox = new GUIStyle(GUI.skin.box)
                {
                    padding =
                    {
                        top = 0,
                        bottom = 0,
                    },
                    margin =
                    {
                        top = 0,
                        bottom = 0,
                    },
                };

                foreach (var item in infos)
                {
                    using var row = new GUILayout.HorizontalScope(noSpaceBox);
                    ComponentImageBox(item.component, size: 16);
                    item.enableCopy = GUILayout.Toggle(item.enableCopy, item.component.GetType().Name);
                }
            }
        }

        private void OnCopyTargetChanged()
        {
            this.selectingInfo.Clear();
            if (Selection.activeGameObject is null)
                return;

            var targets = Selection.activeGameObject.GetComponents<Component>();
            foreach (var item in targets)
            {
                this.selectingInfo.Add(
                    new SelectingInfo
                    {
                        component = item,
                        enableCopy = true,
                        isOverride = true,
                    }
                );
            }
        }

        private void CopySelectedComponents()
        {
            this.readyInfo.Clear();
            foreach (var item in this.selectingInfo.Where(m => m.enableCopy))
            {
                this.readyInfo.Add(
                    new SelectingInfo
                    {
                        component = item.component,
                        enableCopy = true,
                        isOverride = item.isOverride,
                    }
                );
            }
        }

        private void PasteSelectedComponents()
        {
            if (Selection.activeGameObject is null) return;

            var pastetarget = Selection.activeGameObject;

            foreach (var item in this.readyInfo)
            {
                if (item.enableCopy is false)
                    continue;

                ComponentUtility.CopyComponent(item.component);
                if (item.isOverride)
                {
                    var component = pastetarget.GetComponent(item.component.GetType());
                    if (component == null)
                        ComponentUtility.PasteComponentAsNew(pastetarget);
                    else
                        ComponentUtility.PasteComponentValues(item.component);
                }
                else
                    ComponentUtility.PasteComponentAsNew(pastetarget);
            }
        }

        public static void ComponentImageBox(Component component, int size)
        {
            var content = AssetPreview.GetMiniThumbnail(component);
            ImageBox(content, size);
        }

        public static void ImageBox(Texture2D texture, int size = 16)
        {
            var options = new[]
            {
                GUILayout.Width(size),
                GUILayout.Height(size),
            };
            EditorGUILayout.LabelField(new GUIContent(texture), options);
        }
    }
}
