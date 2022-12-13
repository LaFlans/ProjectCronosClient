using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using Generated;

namespace ProjectCronos
{
    [CreateAssetMenu(menuName = "MasterData/Create EnemyData", fileName = "EnemyData")]
    internal class EnemyDataScriptableObject : MasterDataScriptableObject
    {
        [Serializable]
        public class EnemyScriptableData
        {
            public string Key;
            public string Name;
            public int MaxHp;
            public int TimeHpHealPerSeconds;
            public int MaxMp;
            public int TimeMpHealPerSeconds;
            public int Attack;
            public int MagicAttack;
            public int Defense;
            public int MagicDefense;
            public float AiThinkingIntervalTime;
            public float AttackDist;
            public float SearchDist;
            public float MoveSpeed;

            public EnemyScriptableData(
                string Key,
                string Name,
                int MaxHp,
                int TimeHpHealPerSeconds,
                int MaxMp,
                int TimeMpHealPerSeconds,
                int Attack,
                int MagicAttack,
                int Defense,
                int MagicDefense,
                float AiThinkingIntervalTime,
                float AttackDist,
                float SearchDist,
                float MoveSpeed)
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

        [SerializeField]
        private List<EnemyScriptableData> data = new List<EnemyScriptableData>();
        private List<EnemyData> dbData = new List<EnemyData>();

        void OnEnable()
        {
            // データのタイトル設定
            dataTitle = "<b>EnemyDataMasterData</b>";
        }

        public override void UpdateDBCache()
        {
            // DB読み込み
            Load();

            // DBのデータ更新
            dbData = db.EnemyDataTable.All.ToList();

            data.Clear();
            foreach (var item in dbData)
            {
                data.Add(new EnemyScriptableData(
                    item.Key,
                    item.Name,
                    item.MaxHp,
                    item.TimeHpHealPerSeconds,
                    item.MaxMp,
                    item.TimeMpHealPerSeconds,
                    item.Attack,
                    item.MagicAttack,
                    item.Defense,
                    item.MagicDefense,
                    item.AiThinkingIntervalTime,
                    item.AttackDist,
                    item.SearchDist,
                    item.MoveSpeed));
            }
        }

        public override void Save(DatabaseBuilder builder)
        {
            List<EnemyData> temp = new List<EnemyData>();
            foreach (var item in data.Select((v, i) => new { Value = v, Index = i }))
            {
                temp.Add(new EnemyData(
                    item.Value.Key,
                    item.Value.Name,
                    item.Value.MaxHp,
                    item.Value.TimeHpHealPerSeconds,
                    item.Value.MaxMp,
                    item.Value.TimeMpHealPerSeconds,
                    item.Value.Attack,
                    item.Value.MagicAttack,
                    item.Value.Defense,
                    item.Value.MagicDefense,
                    item.Value.AiThinkingIntervalTime,
                    item.Value.AttackDist,
                    item.Value.SearchDist,
                    item.Value.MoveSpeed));
            }
            builder.Append(temp);
        }

        public override List<string> GetMasterDataDiffDebugMessage(bool isShowBefore, bool isShowAllData, out bool existsDiff)
        {
            List<string> messages = new List<string>();
            var sb = new StringBuilder();

            // 差分存在チェック初期化
            existsDiff = false;

            foreach (var item in dbData.Select((v, i) => new { Value = v, Index = i }))
            {
                // 存在している要素で比較して表示
                if (data.Count > item.Index)
                {
                    if (item.Value.Key == data[item.Index].Key &&
                        item.Value.Name == data[item.Index].Name &&
                        item.Value.MaxHp == data[item.Index].MaxHp &&
                        item.Value.TimeHpHealPerSeconds == data[item.Index].TimeHpHealPerSeconds &&
                        item.Value.MaxMp == data[item.Index].MaxMp &&
                        item.Value.TimeMpHealPerSeconds == data[item.Index].TimeMpHealPerSeconds &&
                        item.Value.Attack == data[item.Index].Attack &&
                        item.Value.MagicAttack == data[item.Index].MagicAttack &&
                        item.Value.Defense == data[item.Index].Defense &&
                        item.Value.MagicDefense == data[item.Index].MagicDefense &&
                        item.Value.AiThinkingIntervalTime == data[item.Index].AiThinkingIntervalTime &&
                        item.Value.AttackDist == data[item.Index].AttackDist &&
                        item.Value.SearchDist == data[item.Index].SearchDist &&
                        item.Value.MoveSpeed == data[item.Index].MoveSpeed)
                    {
                        // すべてのデータを表示しない設定の時、変更差分がない場合、何もしない
                        if (!isShowAllData)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        // 差分存在チェック
                        if (!existsDiff)
                        {
                            existsDiff = true;
                        }
                    }

                    sb.Clear();

                    if (isShowBefore)
                    {
                        sb.Append("KEY:" + (item.Value.Key == data[item.Index].Key ?
                            $"{data[item.Index].Key} " :
                            $"KEY:{item.Value.Key}→<color={colorCodeYellow}>{data[item.Index].Key}</color> "));
                        sb.Append("NAME:" + (item.Value.Name == data[item.Index].Name ?
                            $"{data[item.Index].Name} " :
                            $"NAME:{item.Value.Name}→<color={colorCodeYellow}>{data[item.Index].Name}</color> "));
                        sb.Append("MAX_HP:" + (item.Value.MaxHp == data[item.Index].MaxHp ?
                            $"{data[item.Index].MaxHp} " :
                            $"{item.Value.MaxHp}→<color={colorCodeYellow}>{data[item.Index].MaxHp}</color> "));
                        sb.Append("TIME_HP_HEAL_PER_SECONDS:" + (item.Value.TimeHpHealPerSeconds == data[item.Index].TimeHpHealPerSeconds ?
                            $"{data[item.Index].TimeHpHealPerSeconds} " :
                            $"{item.Value.TimeHpHealPerSeconds}→<color={colorCodeYellow}>{data[item.Index].TimeHpHealPerSeconds}</color> "));
                        sb.Append("MAX_MP:" + (item.Value.MaxMp == data[item.Index].MaxMp ?
                            $"{data[item.Index].MaxMp} " :
                            $"{item.Value.MaxMp}→<color={colorCodeYellow}>{data[item.Index].MaxMp}</color> "));
                        sb.Append("TIME_MP_HEAL_PER_SECONDS:" + (item.Value.TimeMpHealPerSeconds == data[item.Index].TimeMpHealPerSeconds ?
                            $"{data[item.Index].TimeMpHealPerSeconds} " :
                            $"{item.Value.TimeMpHealPerSeconds}→<color={colorCodeYellow}>{data[item.Index].TimeMpHealPerSeconds}</color> "));
                        sb.Append("ATTACK:" + (item.Value.Attack == data[item.Index].Attack ?
                            $"{data[item.Index].Attack} " :
                            $"{item.Value.Attack}→<color={colorCodeYellow}>{data[item.Index].Attack}</color> "));
                        sb.Append("MAGIC_ATTACK:" + (item.Value.MagicAttack == data[item.Index].MagicAttack ?
                            $"{data[item.Index].MagicAttack} " :
                            $"{item.Value.MagicAttack}→<color={colorCodeYellow}>{data[item.Index].MagicAttack}</color> "));
                        sb.Append("DEFENSE:" + (item.Value.Defense == data[item.Index].Defense ?
                            $"{data[item.Index].Defense} " :
                            $"{item.Value.Defense}→<color={colorCodeYellow}>{data[item.Index].Defense}</color> "));
                        sb.Append("MAGIC_DEFENSE:" + (item.Value.MagicDefense == data[item.Index].MagicDefense ?
                            $"{data[item.Index].MagicDefense} " :
                            $"{item.Value.MagicDefense}→<color={colorCodeYellow}>{data[item.Index].MagicDefense}</color> "));
                        sb.Append("AI_THINKING_INTERVAL_TIME:" + (item.Value.AiThinkingIntervalTime == data[item.Index].AiThinkingIntervalTime ?
                            $"{data[item.Index].AiThinkingIntervalTime} " :
                            $"{item.Value.AiThinkingIntervalTime}→<color={colorCodeYellow}>{data[item.Index].AiThinkingIntervalTime}</color> "));
                        sb.Append("ATTACK_DIST:" + (item.Value.AttackDist == data[item.Index].AttackDist ?
                            $"{data[item.Index].AttackDist} " :
                            $"{item.Value.AttackDist}→<color={colorCodeYellow}>{data[item.Index].AttackDist}</color> "));
                        sb.Append("SEARCH_DIST:" + (item.Value.SearchDist == data[item.Index].SearchDist ?
                            $"{data[item.Index].SearchDist} " :
                            $"{item.Value.SearchDist}→<color={colorCodeYellow}>{data[item.Index].SearchDist}</color> "));
                        sb.Append("MOVE_SPEED:" + (item.Value.MoveSpeed == data[item.Index].MoveSpeed ?
                            $"{data[item.Index].MoveSpeed} " :
                            $"{item.Value.MoveSpeed}→<color={colorCodeYellow}>{data[item.Index].MoveSpeed}</color> "));
                    }
                    else
                    {
                        sb.Append("KEY:" + (item.Value.Key == data[item.Index].Key ?
                            $"{data[item.Index].Key} " :
                            $"<color={colorCodeYellow}>{data[item.Index].Key}</color> "));
                        sb.Append("NAME:" + (item.Value.Name == data[item.Index].Name ?
                            $"{data[item.Index].Name} " :
                            $"<color={colorCodeYellow}>{data[item.Index].Name}</color> "));
                        sb.Append("MAX_HP:" + (item.Value.MaxHp == data[item.Index].MaxHp ?
                            $"{data[item.Index].MaxHp} " :
                            $"<color={colorCodeYellow}>{data[item.Index].MaxHp}</color> "));
                        sb.Append("TIME_HP_HEAL_PER_SECONDS:" + (item.Value.TimeHpHealPerSeconds == data[item.Index].TimeHpHealPerSeconds ?
                            $"{data[item.Index].TimeHpHealPerSeconds} " :
                            $"<color={colorCodeYellow}>{data[item.Index].TimeHpHealPerSeconds}</color> "));
                        sb.Append("MAX_MP:" + (item.Value.MaxMp == data[item.Index].MaxMp ?
                            $"{data[item.Index].MaxMp} " :
                            $"<color={colorCodeYellow}>{data[item.Index].MaxMp}</color> "));
                        sb.Append("TIME_MP_HEAL_PER_SECONDS:" + (item.Value.TimeMpHealPerSeconds == data[item.Index].TimeMpHealPerSeconds ?
                            $"{data[item.Index].TimeMpHealPerSeconds} " :
                            $"<color={colorCodeYellow}>{data[item.Index].TimeMpHealPerSeconds}</color> "));
                        sb.Append("ATTACK:" + (item.Value.Attack == data[item.Index].Attack ?
                            $"{data[item.Index].Attack} " :
                            $"<color={colorCodeYellow}>{data[item.Index].Attack}</color> "));
                        sb.Append("MAGIC_ATTACK:" + (item.Value.MagicAttack == data[item.Index].MagicAttack ?
                            $"{data[item.Index].MagicAttack} " :
                            $"<color={colorCodeYellow}>{data[item.Index].MagicAttack}</color> "));
                        sb.Append("DEFENSE:" + (item.Value.Defense == data[item.Index].Defense ?
                            $"{data[item.Index].Defense} " :
                            $"<color={colorCodeYellow}>{data[item.Index].Defense}</color> "));
                        sb.Append("MAGIC_DEFENSE:" + (item.Value.MagicDefense == data[item.Index].MagicDefense ?
                            $"{data[item.Index].MagicDefense} " :
                            $"<color={colorCodeYellow}>{data[item.Index].MagicDefense}</color> "));
                        sb.Append("AI_THINKING_INTERVAL_TIME:" + (item.Value.AiThinkingIntervalTime == data[item.Index].AiThinkingIntervalTime ?
                            $"{data[item.Index].AiThinkingIntervalTime} " :
                            $"<color={colorCodeYellow}>{data[item.Index].AiThinkingIntervalTime}</color> "));
                        sb.Append("ATTACK_DIST:" + (item.Value.AttackDist == data[item.Index].AttackDist ?
                            $"{data[item.Index].AttackDist} " :
                            $"<color={colorCodeYellow}>{data[item.Index].AttackDist}</color> "));
                        sb.Append("SEARCH_DIST:" + (item.Value.SearchDist == data[item.Index].SearchDist ?
                            $"{data[item.Index].SearchDist} " :
                            $"<color={colorCodeYellow}>{data[item.Index].SearchDist}</color> "));
                        sb.Append("MOVE_SPEED:" + (item.Value.MoveSpeed == data[item.Index].MoveSpeed ?
                            $"{data[item.Index].MoveSpeed} " :
                            $"<color={colorCodeYellow}>{data[item.Index].MoveSpeed}</color> "));
                    }

                    messages.Add(sb.ToString());

                    continue;
                }

                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                // ScriptableObject側の要素が少ない場合、青で表示
                messages.Add($"-<color={colorCodeBlue}>KEY:{item.Value.Key} NAME:{item.Value.Name} MAX_HP:{item.Value.MaxHp} TIME_HP_HEAL_PER_SECONDS:{item.Value.TimeHpHealPerSeconds} MAX_MP:{item.Value.MaxMp} TIME_MP_HEAL_PER_SECONDS:{item.Value.TimeMpHealPerSeconds} ATTACK:{item.Value.Attack} MAGIC_ATTACK:{item.Value.MagicAttack} DEFENSE:{item.Value.Defense} MAGIC_DEFENSE:{item.Value.MagicDefense} AI_THINKING_INTERVAL_TIME:{item.Value.AiThinkingIntervalTime} ATTACK_DIST:{item.Value.AttackDist} SEARCH_DIST:{item.Value.SearchDist} MOVE_SPEED:{item.Value.MoveSpeed}</color>");
            }

            // ScriptableObject側の要素が多い場合、赤で表示
            if (dbData.Count < data.Count)
            {
                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                for (int i = dbData.Count; i < data.Count; i++)
                {
                    messages.Add($"+<color={colorCodeRed}>KEY:{data[i].Key} NAME:{data[i].Name} MAX_HP:{data[i].MaxHp} TIME_HP_HEAL_PER_SECONDS:{data[i].TimeHpHealPerSeconds} MAX_MP:{data[i].MaxMp} TIME_MP_HEAL_PER_SECONDS:{data[i].TimeMpHealPerSeconds} ATTACK:{data[i].Attack} MAGIC_ATTACK:{data[i].MagicAttack} DEFENSE:{data[i].Defense} MAGIC_DEFENSE:{data[i].MagicDefense} AI_THINKING_INTERVAL_TIME:{data[i].AiThinkingIntervalTime} ATTACK_DIST:{data[i].AttackDist} SEARCH_DIST:{data[i].SearchDist} MOVE_SPEED:{data[i].MoveSpeed}</color>");
                }
            }

            return messages;
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> debugMessage = new List<string>();
            foreach (var item in dbData)
            {
                debugMessage.Add($"KEY:{item.Key} NAME:{item.Name} MAX_HP:{item.MaxHp} TIME_HP_HEAL_PER_SECONDS:{item.TimeHpHealPerSeconds} MAX_MP:{item.MaxMp} TIME_MP_HEAL_PER_SECONDS:{item.TimeMpHealPerSeconds} ATTACK:{item.Attack} MAGIC_ATTACK:{item.MagicAttack} DEFENSE:{item.Defense} MAGIC_DEFENSE:{item.MagicDefense} AI_THINKING_INTERVAL_TIME:{item.AiThinkingIntervalTime} ATTACK_DIST:{item.AttackDist} SEARCH_DIST:{item.SearchDist} MOVE_SPEED:{item.MoveSpeed}");
            }

            return debugMessage;
        }
    }
}
