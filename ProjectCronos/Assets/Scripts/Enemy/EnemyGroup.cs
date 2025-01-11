using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 敵の集団グループ
    /// </summary>
    internal class EnemyGroup : MonoBehaviour
    {
        [SerializeField]
        Enemy[] enemies;

        [SerializeField]
        Collider col;

        [SerializeField]
        EventCollider eventCol;

        [SerializeField]
        int enemyCount = 0;

        public async void Initialize()
        {
            if (eventCol == null)
            {
                InitEnemy();
            }
            else
            {
                eventCol.SetAction(() => InitEnemy());
            }
        }

        async void InitEnemy()
        {
            enemyCount = enemies.Count();

            foreach (var enemy in enemies)
            {
                await enemy.Initialize();
                enemy.SetDeathAction(
                    () =>
                    {
                        SubEnemy();
                    });
            }
        }

        void SubEnemy()
        {
            enemyCount = enemyCount - 1;
            Debug.Log($"残りの敵はあと{enemyCount}体です!");

            if (enemyCount <= 0)
            {
                if (col != null)
                {
                    eventCol.DisappearObject();
                    Debug.Log($"敵を全滅しました");
                }
            }
        }
    }
}
