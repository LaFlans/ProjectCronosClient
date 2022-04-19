using MasterMemory;
using MessagePack;

[MemoryTable("m_dictionary"), MessagePackObject(true)]
public class Dictionary
{
    [PrimaryKey]
    public string Key { get; set; }
    public string Message { get; set; }

    public Dictionary(string Key, string Message)
    {
        this.Key = Key;
        this.Message = Message;
    }
}