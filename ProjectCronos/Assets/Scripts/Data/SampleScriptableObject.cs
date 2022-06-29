using System.Collections;
using System.Collections.Generic;
using System.Text;
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
            public string name;
            [HideInInspector]
            public int id;
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
        private List<Sample> dbData = new List<Sample>();

        void OnEnable()
        {
            Load();

            // DBのデータ更新
            dbData = db.SampleTable.All.ToList();

            data.Clear();
            foreach (var item in dbData)
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

        public override List<string> GetMasterDataDiffDebugMessage(bool isShowBefore)
        {
            List<string> messages = new List<string>();
            messages.Add("SoundMasterData");

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
                        sb.Append(item.Value.Path == data[item.Index].path ? $"PATH:{data[item.Index].path} " : $"PATH:{item.Value.Path}→<color={colorCodeYellow}>{data[item.Index].path}</color> ");
                    }
                    else
                    {
                        sb.Append($"ID:{item.Value.Id} ");
                        sb.Append(item.Value.Name == data[item.Index].name? $"NAME:{data[item.Index].name} " : $"NAME:<color={colorCodeYellow}>{data[item.Index].name}</color> ");
                        sb.Append(item.Value.Path == data[item.Index].path ? $"PATH:{data[item.Index].path} " : $"PATH:<color={colorCodeYellow}>{data[item.Index].path}</color> ");
                    }

                    messages.Add(sb.ToString());

                    continue;
                }

                // ScriptableObject側の要素が少ない場合、青で表示
                messages.Add($"<color={colorCodeBlue}>ID:{item.Value.Id} NAME:{item.Value.Name} PATH:{item.Value.Path}</color>");
            }

            // ScriptableObject側の要素が多い場合、赤で表示
            if (dbData.Count < data.Count)
            {
                for (int i = dbData.Count; i < data.Count; i++)
                {
                    messages.Add($"<color={colorCodeRed}>ID:{data[i].id} NAME:{data[i].name} PATH:{data[i].path}</color>");
                }
            }

            return messages;
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> debugMessage = new List<string>();
            debugMessage.Add("SampleMasterData");

            foreach (var item in data)
            {
                debugMessage.Add($"ID:{item.id} NAME:{item.name} PATH:{item.path}");
            }
            return debugMessage;
        }
    }
}
