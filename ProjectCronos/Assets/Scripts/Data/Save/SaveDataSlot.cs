using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

        [SerializeField]
        Image selectImage;

        /// <summary>
        /// スロットの選択状態を設定
        /// </summary>
        /// <param name="result">指定の状態</param>
        public void SetSelectStatus(bool result)
        {
            selectImage.enabled = result;
        }

        /// <summary>
        /// 適用
        /// </summary>
        /// <param name="data">セーブデータ</param>
        /// <param name="number">セーブデータ番号</param>
        public void Apply(SaveData data, int number)
        {
            // データ番号は1から始まるのでインクリメント
            saveSlotNumberText.text = $"DATA{ number + 1 }";

            if (data == null)
            {
                // セーブデータがない場合
                saveAreaNameText.text = "セーブデータがありません。";
                lastSaveTimeText.gameObject.SetActive(false);
                playTimeText.gameObject.SetActive(false);

                return;
            }

            if (data.saveAreaInfo != null)
            {
                var dickey = MasterDataManager.DB.SaveAreaDataTable.FindById(data.saveAreaInfo.savePointId).SaveAreaNameDicKey;
                var message = MasterDataManager.instance.GetDic(dickey);
                saveAreaNameText.text = message;
            }
            else
            {
                saveAreaNameText.text = "セーブエリア情報が取得できませんでした。";
            }

            lastSaveTimeText.gameObject.SetActive(true);
            playTimeText.gameObject.SetActive(true);

            var dataTime = Utility.GetDateTime(data.lastSaveTime);
            lastSaveTimeText.text = dataTime.ToString("F");
            playTimeText.text = Utility.GetDateTime(data.playTime).ToLongTimeString();
        }
    }
}
