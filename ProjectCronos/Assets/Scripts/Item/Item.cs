using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// アイテムクラス
    /// </summary>
    public class Item : MonoBehaviour
    {
        public int itemId { get; set; }
        public ItemDetailInfo itemDetailInfo { get; set; }

        /// <summary>
        /// デフォルトのコンストラクタ
        /// アイテムidを指定しなかった場合、ダミーデータを読み込みます
        /// </summary>
        public Item()
        {
            var initData = MasterDataManager.DB.ItemDataTable.FindById(0);
            itemDetailInfo = new ItemDetailInfo();
            itemDetailInfo.Initialize(initData);
            this.itemId = itemId;
        }

        public Item(int itemId)
        {
            var initData = MasterDataManager.DB.ItemDataTable.FindById(itemId);
            itemDetailInfo = new ItemDetailInfo();
            itemDetailInfo.Initialize(initData);
            this.itemId = itemId;
        }
    }

    /// <summary>
    /// アイテム詳細の情報クラス
    /// </summary>
    public class ItemDetailInfo
    {
        public int id;
        public string name;
        public string description;
        public EnumCollection.Item.ITEM_CATEGORY category;
        public string imagePath;
        public int effectValue1;
        public int effectValue2;
        public int effectValue3;

        public void Initialize(ItemData data)
        {
            id = data.Id;
            name = data.Name;
            description = data.Description;
            category = (EnumCollection.Item.ITEM_CATEGORY)data.Category;
            imagePath = data.Path;
            effectValue1 = data.EffectValue1;
            effectValue2 = data.EffectValue2;
            effectValue3 = data.EffectValue3;
        }
    }
}
