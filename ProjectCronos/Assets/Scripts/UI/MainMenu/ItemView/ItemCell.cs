using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCronos
{
    /// <summary>
    /// アイテムのセル
    /// </summary>
    public class ItemCell : MonoBehaviour
    {
        /// <summary>
        /// アイテム画像
        /// </summary>
        [SerializeField]
        RawImage image;

        /// <summary>
        /// 個数テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI amountText;

        /// <summary>
        /// 選択中の画像
        /// </summary>
        [SerializeField]
        GameObject selectImage;

        /// <summary>
        /// アイテムID
        /// </summary>
        int itemId {  get; set; }

        /// <summary>
        /// 上下左右を選択時に移動する先の要素数
        /// </summary>
        public int onUpMoveIndex { get; set; }
        public int onDownMoveIndex { get; set; }
        public int onLeftMoveIndex { get; set; }
        public int onRightMoveIndex { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize(int id, int amount)
        {
            itemId = id;
            var imagePath = MasterDataManager.DB.ItemDataTable.FindById(id).Path;
            image.texture = AddressableManager.Instance.GetLoadedTextures(imagePath);
            SetSelected(false);

            amountText.text = amount.ToString();
        }

        /// <summary>
        /// アイテムの選択状態を設定
        /// </summary>
        /// <param name="isSelect">選択している状態にするかどうか</param>
        public void SetSelected(bool isSelect)
        {
            selectImage.SetActive(isSelect);
        }

        /// <summary>
        /// 保持しているアイテムIDを返す
        /// </summary>
        /// <returns>保持しているアイテムID</returns>
        public int GetItemId()
        {
            return itemId;
        }
    }
}
