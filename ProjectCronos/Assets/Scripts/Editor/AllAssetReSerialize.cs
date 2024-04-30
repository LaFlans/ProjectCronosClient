using UnityEditor;

namespace ProjectCronos
{

    public static class AllAssetReSerialize
    {
        /// <summary>
        /// すべてのアセットを再読み込みする
        /// ただし、めちゃくちゃ時間がかかるので注意が必要
        /// </summary>
        [MenuItem("Cronos/AllAssetReSerialize")]
        public static void Execute()
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}
