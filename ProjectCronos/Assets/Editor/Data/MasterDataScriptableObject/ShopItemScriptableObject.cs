using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using System.Linq;
using Generated;

namespace ProjectCronos
{
    [CreateAssetMenu(menuName = "MasterData/Create ShopItemData", fileName = "ShopItemData")]
    internal class ShopItemDataScriptableObject : MasterDataScriptableObject
    {
        [Serializable]
        public class ShopItemScriptableData
        {
            [HideInInspector]
            public int id;
            public int groupId;
            public int itemId;
            public int purchaseLimit;

            public ShopItemScriptableData(int id, int groupId, int itemId, int purchaseLimit)
            {
                this.id = id;
                this.groupId = groupId;
                this.itemId = itemId;
                this.purchaseLimit = purchaseLimit;
            }
        }

        [SerializeField]
        private List<ShopItemScriptableData> data = new List<ShopItemScriptableData>();
        private List<ShopItemData> dbData = new List<ShopItemData>();

        void OnEnable()
        {
            // データのタイトル設定
            dataTitle = "<b>ShopItemMasterData</b>";
        }

        public override void UpdateDBCache()
        {
            // DB読み込み
            Load();

            // DBのデータ更新
            dbData = db.ShopItemDataTable.All.ToList();

            data.Clear();
            foreach (var item in dbData)
            {
                data.Add(new ShopItemScriptableData(item.Id, item.GroupId, item.ItemId, item.PurchaseLimit));
            }
        }

        public override void Save(DatabaseBuilder builder)
        {
            List<ShopItemData> temp = new List<ShopItemData>();
            foreach (var item in data.Select((v, i) => new { Value = v, Index = i }))
            {
                temp.Add(new ShopItemData(item.Index, item.Value.groupId, item.Value.itemId, item.Value.purchaseLimit));
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
                    if (item.Value.GroupId == data[item.Index].groupId &&
                        item.Value.ItemId == data[item.Index].itemId &&
                        item.Value.PurchaseLimit == data[item.Index].purchaseLimit)
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
                        sb.Append("GROUPID:" + (item.Value.GroupId == data[item.Index].groupId ? $"{data[item.Index].groupId} " : $"{item.Value.GroupId}→<color={colorCodeYellow}>{data[item.Index].groupId}</color> "));
                        sb.Append("ITEMID:" + (item.Value.ItemId == data[item.Index].itemId ? $"{data[item.Index].itemId} " : $"{item.Value.ItemId}→<color={colorCodeYellow}>{data[item.Index].itemId}</color> "));
                        sb.Append("PURCHASELIMIT:" + (item.Value.PurchaseLimit == data[item.Index].purchaseLimit ? $"{data[item.Index].purchaseLimit} " : $"{item.Value.ItemId}→<color={colorCodeYellow}>{data[item.Index].itemId}</color> "));
                    }
                    else
                    {
                        sb.Append($"ID:{item.Value.Id} ");
                        sb.Append("GROUPID:" + (item.Value.GroupId == data[item.Index].groupId ? $"{data[item.Index].groupId} " : $"<color={colorCodeYellow}>{data[item.Index].groupId}</color> "));
                        sb.Append("ITEMID:" + (item.Value.ItemId == data[item.Index].itemId ? $"{data[item.Index]} " : $"<color={colorCodeYellow}>{data[item.Index].itemId}</color> "));
                        sb.Append("PURCHASELIMIT:" + (item.Value.PurchaseLimit == data[item.Index].purchaseLimit ? $"{data[item.Index].purchaseLimit} " : $"<color={colorCodeYellow}>{data[item.Index].purchaseLimit}</color> "));
                    }

                    messages.Add(sb.ToString());

                    continue;
                }

                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                // ScriptableObject側の要素が少ない場合、青で表示
                messages.Add($"-<color={colorCodeBlue}>ID:{item.Value.Id} GROUPID:{item.Value.GroupId} ITEMID:{item.Value.ItemId} PURCHASELIMIT:{item.Value.PurchaseLimit}</color>");
            }

            // ScriptableObject側の要素が多い場合、赤で表示
            if (dbData.Count < data.Count)
            {
                // 差分存在チェック
                if (!existsDiff) existsDiff = true;

                for (int i = dbData.Count; i < data.Count; i++)
                {
                    // FIXME: IDはインスペクタ側から追加すると前のIDのまま生成してしまうので、一旦無理やりIDを設定
                    messages.Add($"+<color={colorCodeRed}>ID:{i} GROUPID:{data[i].groupId} ITEMID:{data[i].itemId} PURCHASELIMIT:{data[i].purchaseLimit}</color>");
                }
            }

            return messages;
        }

        public override List<string> GetMasterDataDebugMessage()
        {
            List<string> debugMessage = new List<string>();
            foreach (var item in dbData)
            {
                debugMessage.Add($"ID:{item.Id} GROUPID:{item.GroupId} ITEMID:{item.ItemId} PURCHASELIMIT:{item.PurchaseLimit}");
            }
            return debugMessage;
        }
    }
}
