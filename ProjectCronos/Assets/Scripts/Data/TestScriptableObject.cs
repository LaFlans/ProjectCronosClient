using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using System.Linq;
using Generated;

namespace ProjectCronos
{
    [CreateAssetMenu(menuName = "MasterData/Create TestData", fileName = "TestData")]
    internal class TestScriptableObject : MasterDataScriptableObject
    {
        public string filter = string.Empty;

        private string beforeFileter = string.Empty;

        [Serializable]
        public class TestData
        {
            [HideInInspector]
            public int id;
            public string name;
            public int hp;
            public float attack;
            public float defense;

            public TestData(int id, string name, int hp, float attack, float defense)
            {
                this.id = id;
                this.name = name;
                this.hp = hp;
                this.attack = attack;
                this.defense = defense;
            }
        }

        [SerializeField]
        private List<TestData> data = new List<TestData>();
        private List<Test> dbData = new List<Test>();

        /// <summary>
        /// フィルタ結果のデータ
        /// </summary>
        /// <typeparam name="TestData"></typeparam>
        /// <returns></returns>
        private List<TestData> filterData = new List<TestData>();

        /// <summary>
        /// 表示用データ
        /// </summary>
        private List<TestData> viewData = new List<TestData>();

        void OnEnable()
        {
            // データのタイトル設定
            dataTitle = "<b>TestMasterData</b>";
        }

        public override void UpdateDBCache()
        {
            // DB読み込み
            Load();

            // DBのデータ更新
            dbData = db.TestTable.All.ToList();

            data.Clear();
            foreach (var item in dbData)
            {
                data.Add(new TestData(item.Id, item.Name, item.Hp, item.Attack,item.Deffence));
            }

            //filterData = data.Where(x => x.name.Contains(filter)).ToList();

            //viewData = filter == string.Empty ? data : filterData;

            //beforeFileter = filter;
        }

        public override void Save(DatabaseBuilder builder)
        {
            List<Test> temp = new List<Test>();
            foreach (var item in data.Select((v, i) => new { Value = v, Index = i }))
            {
                temp.Add(new Test(item.Index, item.Value.name, item.Value.hp, item.Value.attack, item.Value.defense));
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
                    if (item.Value.Name == data[item.Index].name &&
                        item.Value.Hp == data[item.Index].hp &&
                        item.Value.Attack == data[item.Index].attack)
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
                        if (!existsDiff) existsDiff = true;
                    }

                    sb.Clear();

                    if (isShowBefore)
                    {
                        sb.Append($"ID:{item.Value.Id} ");
                        sb.Append("NAME:" + (item.Value.Name == data[item.Index].name ? $"{data[item.Index].name} " : $"{item.Value.Name}→<color={colorCodeYellow}>{data[item.Index].name}</color> "));
                        sb.Append("HP:" + (item.Value.Hp == data[item.Index].hp ? $"{data[item.Index].hp} " : $"{item.Value.Hp}→<color={colorCodeYellow}>{data[item.Index].hp}</color> "));
                        sb.Append("ATTACK:" + (item.Value.Attack == data[item.Index].attack ? $"{data[item.Index].attack} " : $"{item.Value.Attack}→<color={colorCodeYellow}>{data[item.Index].attack}</color> "));
                    }
                    else
                    {
                        sb.Append($"ID:{item.Value.Id} ");
                        sb.Append("NAME:" + (item.Value.Name == data[item.Index].name ? $"{data[item.Index].name} " : $"<color={colorCodeYellow}>{data[item.Index].name}</color> "));
                        sb.Append("HP:" + (item.Value.Hp == data[item.Index].hp ? $"{data[item.Index].hp} " : $"<color={colorCodeYellow}>{data[item.Index].hp}</color> "));
                        sb.Append("ATTACK:" + (item.Value.Attack == data[item.Index].attack ? $"{data[item.Index].attack} " : $"<color={colorCodeYellow}>{data[item.Index].attack}</color> "));
                    }

                    messages.Add(sb.ToString());

                    continue;
                }

                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                // ScriptableObject側の要素が少ない場合、青で表示
                messages.Add($"-<color={colorCodeBlue}>ID:{item.Value.Id} NAME:{item.Value.Name} HP:{item.Value.Hp} ATTACK:{item.Value.Attack}</color>");
            }

            // ScriptableObject側の要素が多い場合、赤で表示
            if (dbData.Count < data.Count)
            {
                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                for (int i = dbData.Count; i < data.Count; i++)
                {
                    // FIXME: IDはインスペクタ側から追加すると前のIDのまま生成してしまうので、一旦無理やりIDを設定
                    messages.Add($"+<color={colorCodeRed}>ID:{i} NAME:{data[i].name} HP:{data[i].hp} ATTACK:{data[i].attack}</color>");
                }
            }

            return messages;
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> messages = new List<string>();
            foreach (var item in dbData)
            {
                messages.Add($"ID:{item.Id} NAME:{item.Name} HP:{item.Hp} ATTACK:{item.Attack}");
            }
            return messages;
        }
    }
}
