using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// セーブエリアの情報クラス
    /// </summary
    [Serializable]
    public class SaveAreaInfo
    {
        /// <summary>
        /// 現在所持しているコイン
        /// </summary>
        public int savePointId;
        public Vector3 respawnPosition;
        public Quaternion respawnRotation;

        public SaveAreaInfo(int savePointId, Transform respawnTransform)
        {
            this.savePointId = savePointId;
            respawnPosition = respawnTransform.position;
            respawnRotation = respawnTransform.rotation;
        }
    }

}
