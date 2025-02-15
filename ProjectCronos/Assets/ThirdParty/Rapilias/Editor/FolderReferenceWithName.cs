using System;
using UnityEngine;

namespace EgoParadise.Utility.Unity.Editor
{
    [Serializable]
    public class FolderReferenceWithName
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public FolderReference Folder;
    }
}
