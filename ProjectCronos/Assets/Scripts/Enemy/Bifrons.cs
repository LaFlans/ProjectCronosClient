using Cysharp.Threading.Tasks;
using System;
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

        [SerializeField]
        Transform attackObjParent;

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

        protected override void OnEnemyTimeScaleApply()
        {
            base.OnEnemyTimeScaleApply();

            //攻撃オブジェクトの速度を調整
            bool result = TimeManager.Instance.GetEnemyTimeScale() > 0;
            for (int i = 0; i < attackObjParent.childCount; i++)
            {
                attackObjParent.GetChild(i).GetComponent<AttackTrigger>().SetIsAct(result);
            }
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
                if (pos != null)
                {
                    GameObject obj = AddressableManager.Instance.GetLoadedObject(fireSkeltonPrefabPath);
                    obj.transform.position = pos.position;
                    obj.transform.SetParent(attackObjParent);
                    obj.GetComponent<AttackTrigger>().Init(target.gameObject, 10, EnumCollection.Attack.ATTACK_TYPE.ENEMY, status.attack, 1.0f,true);
                }

                SetNavmeshUpdatePositionFlase();
                agent.isStopped = true;
                isTracking = false;

                await AttackInterval();

                isTracking = true;
                agent.isStopped = false;
                SetNavmeshUpdatePositionTrue();
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
