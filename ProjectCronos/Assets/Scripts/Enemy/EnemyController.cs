using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        Enemy[] enemies;

        public void Initialize()
        {
            foreach (var enemy in enemies)
            {
                enemy.Initialize();
            }
        }
    }
}
