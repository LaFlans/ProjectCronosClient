using MessagePack;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// ゲームのセーブデータクラス
    /// </summary>
    [MessagePackObject(true)]
    public class SaveData
    {
        /// <summary>
        /// プレイ時間
        /// </summary>
        [Key(0)]
        public long playTime;

        /// <summary>
        /// 最後に保存した時間(UnixTime)
        /// </summary>
        [Key(1)]
        public long lastSaveTime;

        /// <summary>
        /// プレイヤーセーブ情報
        /// </summary>
        [Key(2)]
        public PlayerSaveData playerSaveData;

        /// <summary>
        /// セーブエリア情報
        /// </summary>
        [Key(3)]
        public SaveAreaInfo saveAreaInfo;

        /// <summary>
        /// ステージセーブ情報
        /// </summary>
        [Key(4)]
        public StageSaveData stageSaveData;

        public SaveData(
            long playTime,
            long lastSaveTime,
            PlayerSaveData playerSaveData,
            SaveAreaInfo saveAreaInfo,
            StageSaveData stageSaveData)
        {
            this.playTime = playTime;
            this.lastSaveTime = lastSaveTime;
            this.playerSaveData = playerSaveData;
            this.saveAreaInfo = saveAreaInfo;
            this.stageSaveData = stageSaveData;
        }
    }
}
