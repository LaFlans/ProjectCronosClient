using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// プレイヤーのステータス
    /// </summary>
    public class PlayerStatus : Status
    {
        /// <summary>
        /// HP表示更新
        /// </summary>
        public override void ApplyHpText()
        {
            if (hpBar != null)
            {
                hpBar.Apply(currntHp, maxHp, EnumCollection.UI.HP_BAR_SHOW_STATUS.ALL_HP);
            }
        }
    }
}
