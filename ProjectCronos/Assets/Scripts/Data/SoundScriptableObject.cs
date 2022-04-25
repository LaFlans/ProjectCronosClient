using System.Collections;
using System.Collections.Generic;
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

        void OnEnable()
        {
            Load();
            data.Clear();
            foreach (var item in db.SoundTable.All)
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
