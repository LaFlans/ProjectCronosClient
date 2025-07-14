using TMPro;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 単位表示系汎用クラス
    /// </summary>
    internal class UnitView : MonoBehaviour
    {
        /// <summary>
        /// 量テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI amountText;

        /// <summary>
        /// 単位テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI unitText;

        string commaFormat;

        public enum UnitType
        {
            Money,
        }

        public void Init(int amount, UnitType type = UnitType.Money, bool isComma = false)
        {
            commaFormat = isComma ? "N0" : "";
            amountText.text = amount.ToString(commaFormat);
            unitText.text = GetUnitText(type);
        }

        public void ApplyView(int amount)
        {
            amountText.text = amount.ToString(commaFormat);
        }

        string GetUnitText(UnitType type)
        {
            switch (type)
            {
                case UnitType.Money:
                    return MasterDataManager.Instance.GetDic("MonetaryUnit");
            }

            return "";
        }
    }
}
