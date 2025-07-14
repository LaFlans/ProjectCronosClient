using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using System.Linq;
using Generated;

namespace ProjectCronos
{
    [CreateAssetMenu(menuName = "MasterData/Create ItemData", fileName = "ItemData")]
    internal class ItemDataScriptableObject : MasterDataScriptableObject
    {
        [Serializable]
        public class ItemScriptableData
        {
            public int id;
            public string name;
            public string description;
            public EnumCollection.Item.ITEM_CATEGORY category;
            public string imagePath;
            public int basePrice;
            public int effectValue1;
            public int effectValue2;
            public int effectValue3;

            public ItemScriptableData(
                int id,
                string name,
                string description,
                EnumCollection.Item.ITEM_CATEGORY category,
                string imagePath,
                int basePrice,
                int effectValue1,
                int effectValue2,
                int effectValue3)
            {
                this.id = id;
                this.name = name;
                this.description = description;
                this.category = category;
                this.imagePath = imagePath;
                this.basePrice = basePrice;
                this.effectValue1 = effectValue1;
                this.effectValue2 = effectValue2;
                this.effectValue3 = effectValue3;
            }
        }

        [SerializeField]
        private List<ItemScriptableData> data = new List<ItemScriptableData>();
        private List<ItemData> dbData = new List<ItemData>();

        void OnEnable()
        {
            // データのタイトル設定
            dataTitle = "<b>ItemMasterData</b>";
        }

        public override void UpdateDBCache()
        {
            // DB読み込み
            Load();

            // DBのデータ更新
            dbData = db.ItemDataTable.All.ToList();

            data.Clear();
            foreach (var item in dbData)
            {
                data.Add(
                    new ItemScriptableData(
                        item.Id,
                        item.Name,
                        item.Description,
                        (EnumCollection.Item.ITEM_CATEGORY)item.Category,
                        item.Path,
                        item.BasePrice,
                        item.EffectValue1,
                        item.EffectValue2,
                        item.EffectValue3));
            }
        }

        public override void Save(DatabaseBuilder builder)
        {
            List<ItemData> temp = new List<ItemData>();
            foreach (var item in data.Select((v, i) => new { Value = v, Index = i }))
            {
                temp.Add(
                    new ItemData(
                        item.Value.id,
                        item.Value.name,
                        item.Value.description,
                        (int)item.Value.category,
                        item.Value.imagePath,
                        item.Value.basePrice,
                        item.Value.effectValue1,
                        item.Value.effectValue2,
                        item.Value.effectValue3));
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
                    var value = data[item.Index];
                    if (item.Value.Id == value.id &&
                        item.Value.Name == value.name &&
                        item.Value.Description == value.description &&
                        item.Value.Category == (int)value.category &&
                        item.Value.Path == value.imagePath &&
                        item.Value.EffectValue1 == value.effectValue1 &&
                        item.Value.EffectValue2 == value.effectValue2 &&
                        item.Value.EffectValue3 == value.effectValue3)
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
                        sb.Append(ShowDiffMessage("ID", item.Value.Id, value.id));
                        sb.Append(ShowDiffMessage("NAME", item.Value.Name, value.name));
                        sb.Append(ShowDiffMessage("DESCRIPTION", item.Value.Description, value.description));
                        sb.Append(ShowDiffMessage("CATEGORY", item.Value.Category, (int)value.category));
                        sb.Append(ShowDiffMessage("IMAGEPATH", item.Value.Path, value.imagePath));
                        sb.Append(ShowDiffMessage("BASEPRICE", item.Value.BasePrice, value.basePrice));
                        sb.Append(ShowDiffMessage("EFFECTVALUE1", item.Value.EffectValue1, value.effectValue1));
                        sb.Append(ShowDiffMessage("EFFECTVALUE2", item.Value.EffectValue2, value.effectValue2));
                        sb.Append(ShowDiffMessage("EFFECTVALUE3", item.Value.EffectValue3, value.effectValue3));
                    }
                    else
                    {
                        sb.Append(ShowDiffMessage("ID", item.Value.Id, value.id, true));
                        sb.Append(ShowDiffMessage("NAME", item.Value.Name, value.name, true));
                        sb.Append(ShowDiffMessage("DESCRIPTION", item.Value.Description, value.description, true));
                        sb.Append(ShowDiffMessage("CATEGORY", item.Value.Category, (int)value.category, true));
                        sb.Append(ShowDiffMessage("IMAGEPATH", item.Value.Path, value.imagePath, true));
                        sb.Append(ShowDiffMessage("BASEPRICE", item.Value.BasePrice, value.basePrice, true));
                        sb.Append(ShowDiffMessage("EFFECTVALUE1", item.Value.EffectValue1, value.effectValue1, true));
                        sb.Append(ShowDiffMessage("EFFECTVALUE2", item.Value.EffectValue2, value.effectValue2, true));
                        sb.Append(ShowDiffMessage("EFFECTVALUE3", item.Value.EffectValue3, value.effectValue3, true));

                        //sb.Append("KEY:" + (item.Value.Key == data[item.Index].key ? $"{data[item.Index].key} " : $"<color={colorCodeYellow}>{data[item.Index].key}</color> "));
                        //sb.Append("TYPE:" + (item.Value.Type == (int)data[item.Index].type ? $"{data[item.Index].type.ToString()} " : $"<color={colorCodeYellow}>{data[item.Index].type.ToString()}</color> "));
                        //sb.Append("PATH:" + (item.Value.Path == data[item.Index].path ? $"{data[item.Index].path} " : $"<color={colorCodeYellow}>{data[item.Index].path}</color> "));
                    }

                    messages.Add(sb.ToString());

                    continue;
                }

                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                // ScriptableObject側の要素が少ない場合、青で表示
                messages.Add($"-<color={colorCodeBlue}>KEY:{item.Value.Id} NAME:{item.Value.Name} DESCRIPTION:{item.Value.Description} CATEGORY:{item.Value.Category.ToString()} IMAGEPATH:{item.Value.Path} BASEPRICE:{item.Value.BasePrice} EFFECTVALUE1:{item.Value.EffectValue1} EFFECTVALUE2:{item.Value.EffectValue2} EFFECTVALUE3:{item.Value.EffectValue3}</color>");
            }

            // ScriptableObject側の要素が多い場合、赤で表示
            if (dbData.Count < data.Count)
            {
                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                for (int i = dbData.Count; i < data.Count; i++)
                {
                    messages.Add($"+<color={colorCodeRed}>ID:{data[i].id} NAME:{data[i].name} DESCRIPTION:{data[i].description} CATEGORY:{data[i].category.ToString()} IMAGEPATH:{data[i].imagePath} BASEPRICE:{data[i].basePrice} EFFECTVALUE1:{data[i].effectValue1} EFFECTVALUE2:{data[i].effectValue2} EFFECTVALUE3:{data[i].effectValue3}</color>");
                }
            }

            return messages;
        }

        string ShowDiffMessage<T>(string title, T before, T after, bool isShowDiff = false)
        {
            var message =
                $"{title}:" +
                (EqualityComparer<T>.Default.Equals(before,after) ?
                    $"{after} " :
                    isShowDiff ? $"{before}→<color={colorCodeYellow}>{after}</color> " : $"<color={colorCodeYellow}>{after}</color>");
            return message;
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> debugMessage = new List<string>();
            foreach (var item in dbData)
            {
                debugMessage.Add($"ID:{item.Id} NAME:{item.Name} DESCRIPTION:{item.Description} CATEGORY:{((EnumCollection.Item.ITEM_CATEGORY)item.Category).ToString()} IMAGEPATH:{item.Path} BASEPRICE:{item.BasePrice} EFFECTVALUE1:{item.EffectValue1} EFFECTVALUE2:{item.EffectValue2} EFFECTVALUE3:{item.EffectValue3}");
            }

            return debugMessage;
        }
    }
}
