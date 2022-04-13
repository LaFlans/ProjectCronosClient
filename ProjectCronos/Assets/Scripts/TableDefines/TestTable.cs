using MasterMemory;
using MessagePack;

/// <summary>
/// テストテーブル
/// </summary>
[MemoryTable("m_test"), MessagePackObject]
public partial class TestTable
{
    [Key(0)]
    [PrimaryKey] 
    public int Id { get; set; }

    [Key(1)]
    public string Name { get; set; }

    public TestTable(int Id, string Name)
    {
        this.Id = Id;
        this.Name = Name;
    }
}