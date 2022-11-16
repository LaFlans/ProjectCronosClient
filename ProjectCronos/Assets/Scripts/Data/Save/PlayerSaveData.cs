using UnityEngine;
using System;

namespace ProjectCronos
{
    /// <summary>
    /// プレイヤーのセーブデータクラス
    /// </summary>
    public class PlayerSaveData
    {
        /// <summary>
        /// プレイ時間
        /// </summary>
        public long playTime;

        /// <summary>
        /// 最後に保存した時間(UnixTime)
        /// </summary>
        public long lastSaveTime;

        /// <summary>
        /// セーブポイントのID
        /// </summary>
        public int savePointId;

        PlayerSaveData(long playTime, long lastSaveTime, int savePointId)
        {
            this.playTime = playTime;
            this.lastSaveTime = lastSaveTime;
            this.savePointId = savePointId;
        }

        public static PlayerSaveData Create(int savePointId)
        {
            return new PlayerSaveData(((long)Time.time), Utility.GetUnixTime(DateTime.Now), savePointId);
        }
    }
}
