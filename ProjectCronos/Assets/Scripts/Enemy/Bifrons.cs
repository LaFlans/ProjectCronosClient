using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace ProjectCronos
{
    /// <summary>
    /// ビフロンス
    /// </summary>
    class Bifrons : Enemy
    {
        [SerializeField]
        List<Transform> spawnPos = null;

        /// <summary>
        /// 攻撃のインターバル
        /// </summary>
        float attackInterval = 1.5f;

        /// <summary>
        /// 攻撃できるか
        /// </summary>
        bool isAttack = true;



        string fireSkeltonPrefabPath = "Assets/Prefabs/FireSkelton.prefab";

        protected override async UniTask Load()
        {
            await AddressableManager.Instance.Load(fireSkeltonPrefabPath);
        }

        protected override async void Attack()
        {
            base.Attack();

            if (!isAttack)
            {
                // 攻撃できない状態の場合、何もしない
                return;
            }

            if (spawnPos.Any())
            {
                if (target == null)
                {
                    // ターゲットがいない場合、何もしない
                    return;
                }

                var pos = spawnPos[UnityEngine.Random.Range(0, spawnPos.Count)];

                GameObject obj = AddressableManager.Instance.GetLoadedObject(fireSkeltonPrefabPath);
                obj.transform.position = pos.position;

                obj.GetComponent<AttackTrigger>().Init(target.gameObject, 10, EnumCollection.Attack.ATTACK_TYPE.ENEMY, true);

                await AttackInterval();
            }
        }

        /// <summary>
        /// AIの思考インターバル
        /// </summary>
        async UniTask AttackInterval()
        {
            isAttack = false;

            await UniTask.Delay(TimeSpan.FromSeconds(attackInterval));

            isAttack = true;
        }
    }
}
