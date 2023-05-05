using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 基礎ステータスクラス
    /// </summary>
    public class Status : MonoBehaviour
    {
        const float AUTO_HEAL_SECONDS = 1;
        float autoHealSeconds;

        [SerializeField]
        protected string statusKey;

        /// <summary>
        /// HPを表示するバー
        /// </summary>
        [SerializeField]
        protected IStatusBar hpBar;

        /// <summary>
        /// MPを表示するバー
        /// </summary>
        [SerializeField]
        protected IStatusBar mpBar;

        /// <summary>
        /// 最大体力
        /// </summary>
        public int maxHp { get; set; }

        /// <summary>
        /// 残り体力
        /// </summary>
        public int currentHp { get; set; }

        /// <summary>
        /// 時間で回復するHPの割合
        /// </summary>
        public int timeHpHealPerSeconds { get; set; }

        /// <summary>
        /// 最大MP
        /// </summary>
        public int maxMp { get; set; }

        /// <summary>
        /// 残りMP
        /// </summary>
        public int currentMp { get; set; }

        /// <summary>
        /// 時間で回復するMPの割合
        /// </summary>
        public int timeMpHealPerSeconds { get; set; }

        /// <summary>
        /// 移動速度
        /// </summary>
        public float moveSpeed { get; set; }

        /// <summary>
        /// 攻撃力
        /// </summary>
        public int attack { get; set; }

        /// <summary>
        /// 魔法攻撃力
        /// </summary>
        public int magicAttack { get; set; }

        /// <summary>
        /// 防御力
        /// </summary>
        public int defence { get; set; }

        /// <summary>
        /// 魔法防御力
        /// </summary>
        public int magicDefence { get; set; }

        /// <summary>
        /// クリティカルが発生する確率(基本は0)
        /// </summary>
        public float criticalRate { get; set; }

        /// <summary>
        /// クリティカルが発生した時に攻撃力にかかる倍率
        /// </summary>
        public float criticalDamageRate { get; set; }

        /// <summary>
        /// 初期化済みか
        /// </summary>
        protected bool isInit;

        /// <summary>
        /// 更新処理
        /// </summary>
        void Update()
        {
            if (isInit)
            {
                // 時間自動回復処理
                TimeAutoHeal();
            }
        }

        /// <summary>
        /// 時間自動回復
        /// </summary>
        void TimeAutoHeal()
        {
            autoHealSeconds += Time.deltaTime;
            if (autoHealSeconds > AUTO_HEAL_SECONDS)
            {
                autoHealSeconds = 0;
                Heal();
            }
        }

        /// <summary>
        /// 回復処理
        /// </summary>
        void Heal()
        {
            AutoHealHp();
            AutoHealMp();
        }

        /// <summary>
        /// HP自動回復処理
        /// </summary>
        void AutoHealHp()
        {
            if (hpBar != null)
            {
                var val = (int)(maxHp * (timeHpHealPerSeconds / 100.0f));
                //Debug.Log($"{timeHpHealPerSeconds}%のHP自動回復(回復した値:{val})");
                HealHp(val);
            }
        }

        /// <summary>
        /// MP自動回復処理
        /// </summary>
        void AutoHealMp()
        {
            if (mpBar != null)
            {
                var val = (int)(maxMp * (timeMpHealPerSeconds / 100.0f));
                //Debug.Log($"{timeMpHealPerSeconds}%のMP自動回復(回復した値:{val})");
                HealMp(val);
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize()
        {
            isInit = false;

            //　HP周りの設定
            if (hpBar != null && !hpBar.gameObject.activeSelf)
            {
                hpBar.gameObject.SetActive(true);
            }

            maxHp = 10;
            currentHp = maxHp;
            timeHpHealPerSeconds = 10;
            ApplyHpText();

            // MP回りの設定
            maxMp = 1000;
            timeMpHealPerSeconds = 10;
            currentMp = maxMp;
            ApplyMpText();

            isInit = true;
        }

        /// <summary>
        /// HPダメージ処理(HPが０の場合、trueを返す)
        /// </summary>
        /// <param name="value">ダメージの値</param>
        /// <returns>HPが無くなった場合true</returns>
        public bool DamageHp(int value)
        {
            currentHp -= value;

            if (currentHp <= 0)
            {
                hpBar.SetShow(false);
                return true;
            }

            ApplyHpText();

            return false;
        }

        /// <summary>
        /// MPダメージ処理
        /// </summary>
        /// <param name="value">ダメージの値</param>
        /// <returns>MPが足りない場合、減らさずにtrueを返す</returns>
        public bool DamageMp(int value)
        {
            if (currentMp < value)
            {
                return true;
            }

            currentMp -= value;

            if (currentMp <= 0)
            {
                currentMp = 0;
            }

            ApplyMpText();

            return false;
        }

        /// <summary>
        /// HP回復処理
        /// </summary>
        public void HealHp(int value)
        {
            currentHp += value;
            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }

            ApplyHpText();
        }

        /// <summary>
        /// MP回復処理
        /// </summary>
        public void HealMp(int value)
        {
            currentMp += value;
            if (currentMp > maxMp)
            {
                currentMp = maxMp;
            }

            ApplyMpText();
        }

        /// <summary>
        /// HP表示更新
        /// </summary>
        public virtual void ApplyHpText()
        {
            if (hpBar != null)
            {
                hpBar.Apply(currentHp, maxHp, EnumCollection.UI.BAR_SHOW_STATUS.ALL);
            }
        }

        /// <summary>
        /// MP表示更新
        /// </summary>
        public virtual void ApplyMpText()
        {
            if (mpBar != null)
            {
                mpBar.Apply(currentMp, maxMp, EnumCollection.UI.BAR_SHOW_STATUS.ALL);
            }
        }
    }
}
