using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProjectCronos.EnumCollection;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace ProjectCronos
{
    /// <summary>
    /// ボタンの説明クラス
    /// </summary>
    public class ButtonDescription : MonoBehaviour
    {
        /// <summary>
        /// ボタンの画像
        /// </summary>
        [SerializeField]
        RawImage iconImage;

        /// <summary>
        /// 対象のボタンを押した時に起こる事柄の説明テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI descriptionText;

        public void SetUp(Texture2D texture, string description)
        {
            iconImage.texture = texture;
            descriptionText.text = description;
            descriptionText.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
        }
    }
}
