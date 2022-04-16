using MasterMemory;
using MessagePack;

[MemoryTable("test"), MessagePackObject(true)]
public class Test
{
    [PrimaryKey] public int Id { get; set; }

    public string Name { get; set; }

    public int hp { get; set; }
    public float attack { get; set; }

    public Test(int Id, string Name)
    {
        this.Id = Id;
        this.Name = Name;
    }

    public Test(int Id, string Name, int hp, float attack)
    {
        this.Id = Id;
        this.Name = Name;
        this.hp = hp;
        this.attack = attack;
    }
}