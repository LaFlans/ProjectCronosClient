using System;
using System.Collections;
using System.Collections.Generic;
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

        public async void Initialize()
        {
            if (col == null)
            {
                InitEnemy();
            }
            else
            {
                if (col.TryGetComponent(out EventCollider eventCol))
                {
                    eventCol.SetAction(() => InitEnemy());
                }
            }
        }

        async void InitEnemy()
        {
            foreach (var enemy in enemies)
            {
                await enemy.Initialize();
            }
        }
    }
}
