using MasterMemory;
using MessagePack;

[MemoryTable("m_test"), MessagePackObject(true)]
public class Test
{
    [PrimaryKey]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Hp { get; set; }
    public float Attack { get; set; }
    public float Deffence { get; set; }

    public Test(int Id, string Name, int Hp, float Attack, float Deffence)
    {
        this.Id = Id;
        this.Name = Name;
        this.Hp = Hp;
        this.Attack = Attack;
        this.Deffence = Deffence;
    }
}
