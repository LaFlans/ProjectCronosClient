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
        /// 初期化
        /// </summary>
        public void Initialize(int id, int amount)
        {
            var imagePath = MasterDataManager.DB.ItemDataTable.FindById(id).Path;
            image.texture = AddressableManager.Instance.GetLoadedTextures(imagePath);

            amountText.text = amount.ToString();
        }
    }
}
