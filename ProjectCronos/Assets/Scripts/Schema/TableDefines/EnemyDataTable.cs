using MasterMemory;
using MessagePack;

[MemoryTable("m_enemy_data"), MessagePackObject(true)]
public class EnemyData
{
    [PrimaryKey]
    public string Key { get; set; }
    public string Name { get; set; }
    public int MaxHp { get; set; }
    public int TimeHpHealPerSeconds { get; set; }
    public int MaxMp { get; set; }
    public int TimeMpHealPerSeconds { get; set; }
    public int Attack { get; set; }
    public int MagicAttack { get; set; }
    public int Defense { get; set; }
    public int MagicDefense { get; set; }
    public float AiThinkingIntervalTime { get; set; }
    public float AttackDist { get; set; }
    public float SearchDist { get; set; }
    public float MoveSpeed { get; set; }

    public EnemyData(string Key, string Name, int MaxHp, int TimeHpHealPerSeconds, int MaxMp, int TimeMpHealPerSeconds, int Attack, int MagicAttack, int Defense, int MagicDefense, float AiThinkingIntervalTime, float AttackDist, float SearchDist, float MoveSpeed)
    {
        this.Key = Key;
        this.Name = Name;
        this.MaxHp = MaxHp;
        this.TimeHpHealPerSeconds = TimeHpHealPerSeconds;
        this.MaxMp = MaxMp;
        this.TimeMpHealPerSeconds = TimeMpHealPerSeconds;
        this.Attack = Attack;
        this.MagicAttack = MagicAttack;
        this.Defense = Defense;
        this.MagicDefense = MagicDefense;
        this.AiThinkingIntervalTime = AiThinkingIntervalTime;
        this.AttackDist = AttackDist;
        this.SearchDist = SearchDist;
        this.MoveSpeed = MoveSpeed;
    }
}
