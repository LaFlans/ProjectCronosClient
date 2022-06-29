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

            public TestData(int id, string name, int hp, float attack)
            {
                this.id = id;
                this.name = name;
                this.hp = hp;
                this.attack = attack;
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
            Load();
            UpdateData();
        }

        //void OnValidate()
        //{
        //    Debug.Log("変更されました！");
        //    if (filter == beforeFileter)
        //    {
        //        // フィルターが更新されていない場合、何もしない
        //        return;
        //    }

        //    UpdateData();
        //}

        void UpdateData()
        {
            // DBのデータ更新
            dbData = db.TestTable.All.ToList();

            data.Clear();
            foreach (var item in dbData)
            {
                data.Add(new TestData(item.Id, item.Name, item.Hp, item.Attack));
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
                temp.Add(new Test(item.Index, item.Value.name, item.Value.hp, item.Value.attack));
            }
            builder.Append(temp);
        }

        public override List<string> GetMasterDataDiffDebugMessage(bool isShowBefore)
        {
            List<string> messages = new List<string>();
            messages.Add("TestMasterData");

            var sb = new StringBuilder();

            foreach (var item in dbData.Select((v, i) => new { Value = v, Index = i }))
            {
                // 存在している要素で比較して表示
                if (data.Count > item.Index)
                {
                    sb.Clear();

                    if (isShowBefore)
                    {
                        sb.Append($"ID:{item.Value.Id} ");
                        sb.Append(item.Value.Name == data[item.Index].name ? $"NAME:{data[item.Index].name} " : $"NAME:{item.Value.Name}→<color={colorCodeYellow}>{data[item.Index].name}</color> ");
                        sb.Append(item.Value.Hp == data[item.Index].hp ? $"HP:{data[item.Index].hp} " : $"HP:{item.Value.Hp}→<color={colorCodeYellow}>{data[item.Index].hp}</color> ");
                        sb.Append(item.Value.Attack == data[item.Index].attack ? $"ATTACK:{data[item.Index].attack} " : $"ATTACK:{item.Value.Attack}→<color={colorCodeYellow}>{data[item.Index].attack}</color> ");
                    }
                    else
                    {
                        sb.Append($"ID:{item.Value.Id} ");
                        sb.Append(item.Value.Name == data[item.Index].name ? $"NAME:{data[item.Index].name} " : $"NAME:<color={colorCodeYellow}>{data[item.Index].name}</color> ");
                        sb.Append(item.Value.Hp == data[item.Index].hp ? $"HP:{data[item.Index].hp} " : $"HP:<color={colorCodeYellow}>{data[item.Index].hp}</color> ");
                        sb.Append(item.Value.Attack == data[item.Index].attack ? $"ATTACK:{data[item.Index].attack} " : $"ATTACK:<color={colorCodeYellow}>{data[item.Index].attack}</color> ");
                    }

                    messages.Add(sb.ToString());

                    continue;
                }

                // ScriptableObject側の要素が少ない場合、青で表示
                messages.Add($"<color={colorCodeBlue}>ID:{item.Value.Id} NAME:{item.Value.Name} HP:{item.Value.Hp} ATTACK:{item.Value.Attack}</color>");
            }

            // ScriptableObject側の要素が多い場合、赤で表示
            if (dbData.Count < data.Count)
            {
                for (int i = dbData.Count; i < data.Count; i++)
                {
                    messages.Add($"<color={colorCodeRed}>ID:{data[i].id} NAME:{data[i].name} HP:{data[i].hp} ATTACK:{data[i].attack}</color>");
                }
            }

            return messages;
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> messages = new List<string>();
            messages.Add("TestMasterData");

            foreach (var item in dbData)
            {
                messages.Add($"ID:{item.Id} NAME:{item.Name} HP:{item.Hp} ATTACK:{item.Attack}");
            }
            return messages;
        }
    }
}
