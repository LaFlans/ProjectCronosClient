using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class SaveAreaController : MonoBehaviour
    {
        [SerializeField]
        SaveArea[] saveAreas;

        /// <summary>
        /// セーブエリア初期化
        /// </summary>
        public async void Initialize()
        {
            foreach (var saveArea in saveAreas)
            {
                await saveArea.Initialize();
            }
        }
    }
}
