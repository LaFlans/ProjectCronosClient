using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EgoParadise.Utility.Unity.Editor
{
    [CreateAssetMenu(fileName = "ScenePresetAsset", menuName = "EgoParadise/Scene/ScenePresetAsset")]
    public class ScenePresetAsset : ScriptableObject
    {
        public List<ScenePresetElement> elements = new List<ScenePresetElement>();
    }

    [Serializable]
    public class ScenePresetElement
    {
        public string presetName;
        public List<SceneAsset> scenes = new List<SceneAsset>();
    }
}
