using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Generated;

namespace ProjectCronos
{
    [CreateAssetMenu(menuName = "MasterData/Create SampleData", fileName = "SampleData")]
    internal class SampleScriptableObject : MasterDataScriptableObject
    {
        [Serializable]
        public class SampleData
        {
            [HideInInspector]
            public int id;
            public string name;
            public string path;
            public SampleData(int id, string name, string path)
            {
                this.id = id;
                this.name = name;
                this.path = path;
            }
        }

        [SerializeField]
        private List<SampleData> data = new List<SampleData>();

        void OnEnable()
        {
            Load();
            data.Clear();
            foreach (var item in db.SampleTable.All)
            {
                data.Add(new SampleData(item.Id, item.Name, item.Path));
            }
        }

        public override void Save(DatabaseBuilder builder)
        {
            List<Sample> temp = new List<Sample>();
            foreach (var item in data.Select((v, i) => new { Value = v, Index = i }))
            {
                temp.Add(new Sample(item.Index, item.Value.name, item.Value.path));
            }
            builder.Append(temp);
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> debugMessage = new List<string>();
            debugMessage.Add("SampleMasterData");

            foreach(var item in data)
            {
                debugMessage.Add($"ID:{item.id} NAME:{item.name} PATH:{item.path}");
            }
            return debugMessage;
        }
    }
}
