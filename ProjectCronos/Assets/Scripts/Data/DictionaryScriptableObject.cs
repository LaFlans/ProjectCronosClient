using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Generated;

namespace ProjectCronos
{
    [CreateAssetMenu(menuName = "MasterData/Create DictionaryData", fileName = "DictionaryData")]
    internal class DictionaryScriptableObject : MasterDataScriptableObject
    {
        [Serializable]
        public class DictionaryData
        {
            public string key;
            public string message;
            public DictionaryData(string key, string message)
            {
                this.key = key;
                this.message = message;
            }
        }

        [SerializeField]
        private List<DictionaryData> data = new List<DictionaryData>();

        void OnEnable()
        {
            Load();
            data.Clear();
            foreach (var item in db.DictionaryTable.All)
            {
                data.Add(new DictionaryData(item.Key, item.Message));
            }
        }

        public override void Save(DatabaseBuilder builder)
        {
            List<Dictionary> temp = new List<Dictionary>();
            foreach (var item in data.Select((v, i) => new { Value = v, Index = i }))
            {
                temp.Add(new Dictionary(item.Value.key, item.Value.message));
            }
            builder.Append(temp);
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> debugMessage = new List<string>();
            debugMessage.Add("DictionaryMasterData");

            foreach (var item in data)
            {
                debugMessage.Add($"KEY:{item.key} MESSAGE:{item.message}");
            }
            return debugMessage;
        }
    }
}
