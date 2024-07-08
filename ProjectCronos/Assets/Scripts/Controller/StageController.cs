using UnityEngine;
using System.Collections.Generic;

namespace ProjectCronos
{
    public class StageController : MonoBehaviour
    {
        [SerializeField]
        Gimmick[] gimmicks;

        public async void Initialize(List<int> gimmickStatus)
        {
            int index = -1;
            foreach (var gimmick in gimmicks)
            {
                index++;
                if (index < gimmickStatus.Count)
                {
                    gimmick.Initialize((EnumCollection.Stage.GIMMICK_STATUS)gimmickStatus[index]);
                }
                else
                {
                    gimmick.Initialize(EnumCollection.Stage.GIMMICK_STATUS.UNTRIGGERED);
                }
            }
        }

        /// <summary>
        /// ステージのセーブデータ情報を取得
        /// </summary>
        public List<int> GetStageSaveData()
        {
            var statusList = new List<int>();

            foreach (var gimmick in gimmicks)
            {
                statusList.Add((int)gimmick.gimmickStatus);
            }

            return statusList;
        }
    }
}
