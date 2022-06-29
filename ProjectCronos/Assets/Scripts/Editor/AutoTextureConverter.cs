using UnityEngine;
using UnityEditor;

namespace ProjectCronos
{
    public class AutoTextureConverter : AssetPostprocessor
    {
        ///// <summary>
        ///// テクスチャがプロジェクトにimportされる直前に呼び出される
        ///// </summary>
        //void OnPreprocessTexture()
        //{
        //    TextureImporter importer = assetImporter as TextureImporter;

        //    // FIXME: プロジェクト設定とかに文字列の設定を持っていく
        //    if (importer.name.Contains("_Normal"))
        //    {
        //        // テクスチャ名に指定の文字があった場合、テクスチャタイプを法線マップにする
        //        importer.textureType = TextureImporterType.NormalMap;
        //    }
        //}
    }
}
