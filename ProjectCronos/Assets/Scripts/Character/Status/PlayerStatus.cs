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
        /// ジャンプ力
        /// </summary>
        public float jumpPower { get; set; }

        public override void Initialize()
        {
            isInit = false;

            var initData = MasterDataManager.DB.PlayerDataTable.FindByKey(statusKey);

            // HP周り設定
            maxHp = initData.MaxHp;
            currentHp = maxHp;
            timeHpHealPerSeconds = initData.TimeHpHealPerSeconds;
            ApplyHpText();

            // MP回り設定
            maxMp = initData.MaxMp;
            currentMp = maxMp;
            timeMpHealPerSeconds = initData.TimeMpHealPerSeconds;
            ApplyMpText();

            //　その他の値設定
            attack = initData.Attack;
            magicAttack = initData.MagicAttack;
            defence = initData.Defense;
            magicDefence = initData.MagicDefense;
            criticalRate = initData.CriticalRate;
            criticalDamageRate = initData.CriticalDamageRate;
            moveSpeed = initData.MoveSpeed;
            jumpPower = initData.JumpPower;

            isInit = true;
        }

        /// <summary>
        /// HP表示更新
        /// </summary>
        public override void ApplyHpText()
        {
            if (hpBar != null)
            {
                hpBar.Apply(currentHp, maxHp, EnumCollection.UI.BAR_SHOW_STATUS.ALL);
            }
        }
    }
}
