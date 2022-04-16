//using System.IO;
//using MessagePack.Resolvers;
//// MasterMemory.Generatorにて生成時にNameSpaceをGeneratedで指定
//using Generated;
//using MessagePack;
//using UnityEditor;
//using UnityEngine;

//namespace ProjectCronos
//{
//    public static class MasterDataGenerator
//    {
//#if UNITY_EDITOR
//        [MenuItem("MasterMemory/MasterDataGenerator")]
//        static void BuildMasterData()
//        {
//            // MessagePackのResolverを設定
//            try
//            {
//                StaticCompositeResolver.Instance.Register
//                (
//                    new IFormatterResolver[]
//                    {
//                    MasterMemoryResolver.Instance,
//                    GeneratedResolver.Instance,
//                    StandardResolver.Instance,
//                    });
//                var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
//                MessagePackSerializer.DefaultOptions = options;
//            }
//            catch
//            {
//            }

//            // 本題のMasterデータ作成はこちら
//            var builder = new DatabaseBuilder();
//            builder.Append(new TestTable[]
//            {
//                new TestTable(Id: 1,Name: "あああああああああああああああ"),
//                new TestTable(Id: 2, Name: "ううううううう"),
//                new TestTable(Id: 3, Name: "huuuuuuuuuuuuuuu!!!!!"),
//                new TestTable(Id: 4, Name: "ｻﾄｼ"),
//            });

//            //builder.Append(new SampleTable[]
//            //{
//            //    new SampleTable(Id: 1,Name: "あああああああああああああああ", path:"Assets/Resources_moved/Prefabs/PlayerInput.prefab"),
//            //    new SampleTable(Id: 2, Name: "ううううううう", path:"Assets/Resources_moved/Prefabs/PlayerInput.prefab"),
//            //    new SampleTable(Id: 3, Name: "huuuuuuuuuuuuuuu!!!!!", path:"Assets/Resources_moved/Prefabs/PlayerInput.prefab"),
//            //    new SampleTable(Id: 4, Name: "ｻﾄｼ", path:"Assets/Resources_moved/Prefabs/PlayerInput.prefab"),
//            //});

//            byte[] data = builder.Build();
//            Debug.Log("ConvertToJson : " + MessagePackSerializer.ConvertToJson(data));
//            var resourcesDir = $"{Application.dataPath}/MasterData";
//            Directory.CreateDirectory(resourcesDir);
//            var filename = "/master-data.bytes";

//            using (var fs = new FileStream(resourcesDir + filename, FileMode.Create))
//            {
//                fs.Write(data, 0, data.Length);
//            }

//            Debug.Log($"Write byte[] to: {resourcesDir + filename}");

//            AssetDatabase.Refresh();
//        }
//#endif
//    }
//}
