using MasterMemory;
using MessagePack;

[MemoryTable("m_item"), MessagePackObject(true)]
public class ItemData
{
    /// <summary>
    /// アイテムID
    /// </summary>
    [PrimaryKey]
    public int Id { get; set; }
    /// <summary>
    /// アイテム名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// アイテムの説明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// アイテムの種類
    /// </summary>
    public int Category {  get; set; }

    /// <summary>
    /// アイテムのパス
    /// </summary>
    public string Path {  get; set; }

    /// <summary>
    /// アイテムの効果値1
    /// </summary>
    public int EffectValue1 {  get; set; }

    /// <summary>
    /// アイテムの効果値2
    /// </summary>
    public int EffectValue2 { get; set; }

    /// <summary>
    /// アイテムの効果値3
    /// </summary>
    public int EffectValue3 { get; set; }

    public ItemData(int Id, string Name, string Description, int Category, string Path, int EffectValue1, int EffectValue2, int EffectValue3)
    {
        this.Id = Id;
        this.Name = Name;
        this.Description = Description;
        this.Category = Category;
        this.Path = Path;
        this.EffectValue1 = EffectValue1;
        this.EffectValue2 = EffectValue2;
        this.EffectValue3 = EffectValue3;
    }
}
