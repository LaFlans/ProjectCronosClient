using MasterMemory;
using MessagePack;

[MemoryTable("sample"), MessagePackObject(true)]
public class Sample
{
    [PrimaryKey] 
    public int Id { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public Sample(int Id, string Name, string path)
    {
        this.Id = Id;
        this.Name = Name;
        this.Path = path;
    }
}