using UnityEngine;
using TMPro;

namespace ProjectCronos
{
    internal class HpBar3d : IStatusBar
    {
        /// <summary>
        /// 表示テキスト
        /// </summary>
        [SerializeField]
        protected TextMeshPro viewText = null;

        [SerializeField]
        Transform hpDecreaseEffectPos;

        int tempValue;

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            tempValue = 0;
        }

        public override void SetShow(bool isShow)
        {
            this.gameObject.SetActive(isShow);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Apply(int currentValue, int maxValue, EnumCollection.UI.BAR_SHOW_STATUS status)
        {
            if (currentValue <= 0)
            {
                // HPが0以下の場合、表示しないようにする
                parent.gameObject.SetActive(false);
                return;
            }

            if (tempValue == currentValue)
            {
                // 値の更新がない時は何もしない
                return;
            }

            // 値が最大の時は、HP現象エフェクトを表示しない
            if (currentValue != maxValue)
            {
                Utility.CreateObject("Assets/Resources_moved/Prefabs/Effects/DecreaseHpEffect.prefab", hpDecreaseEffectPos.position, 1.0f);
            }

            // テキスト更新
            ApplyText(currentValue, maxValue, status);

            // バー表示対応
            var scale = parent.transform.localScale;
            scale.x = (float)currentValue / (float)maxValue;
            parent.transform.localScale = scale;

            // 一時保存している値更新
            tempValue = currentValue;
        }

        void ApplyText(int currentValue, int maxValue, EnumCollection.UI.BAR_SHOW_STATUS status)
        {
            switch (status)
            {
                case EnumCollection.UI.BAR_SHOW_STATUS.CURRENT_ONLY:
                    viewText.text = currentValue.ToString();
                    break;
                case EnumCollection.UI.BAR_SHOW_STATUS.ALL:
                    viewText.text = $"{currentValue}/{maxValue}";
                    break;
            }
        }
    }
}
