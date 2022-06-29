using System.Collections;using System.Collections.Generic;
using System.Text;using UnityEngine;using System;using System.Linq;using Generated;namespace ProjectCronos{    [CreateAssetMenu(menuName = "MasterData/Create DictionaryData", fileName = "DictionaryData")]    internal class DictionaryScriptableObject : MasterDataScriptableObject    {        [Serializable]        public class DictionaryData        {
            public string key;            public string message;            public DictionaryData(string key, string message)            {
                this.key = key;                this.message = message;            }        }        [SerializeField]        private List<DictionaryData> data = new List<DictionaryData>();
        private List<Dictionary> dbData = new List<Dictionary>();
        void OnEnable()        {            Load();

            // DBのデータ更新
            dbData = db.DictionaryTable.All.ToList();
            data.Clear();            foreach (var item in dbData)            {                data.Add(new DictionaryData(item.Key, item.Message));            }        }        public override void Save(DatabaseBuilder builder)        {            List<Dictionary> temp = new List<Dictionary>();            foreach (var item in data.Select((v, i) => new { Value = v, Index = i }))            {                temp.Add(new Dictionary(item.Value.key, item.Value.message));            }            builder.Append(temp);        }

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
                        sb.Append(item.Value.Key == data[item.Index].key ? $"KEY:{data[item.Index].key} " : $"KEY:{item.Value.Key}→<color={colorCodeYellow}>{data[item.Index].key}</color> ");
                        sb.Append(item.Value.Message == data[item.Index].message ? $"MESSAGE:{data[item.Index].message} " : $"MESSAGE:{item.Value.Message}→<color={colorCodeYellow}>{data[item.Index].message}</color> ");
                    }
                    else
                    {
                        sb.Append(item.Value.Key == data[item.Index].key ? $"KEY:{data[item.Index].key} " : $"KEY:<color={colorCodeYellow}>{data[item.Index].key}</color> ");
                        sb.Append(item.Value.Message == data[item.Index].message ? $"MESSAGE:{data[item.Index].message} " : $"MESSAGE:<color={colorCodeYellow}>{data[item.Index].message}</color> ");
                    }

                    messages.Add(sb.ToString());

                    continue;
                }

                // ScriptableObject側の要素が少ない場合、青で表示
                messages.Add($"<color={colorCodeBlue}>KEY:{item.Value.Key} MESSAGE:{item.Value.Message}</color>");
            }

            // ScriptableObject側の要素が多い場合、赤で表示
            if (dbData.Count < data.Count)
            {
                for (int i = dbData.Count; i < data.Count; i++)
                {
                    messages.Add($"<color={colorCodeRed}>KEY:{data[i].key} MESSAGE:{data[i].message}</color>");
                }
            }

            return messages;
        }        public override List<string> GetMasterDataDebugMessage()        {            List<string> debugMessage = new List<string>();            debugMessage.Add("DictionaryMasterData");            foreach (var item in data)            {                debugMessage.Add($"KEY:{item.key} MESSAGE:{item.message}");            }            return debugMessage;        }    }}