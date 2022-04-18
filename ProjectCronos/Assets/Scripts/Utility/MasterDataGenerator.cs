using System.IO;
using MessagePack.Resolvers;
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

            builder.Append(new Test[]
            {
                new Test(Id: 1, Name: "あ",10,1.0f),
                new Test(Id: 2, Name: "い",20,2.0f),
                new Test(Id: 3, Name: "う",50,10),
                new Test(Id: 4, Name: "え",10000,9999),
            });

            builder.Append(new Sample[]
            {
                new Sample(Id: 1,Name: "か", path:"Assets/Resources_moved/Prefabs/PlayerInput.prefab"),
                new Sample(Id: 2, Name: "き", path:"Assets/Resources_moved/Prefabs/PlayerInput.prefab"),
                new Sample(Id: 3, Name: "く", path:"Assets/Resources_moved/Prefabs/PlayerInput.prefab"),
                new Sample(Id: 4, Name: "け", path:"Assets/Resources_moved/Prefabs/PlayerInput.prefab"),
                new Sample(Id: 5, Name: "こ", path:"Assets/Resources_moved/Prefabs/PlayerInput.prefab"),
            });

            byte[] data = builder.Build();
            Debug.Log("ConvertToJson : " + MessagePackSerializer.ConvertToJson(data));
            var resourcesDir = $"{Application.dataPath}/MasterData/Generated";
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
