using UnityEngine;
using TMPro;

namespace ProjectCronos
{
    internal class SaveDataSlot : MonoBehaviour
    {
        /// <summary>
        /// セーブデータのスロット番号
        /// </summary>
        [SerializeField]
        TextMeshProUGUI saveSlotNumberText;

        /// <summary>
        /// セーブエリア名
        /// </summary>
        [SerializeField]
        TextMeshProUGUI saveAreaNameText;

        /// <summary>
        /// 最後に保存した時間テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI lastSaveTimeText;

        /// <summary>
        /// プレイ時間テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI playTimeText;

        /// <summary>
        /// 適用
        /// </summary>
        /// <param name="data">セーブデータ</param>
        /// <param name="number">セーブデータ番号</param>
        public void Apply(PlayerSaveData data, int number)
        {
            saveSlotNumberText.text = "DATA " + number.ToString();
            saveAreaNameText.text = data.savePointId.ToString();
            lastSaveTimeText.text = data.lastSaveTime.ToString();
            playTimeText.text = data.playTime.ToString();
        }
    }
}
