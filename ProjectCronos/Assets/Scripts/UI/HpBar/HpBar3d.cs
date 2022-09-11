using UnityEngine;
using TMPro;

namespace ProjectCronos
{
    internal class HpBar3d : IHpBar
    {
        /// <summary>
        /// HP表示テキスト
        /// </summary>
        [SerializeField]
        protected TextMeshPro hpText = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {

        }

        public override void SetShow(bool isShow)
        {
            this.gameObject.SetActive(isShow);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Apply(int value, int hpMax, EnumCollection.UI.HP_BAR_SHOW_STATUS status)
        {
            if (value <= 0)
            {
                // HPが0以下の場合、表示しないようにする
                parent.gameObject.SetActive(false);
                return;
            }

            // テキスト更新
            ApplyHpText(value, hpMax, status);

            // バー表示対応
            var scale = parent.transform.localScale;
            scale.x = (float)value / (float)hpMax;
            parent.transform.localScale = scale;
        }

        void ApplyHpText(int value, int hpMax, EnumCollection.UI.HP_BAR_SHOW_STATUS status)
        {
            switch (status)
            {
                case EnumCollection.UI.HP_BAR_SHOW_STATUS.CURRENT_HP_ONLY:
                    hpText.text = value.ToString();
                    break;
                case EnumCollection.UI.HP_BAR_SHOW_STATUS.ALL_HP:
                    hpText.text = $"{value}/{hpMax}";
                    break;
            }
        }
    }
}
