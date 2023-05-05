using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        EnemyGroup[] enemyGroups;

        public async void Initialize()
        {
            foreach (var enemyGroup in enemyGroups)
            {
                enemyGroup.Initialize();
            }
        }
    }
}
