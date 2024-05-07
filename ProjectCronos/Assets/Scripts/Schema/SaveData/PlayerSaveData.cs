using MessagePack;
using System.Collections.Generic;

namespace ProjectCronos
{
    /// <summary>
    /// プレイヤーのセーブデータクラス
    /// </summary
    [MessagePackObject(true)]
    public class PlayerSaveData
    {
        /// <summary>
        /// 現在所持しているコイン
        /// </summary>
        [Key(0)]
        public int coinNum;

        /// <summary>
        /// 所持アイテム一覧
        /// アイテムIDと所持数
        /// </summary>]
        [Key(1)]
        public Dictionary<string,int> ownItems;

        public PlayerSaveData(int coinNum, Dictionary<string, int> ownItems)
        {
            this.coinNum = coinNum;
            this.ownItems = ownItems;
        }
    }
}
