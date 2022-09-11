using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCronos
{
    internal class HpBar3d : IHpBar
    {
        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Apply(int value, int hpMax)
        {
            hpText.text = value.ToString();

            // 3Dバー表示対応
            var scale = parent.transform.localScale;
            scale.x = (float)value / (float)hpMax;
            parent.transform.localScale = scale;
        }
    }
}
