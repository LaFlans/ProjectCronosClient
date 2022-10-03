using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 敵のステータス
    /// </summary>
    public class EnemyStatus : Status
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// AIの思考インターバル
        /// </summary>
        public float aiThinkingIntervalTime { get; set; }

        /// <summary>
        /// 攻撃範囲
        /// </summary>
        public float attackDist { get; set; }

        /// <summary>
        /// 索敵範囲
        /// </summary>
        public float searchDist { get; set; }

        public override void Initialize()
        {
            isInit = false;

            var initData = MasterDataManager.DB.EnemyDataTable.FindByKey(statusKey);

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
            name = initData.Name;
            aiThinkingIntervalTime = initData.AiThinkingIntervalTime;
            attackDist = initData.AttackDist;
            searchDist = initData.SearchDist;
            moveSpeed = initData.MoveSpeed;

            isInit = true;
        }
    }
}
