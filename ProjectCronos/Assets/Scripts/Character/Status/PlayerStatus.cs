using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace ProjectCronos
{
    /// <summary>
    /// プレイヤーのステータス
    /// </summary>
    public class PlayerStatus : Status
    {
        [SerializeField]
        ItemLogger itemLogger;

        /// <summary>
        /// ジャンプ力
        /// </summary>
        public float jumpPower { get; set; }

        /// <summary>
        /// 所持しているコイン数
        /// </summary>
        public int coinNum { get; set; }

        /// <summary>
        /// 所持しているアイテム
        /// </summary>
        public Dictionary<int,int> ownItems { get; set; }

        /// <summary>
        /// ステータス更新イベント
        /// ステータス更新時に呼んでほしいAPIをここに追加していく
        /// </summary>
        Action statusUpdateEvent;

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

            // セーブデータの読み込み
            // FIXME: 現在は決め打ちでデータを読み込んでいる
            var saveData = SaveManager.Instance.Load(0);
            if (saveData != null)
            {
                if (saveData.playerSaveData != null)
                {
                    coinNum = saveData.playerSaveData.coinNum;

                    if (saveData.playerSaveData.ownItems == null)
                    {
                        Debug.Log("所持アイテム初期化");
                        ownItems = new Dictionary<int, int>();
                    }
                    else
                    {
                        Debug.Log("所持アイテム読み込み");
                        ownItems = saveData.playerSaveData.ownItems;
                    }
                }
            }
            else
            {
                Debug.Log("セーブデータが読み込めませんでした");
                coinNum = 0;
            }

            isInit = true;
        }

        /// <summary>
        /// アイテムを追加
        /// </summary>
        /// <param name="itemId">追加するアイテムのID</param>
        /// <param name="amount">追加するアイテムの個数</param>
        public void AddItem(int itemId, int amount)
        {
            if (ownItems.ContainsKey(itemId))
            {
                ownItems[itemId] += amount;
                Debug.Log($"アイテムID:{itemId}を{amount}個追加しました");
            }
            else
            {
                ownItems.Add(itemId, amount);
            }
        }

        /// <summary>
        /// アイテムを消費
        /// </summary>
        /// <param name="itemId">消費するアイテムのID</param>
        /// <param name="amount">消費数</param>
        /// <returns>所持しているアイテムが足りずに消費に失敗したらfalse,消費に成功したらtrue</returns>
        public bool SubItem(int itemId, int amount)
        {
            if (ownItems.ContainsKey(itemId))
            {
                if (ownItems[itemId] >= amount)
                {
                    ownItems[itemId] -= amount;
                    Debug.Log($"アイテムID:{itemId}を{amount}個消費しました");
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// コイン追加
        /// </summary>
        /// <param name="value">追加するコイン数</param>
        public void AddCoin(int value)
        {
            coinNum += value;

            // ステータス更新イベント処理
            statusUpdateEvent?.Invoke();

            // FIXME: ここは後でマスタ文言に差し替え
            itemLogger.ShowItemLog($"{value}コイン入手しました");
        }

        /// <summary>
        /// コインを消費
        /// </summary>
        /// <param name="value">消費するコイン数</param>
        /// <returns>所持しているコインが足りずに消費に失敗したらfalse,消費に成功したらtrue</returns>
        public bool SubCoin(int value)
        {
            if (coinNum < value)
            {
                return false;
            }

            coinNum -= value;

            // ステータス更新イベント処理
            statusUpdateEvent?.Invoke();

            return true;
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

        /// <summary>
        /// ステータス更新イベント登録
        /// </summary>
        /// <param name="action">登録したいイベント</param>
        public void ResisterStatusUpdateEvent(Action action)
        {
            statusUpdateEvent += action;
        }

        /// <summary>
        /// ステータス更新イベント削除
        /// </summary>
        /// <param name="action">削除したいイベント</param>
        public void UnresisterStatusUpdateEvent(Action action)
        {
            statusUpdateEvent -= action;
        }
    }
}
