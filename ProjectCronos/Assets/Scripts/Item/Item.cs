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
        ItemDetailInfo itemDetailInfo;

        void Initialize()
        {
            var initData = MasterDataManager.DB.ItemDataTable.FindById(0);
            itemDetailInfo.Initialize(initData);
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
