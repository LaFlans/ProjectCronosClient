using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Generated;

namespace ProjectCronos
{
    [CreateAssetMenu(menuName = "MasterData/Create TestData", fileName = "TestData")]
    internal class TestScriptableObject : MasterDataScriptableObject
    {
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

        void OnEnable()
        {
            Load();
            data.Clear();
            foreach (var item in db.TestTable.All)
            {
                data.Add(new TestData(item.Id, item.Name, item.Hp , item.Attack));
            }
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

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> debugMessage = new List<string>();
            debugMessage.Add("SampleMasterData");

            foreach (var item in data)
            {
                debugMessage.Add($"ID:{item.id} NAME:{item.name} HP:{item.hp} ATTACK:{item.attack}");
            }
            return debugMessage;
        }
    }
}
