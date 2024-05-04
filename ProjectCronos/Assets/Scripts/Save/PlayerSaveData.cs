using UnityEngine;
using System;
using System.Collections.Generic;

namespace ProjectCronos
{
    /// <summary>
    /// プレイヤーのセーブデータクラス
    /// </summary
    [Serializable]
    public class PlayerSaveData
    {
        /// <summary>
        /// 現在所持しているコイン
        /// </summary>
        public int coinNum;

        /// <summary>
        /// 所持アイテム一覧
        /// アイテムIDと所持数
        /// </summary>
        public Dictionary<int,int> ownItems;

        public PlayerSaveData(int coinNum, Dictionary<int, int> ownItems)
        {
            this.coinNum = coinNum;
            this.ownItems = ownItems;
        }
    }
}
