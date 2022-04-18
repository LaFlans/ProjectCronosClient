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
    public abstract void Save(DatabaseBuilder builder);
    public abstract List<string> GetMasterDataDebugMessage();

    protected static MemoryDatabase db;

    protected void Load()
    {
        // MessagePackÇÃResolverÇê›íË
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
