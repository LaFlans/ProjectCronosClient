using System.IO;
using MessagePack.Resolvers;
// MasterMemory.Generatorにて生成時にNameSpaceをGeneratedで指定
using Generated;
using MessagePack;
using UnityEditor;
using UnityEngine;

namespace ProjectCronos
{
    public static class MasterDataGenerator
    {
#if UNITY_EDITOR
        [MenuItem("MasterMemory/MasterDataGenerator")]
        static void BuildMasterData()
        {
            // MessagePackのResolverを設定
            try
            {
                StaticCompositeResolver.Instance.Register
                (
                    new IFormatterResolver[]
                    {
                    MasterMemoryResolver.Instance,
                    GeneratedResolver.Instance,
                    StandardResolver.Instance,
                    });
                var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
                MessagePackSerializer.DefaultOptions = options;
            }
            catch
            {
            }

            // 本題のMasterデータ作成はこちら
            var builder = new DatabaseBuilder();
            builder.Append(new TestTable[]
            {
                new TestTable(Id: 1,Name: "フシギダネ"),
                new TestTable(Id: 2, Name: "リザードン"),
            });

            byte[] data = builder.Build();
            Debug.Log("ConvertToJson : " + MessagePackSerializer.ConvertToJson(data));
            var resourcesDir = $"{Application.dataPath}/Resources";
            Directory.CreateDirectory(resourcesDir);
            var filename = "/master-data.bytes";

            using (var fs = new FileStream(resourcesDir + filename, FileMode.Create))
            {
                fs.Write(data, 0, data.Length);
            }

            Debug.Log($"Write byte[] to: {resourcesDir + filename}");

            AssetDatabase.Refresh();
        }
#endif
    }
}
