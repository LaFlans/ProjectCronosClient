using System;
using UnityEngine;

namespace EgoParadise.Utility.Unity.Editor
{
    [CreateAssetMenu(fileName = "FolderNavigationAsset", menuName = "EgoParadise/Scene/FolderNavigationAsset")]
    public class ProjectFolderNavigationAsset : ScriptableObject
    {
        [SerializeField]
        public FolderReferenceWithName[] Folders = Array.Empty<FolderReferenceWithName>();
    }
}
