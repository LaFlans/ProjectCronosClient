using MasterMemory;
using MessagePack;

[MemoryTable("m_player_data"), MessagePackObject(true)]
public class PlayerData
{
    [PrimaryKey]
    public string Key { get; set; }
    public int MaxHp { get; set; }
    public int TimeHpHealPerSeconds { get; set; }
    public int MaxMp { get; set; }
    public int TimeMpHealPerSeconds { get; set; }
    public int Attack { get; set; }
    public int MagicAttack { get; set; }
    public int Defense { get; set; }
    public int MagicDefense { get; set; }
    public int CriticalRate { get; set; }
    public int CriticalDamageRate { get; set; }
    public float MoveSpeed { get; set; }
    public float JumpPower { get; set; }

    public PlayerData(string Key, int MaxHp, int TimeHpHealPerSeconds, int MaxMp, int TimeMpHealPerSeconds, int Attack, int MagicAttack, int Defense, int MagicDefense, int CriticalRate, int CriticalDamageRate, float MoveSpeed, float JumpPower)
    {
        this.Key = Key;
        this.MaxHp = MaxHp;
        this.TimeHpHealPerSeconds = TimeHpHealPerSeconds;
        this.MaxMp = MaxMp;
        this.TimeMpHealPerSeconds = TimeMpHealPerSeconds;
        this.Attack = Attack;
        this.MagicAttack = MagicAttack;
        this.Defense = Defense;
        this.MagicDefense = MagicDefense;
        this.CriticalRate = CriticalRate;
        this.CriticalDamageRate = CriticalDamageRate;
        this.MoveSpeed = MoveSpeed;
        this.JumpPower = JumpPower;
    }
}
