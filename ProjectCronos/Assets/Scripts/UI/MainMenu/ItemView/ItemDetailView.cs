using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCronos
{
    /// <summary>
    /// アイテム詳細画面
    /// </summary>
    public class ItemDetailView : MonoBehaviour
    {
        /// <summary>
        /// アイテムイメージ
        /// </summary>
        [SerializeField]
        RawImage image;

        /// <summary>
        /// アイテム名テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI nameText;
        
        /// <summary>
        /// アイテム説明テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI descriptionText;

        /// <summary>
        /// アイテム個数テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI amountText;

        /// <summary>
        /// 初期化
        /// </summary>
        void Initialize(ItemDetailInfo info)
        {
            UpdateView(info);
        }

        void UpdateView(ItemDetailInfo info)
        {
            image.texture = AddressableManager.Instance.GetLoadedTextures(info.imagePath);
            nameText.text = info.name;
            descriptionText.text = info.description;

            // FIXME: プレイヤー情報から取得
            amountText.text = "1";
        }
    }
}
