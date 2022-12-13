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
            // データのタイトル設定
            dataTitle = "<b>SoundMasterData</b>";
        }

        public override void UpdateDBCache()
        {
            // DB読み込み
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
                    if (item.Value.Key == data[item.Index].key &&
                        item.Value.Type == (int)data[item.Index].type &&
                        item.Value.Path == data[item.Index].path)
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
                        sb.Append("KEY:" + (item.Value.Key == data[item.Index].key ? $"{data[item.Index].key} " : $"{item.Value.Key}→<color={colorCodeYellow}>{data[item.Index].key}</color> "));
                        sb.Append("TYPE:" + (item.Value.Type == (int)data[item.Index].type ? $"{data[item.Index].type.ToString()} " : $"{((EnumCollection.Sound.SOUND_TYPE)item.Value.Type).ToString()}→<color={colorCodeYellow}>{data[item.Index].type.ToString()}</color> "));
                        sb.Append("PATH:" + (item.Value.Path == data[item.Index].path ? $"{data[item.Index].path} " : $"{item.Value.Path}→<color={colorCodeYellow}>{data[item.Index].path}</color> "));
                    }
                    else
                    {
                        sb.Append("KEY:" + (item.Value.Key == data[item.Index].key ? $"{data[item.Index].key} " : $"<color={colorCodeYellow}>{data[item.Index].key}</color> "));
                        sb.Append("TYPE:" + (item.Value.Type == (int)data[item.Index].type ? $"{data[item.Index].type.ToString()} " : $"<color={colorCodeYellow}>{data[item.Index].type.ToString()}</color> "));
                        sb.Append("PATH:" + (item.Value.Path == data[item.Index].path ? $"{data[item.Index].path} " : $"<color={colorCodeYellow}>{data[item.Index].path}</color> "));
                    }

                    messages.Add(sb.ToString());

                    continue;
                }

                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                // ScriptableObject側の要素が少ない場合、青で表示
                messages.Add($"-<color={colorCodeBlue}>KEY:{item.Value.Key} TYPE:{item.Value.Type.ToString()} PATH:{item.Value.Path}</color>");
            }

            // ScriptableObject側の要素が多い場合、赤で表示
            if (dbData.Count < data.Count)
            {
                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                for (int i = dbData.Count; i < data.Count; i++)
                {
                    messages.Add($"+<color={colorCodeRed}>KEY:{data[i].key} TYPE:{data[i].type.ToString()} PATH:{data[i].path}</color>");
                }
            }

            return messages;
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> debugMessage = new List<string>();
            foreach (var item in dbData)
            {
                debugMessage.Add($"KEY:{item.Key} TYPE:{((EnumCollection.Sound.SOUND_TYPE)item.Type).ToString()} MESSAGE:{item.Path}");
            }
            return debugMessage;
        }
    }
}
