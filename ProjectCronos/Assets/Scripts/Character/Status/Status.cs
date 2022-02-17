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
        /// <summary>
        /// HPを表示するバー
        /// </summary>
        [SerializeField]
        HpBar hpBar;

        /// <summary>
        /// 最大体力
        /// </summary>
        [SerializeField]
        public int maxHp { get; set; }

        /// <summary>
        /// 残り体力
        /// </summary>
        [SerializeField]
        public int currntHp { get; set; }

        /// <summary>
        /// 移動速度
        /// </summary>
        [SerializeField]
        public int moveSpeed { get; set; }

        /// <summary>
        /// 攻撃力
        /// </summary>
        [SerializeField]
        public int attack { get; set; }

        /// <summary>
        /// 魔法攻撃力
        /// </summary>
        [SerializeField]
        public int magicAttack { get; set; }

        /// <summary>
        /// 防御力
        /// </summary>
        [SerializeField]
        public int defence { get; set; }

        /// <summary>
        /// 魔法防御力
        /// </summary>
        [SerializeField]
        public int magicDefence { get; set; }

        /// <summary>
        /// クリティカルが発生する確率(基本は0)
        /// </summary>
        [SerializeField]
        public float criticalRate { get; set; }

        /// <summary>
        /// クリティカルが発生した時に攻撃力にかかる倍率
        /// </summary>
        [SerializeField]
        public float criticalDamageRate { get; set; }

        /// <summary>
        /// 開始処理
        /// </summary>
        [SerializeField]
        void Start()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize()
        {
            currntHp = 10;
            UpdateHpText();
        }

        /// <summary>
        /// 被弾時処理(HPが０の場合、trueを返す)
        /// </summary>
        /// <param name="value">ダメージの値</param>
        /// <returns>HPが無くなった場合true</returns>
        public bool Damage(int value)
        {
            currntHp -= value;

            if (currntHp <= 0)
            {
                return true;
            }

            UpdateHpText();

            return false;
        }

        /// <summary>
        /// HP表示更新
        /// </summary>
        void UpdateHpText()
        {
            if (hpBar != null)
            {
                hpBar.UpdateHpText(currntHp);
            }
        }
    }
}
