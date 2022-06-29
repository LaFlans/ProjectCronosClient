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

public abstract class MasterDataScriptableObject : ScriptableObject
{
    public const string colorCodeYellow = "#ffff00";
    public const string colorCodeRed = "#ff0000";
    public const string colorCodeBlue = "#0000ff";

    public abstract void Save(DatabaseBuilder builder);

    /// <summary>
    /// DBのデータの中身をデバックとしてメッセージで取得
    /// </summary>
    public abstract List<string> GetMasterDataDebugMessage();

    /// <summary>
    /// マスタデータの変更差分のみをデバックとしてメッセージで取得
    /// </summary>
    /// <param name="isShowBefore">変更前の値も表示するか</param>
    public abstract List<string> GetMasterDataDiffDebugMessage(bool isShowBefore);

    protected static MemoryDatabase db;

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
