using UnityEngine;
using TMPro;

namespace ProjectCronos
{
    internal class MpBar : IStatusBar
    {
        /// <summary>
        /// 表示テキスト
        /// </summary>
        [SerializeField]
        protected TextMeshProUGUI viewText = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            // 親のスケールを初期化
            parent.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 表示設定
        /// </summary>
        /// <param name="isShow">表示するかどうか</param>
        public override void SetShow(bool isShow)
        {
            this.gameObject.SetActive(isShow);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Apply(int current, int max, EnumCollection.UI.BAR_SHOW_STATUS status)
        {
            // テキスト更新
            ApplyText(current, max, status);

            // バー表示対応
            var scale = parent.transform.localScale;
            scale.x = (float)current / (float)max;
            parent.transform.localScale = scale;
        }

        void ApplyText(int current, int max, EnumCollection.UI.BAR_SHOW_STATUS status)
        {
            switch (status)
            {
                case EnumCollection.UI.BAR_SHOW_STATUS.CURRENT_ONLY:
                    viewText.text = current.ToString();
                    break;
                case EnumCollection.UI.BAR_SHOW_STATUS.ALL:
                    viewText.text = $"{current}/{max}";
                    break;
            }
        }
    }
}
