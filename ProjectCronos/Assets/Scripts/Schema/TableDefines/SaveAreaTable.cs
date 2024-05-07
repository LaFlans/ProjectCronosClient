using MasterMemory;
using MessagePack;

[MemoryTable("m_save_area_data"), MessagePackObject(true)]
public class SaveAreaData
{
    /// <summary>
    /// セーブエリアID(重複不可)
    /// </summary>
    [PrimaryKey]
    public int Id { get; set; }

    /// <summary>
    /// セーブエリア名辞書キー
    /// </summary>
    public string SaveAreaNameDicKey { get; set; }

    public SaveAreaData(int Id, string SaveAreaNameDicKey)
    {
        this.Id = Id;
        this.SaveAreaNameDicKey = SaveAreaNameDicKey;
    }
}
