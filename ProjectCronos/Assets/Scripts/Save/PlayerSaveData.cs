using UnityEngine;
using System;

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

        public PlayerSaveData(int coinNum)
        {
            this.coinNum = coinNum;
        }
    }
}
