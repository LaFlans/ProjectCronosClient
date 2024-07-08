using MessagePack;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
	/// <summary>
	/// ステージのセーブデータクラス
	/// </summary
	[MessagePackObject(true)]
	public class StageSaveData
	{
        /// <summary>
        /// ギミックの状態管理
        /// </summary>]
        [Key(0)]
        public List<int> gimmicStatus;

        public StageSaveData(List<int> gimmicStatus)
		{
			this.gimmicStatus = gimmicStatus;
		}
	}
}
