using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        Enemy[] enemies;

        public async void Initialize()
        {
            foreach (var enemy in enemies)
            {
                await enemy.Initialize();
            }
        }
    }
}
