using MasterMemory;
using MessagePack;

[MemoryTable("m_sound"), MessagePackObject(true)]
public class Sound
{
    /// <summary>
    /// キー
    /// </summary>
    [PrimaryKey]
    public string Key { get; set; }

    /// <summary>
    /// サウンドの種類(BGM or SE)
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// サウンドリソースのAddressablePath
    /// </summary>
    public string Path { get; set; }

    public Sound(string Key, int type, string Path)
    {
        this.Key = Key;
        this.Type = type;
        this.Path = Path;
    }
}