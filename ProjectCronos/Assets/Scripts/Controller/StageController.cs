using UnityEngine;
using System.Collections.Generic;

namespace ProjectCronos
{
    public class StageController : MonoBehaviour
    {
        [SerializeField]
        Gimmick[] gimmicks;

        [SerializeField]
        ItemShop[] itemShops;

        public async void Initialize(List<int> gimmickStatus)
        {
            // ギミックの初期化
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

            // アイテムショップの初期化
            foreach (var itemShop in itemShops)
            {
                await itemShop.Initialize();
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
