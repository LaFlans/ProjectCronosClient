using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MessagePack.Resolvers;
using Generated;
using MessagePack;
using UnityEditor;
using UnityEngine;

namespace ProjectCronos
{
    public abstract class MasterDataScriptableObject : Editor
    {
        public const string colorCodeYellow = "#FFF100";
        public const string colorCodeRed = "#FF4B00";
        public const string colorCodeBlue = "#005AFF";

        public abstract void Save(DatabaseBuilder builder);

        /// <summary>
        /// データのタイトル取得
        /// </summary>
        /// <returns></returns>
        public string GetMasterDataTitle() => dataTitle;

        /// <summary>
        /// マスタデータの変更差分のみをデバックとしてメッセージで取得
        /// </summary>
        /// <param name="isShowBefore">変更前の値も表示するか</param>
        /// <param name="isShowAllData">変更のない値も表示するか</param>
        /// <param name="existsDiff">差分が存在するか</param>
        public abstract List<string> GetMasterDataDiffDebugMessage(bool isShowBefore, bool isShowAllData, out bool existsDiff);

        /// <summary>
        /// DBのデータの中身をデバックとしてメッセージで取得
        /// </summary>
        public abstract List<string> GetMasterDataDebugMessage();

        /// <summary>
        /// DBキャッシュ更新処理
        /// </summary>
        public abstract void UpdateDBCache();

        protected static MemoryDatabase db;
        protected string dataTitle = String.Empty;

        protected void Load()
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

            string path = $"{Application.dataPath}/MasterData/Generated/master-data.bytes";
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader bin = new BinaryReader(stream);
            db = new MemoryDatabase(bin.ReadBytes((int)bin.BaseStream.Length));
            stream.Close();
        }
    }
}
