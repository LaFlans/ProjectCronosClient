using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using System.Linq;
using Generated;

namespace ProjectCronos
{
    [CreateAssetMenu(menuName = "MasterData/Create SoundData", fileName = "SoundData")]
    internal class SoundScriptableObject : MasterDataScriptableObject
    {
        [Serializable]
        public class SoundData
        {
            public string key;
            public EnumCollection.Sound.SOUND_TYPE type;
            public string path;
            public SoundData(string key, EnumCollection.Sound.SOUND_TYPE type, string path)
            {
                this.key = key;
                this.type = type;
                this.path = path;
            }
        }

        [SerializeField]
        private List<SoundData> data = new List<SoundData>();
        private List<Sound> dbData = new List<Sound>();

        void OnEnable()
        {
            Load();

            // DBのデータ更新
            dbData = db.SoundTable.All.ToList();

            data.Clear();
            foreach (var item in dbData)
            {
                data.Add(new SoundData(item.Key, (EnumCollection.Sound.SOUND_TYPE)item.Type, item.Path));
            }
        }

        public override void Save(DatabaseBuilder builder)
        {
            List<Sound> temp = new List<Sound>();
            foreach (var item in data.Select((v, i) => new { Value = v, Index = i }))
            {
                temp.Add(new Sound(item.Value.key, (int)item.Value.type, item.Value.path));
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
                        sb.Append(item.Value.Key == data[item.Index].key ? $"KEY:{data[item.Index].key} " : $"NAME:{item.Value.Key}→<color={colorCodeYellow}>{data[item.Index].key}</color> ");
                        sb.Append(item.Value.Path == data[item.Index].path ? $"PATH:{data[item.Index].path} " : $"PATH:{item.Value.Path}→<color={colorCodeYellow}>{data[item.Index].path}</color> ");
                    }
                    else
                    {
                        sb.Append(item.Value.Key == data[item.Index].key ? $"KEY:{data[item.Index].key} " : $"KEY:<color={colorCodeYellow}>{data[item.Index].key}</color> ");
                        sb.Append(item.Value.Path == data[item.Index].path ? $"PATH:{data[item.Index].path} " : $"PATH:<color={colorCodeYellow}>{data[item.Index].path}</color> ");
                    }

                    messages.Add(sb.ToString());

                    continue;
                }

                // ScriptableObject側の要素が少ない場合、青で表示
                messages.Add($"<color={colorCodeBlue}>KEY:{item.Value.Key} PATH:{item.Value.Path}</color>");
            }

            // ScriptableObject側の要素が多い場合、赤で表示
            if (dbData.Count < data.Count)
            {
                for (int i = dbData.Count; i < data.Count; i++)
                {
                    messages.Add($"<color={colorCodeRed}>KEY:{data[i].key} PATH:{data[i].path}</color>");
                }
            }

            return messages;
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> debugMessage = new List<string>();
            debugMessage.Add("SoundMasterData");

            foreach (var item in data)
            {
                debugMessage.Add($"KEY:{item.key} TYPE:{item.type.ToString()} MESSAGE:{item.path}");
            }
            return debugMessage;
        }
    }
}
