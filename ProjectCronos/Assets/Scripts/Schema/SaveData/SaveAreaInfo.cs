using MessagePack;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// セーブエリアの情報クラス
    /// </summary
    [MessagePackObject(true)]
    public class SaveAreaInfo
    {
        /// <summary>
        /// セーブポイントID
        /// </summary>
        [Key(0)]
        public int savePointId;

        /// <summary>
        /// リスポーン位置
        /// </summary>
        [Key(1)]
        public Vector3 respawnPosition;

        /// <summary>
        /// リスポーン時の向き
        /// </summary>
        [Key(2)]
        public Quaternion respawnRotation;

        public SaveAreaInfo(int savePointId, Vector3 respawnPosition, Quaternion respawnRotation)
        {
            this.savePointId = savePointId;
            this.respawnPosition = respawnPosition;
            this.respawnRotation = respawnRotation;
        }
    }
}
