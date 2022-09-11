using System;
using UnityEditor;

namespace EgoParadise.Unity.Utility.Editor
{
    public class EditorDisableGroup : IDisposable
    {
        private bool _disable { get; } = false;

        public EditorDisableGroup(bool disable)
        {
            this._disable = disable;
            if (disable)
                EditorGUI.BeginDisabledGroup(true);
        }

        public void Dispose()
        {
            if (this._disable)
                EditorGUI.EndDisabledGroup();
        }
    }
}
