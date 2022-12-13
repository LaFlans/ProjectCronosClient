using System;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// ゲームのセーブデータクラス
    /// </summary>
    [Serializable]
    public class SaveData
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
        /// プレイヤーセーブ情報
        /// </summary>
        public PlayerSaveData playerSaveData;

        /// <summary>
        /// セーブエリア情報
        /// </summary>
        public SaveAreaInfo saveAreaInfo;

        SaveData(long playTime, long lastSaveTime, PlayerSaveData playerSaveData, SaveAreaInfo saveAreaInfo)
        {
            this.playTime = playTime;
            this.lastSaveTime = lastSaveTime;
            this.playerSaveData = playerSaveData;
            this.saveAreaInfo = saveAreaInfo;
        }

        public static SaveData Create(SaveAreaInfo saveAreaInfo)
        {
            // 時間のセーブ情報を作成
            var playTime = (long)TimeManager.instance.GetPlayTimeFloat();
            var lastSaveTime = Utility.GetUnixTime(DateTime.Now);

            // プレイヤーのセーブ情報を作成
            var playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            var playerSaveData = new PlayerSaveData(playerStatus.coinNum);
            Debug.Log($"セーブします:所持コイン{playerStatus.coinNum}");

            return new SaveData(playTime, lastSaveTime, playerSaveData, saveAreaInfo);
        }
    }
}
