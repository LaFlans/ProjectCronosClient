using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectCronos
{
    internal class SavePopup : MonoBehaviour
    {
        /// <summary>
        /// セーブデータスロットのプレハブパス
        /// </summary>
        const string prefabPath = "Assets/Prefabs/UIs/Save/SaveDataSlot.prefab";

        Transform slotParent;

        /// <summary>
        /// セーブデータスロット
        /// </summary>
        List<SaveDataSlot> dataSlots;

        /// <summary>
        /// 適用
        /// </summary>
        void Apply()
        {
            dataSlots = new List<SaveDataSlot>();

            for (int i = 0; i < SaveManager.SAVE_DATA_COUNT;i++)
            {
                dataSlots.Add(new SaveDataSlot());
            }
        }
    }
}
