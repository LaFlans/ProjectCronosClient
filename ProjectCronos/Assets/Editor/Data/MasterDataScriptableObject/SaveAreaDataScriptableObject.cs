using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using System.Linq;
using Generated;

namespace ProjectCronos
{
    [CreateAssetMenu(menuName = "MasterData/Create SaveAreaData", fileName = "SaveAreaData")]
    internal class SaveAreaDataScriptableObject : MasterDataScriptableObject
    {
        [Serializable]
        public class SaveAreaScriptableData
        {
            public int id;
            public string saveAreaNameDicKey;
            public SaveAreaScriptableData(int id, string saveAreaNameDicKey)
            {
                this.id = id;
                this.saveAreaNameDicKey = saveAreaNameDicKey;
            }
        }

        [SerializeField]
        private List<SaveAreaScriptableData> data = new List<SaveAreaScriptableData>();
        private List<SaveAreaData> dbData = new List<SaveAreaData>();

        void OnEnable()
        {
            // データのタイトル設定
            dataTitle = "<b>SampleMasterData</b>";
        }

        public override void UpdateDBCache()
        {
            // DB読み込み
            Load();

            // DBのデータ更新
            dbData = db.SaveAreaDataTable.All.ToList();

            data.Clear();
            foreach (var item in dbData)
            {
                data.Add(new SaveAreaScriptableData(item.Id, item.SaveAreaNameDicKey));
            }
        }

        public override void Save(DatabaseBuilder builder)
        {
            List<SaveAreaData> temp = new List<SaveAreaData>();
            foreach (var item in data.Select((v, i) => new { Value = v, Index = i }))
            {
                temp.Add(new SaveAreaData(item.Index, item.Value.saveAreaNameDicKey));
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
                    if (item.Value.SaveAreaNameDicKey == data[item.Index].saveAreaNameDicKey)
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
                        sb.Append("SAVEAREANAMEDICKEY:" + (item.Value.SaveAreaNameDicKey == data[item.Index].saveAreaNameDicKey ? $"{data[item.Index].saveAreaNameDicKey} " : $"{item.Value.SaveAreaNameDicKey}→<color={colorCodeYellow}>{data[item.Index].saveAreaNameDicKey}</color> "));
                    }
                    else
                    {
                        sb.Append($"ID:{item.Value.Id} ");
                        sb.Append("SAVEAREANAMEDICKEY:" + (item.Value.SaveAreaNameDicKey == data[item.Index].saveAreaNameDicKey ? $"{data[item.Index].saveAreaNameDicKey} " : $"<color={colorCodeYellow}>{data[item.Index].saveAreaNameDicKey}</color> "));
                    }

                    messages.Add(sb.ToString());

                    continue;
                }

                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                // ScriptableObject側の要素が少ない場合、青で表示
                messages.Add($"-<color={colorCodeBlue}>ID:{item.Value.Id} SAVEAREANAMEDICKEY:{item.Value.SaveAreaNameDicKey} </color>");
            }

            // ScriptableObject側の要素が多い場合、赤で表示
            if (dbData.Count < data.Count)
            {
                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                for (int i = dbData.Count; i < data.Count; i++)
                {
                    // FIXME: IDはインスペクタ側から追加すると前のIDのまま生成してしまうので、一旦無理やりIDを設定
                    messages.Add($"+<color={colorCodeRed}>ID:{i} SAVEAREANAMEDICKEY:{data[i].saveAreaNameDicKey} </color>");
                }
            }

            return messages;
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> debugMessage = new List<string>();
            foreach (var item in dbData)
            {
                debugMessage.Add($"ID:{item.Id} SAVEAREANAMEDICKEY:{item.SaveAreaNameDicKey}");
            }
            return debugMessage;
        }
    }
}
