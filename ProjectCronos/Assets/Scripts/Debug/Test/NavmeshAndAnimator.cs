using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.Animations;

namespace ProjectCronos
{
    public class NavmeshAndAnimator : MonoBehaviour
    {
        [SerializeField]
        NavMeshAgent agent;

        [SerializeField]
        Transform target;

        [SerializeField]
        Transform body;

        /// <summary>
        /// 体の親子関係コンストレイント
        /// </summary>
        [SerializeField]
        ParentConstraint bodyParentConstraint;

        /// <summary>
        /// ナビメッシュの親子関係コンストレイント
        /// </summary>
        [SerializeField]
        ParentConstraint navmeshParentConstraint;

        void Start()
        {
            //agent.SetDestination(target.position);

            // コンストレイント設定
            bodyParentConstraint.enabled = true;
            navmeshParentConstraint.enabled = false;
        }

        private void Update()
        {
            
            //agent.destination = target.position;
        }

        void SwitchParentConstraint()
        {
            bodyParentConstraint.enabled = !bodyParentConstraint.enabled;
            navmeshParentConstraint.enabled = !navmeshParentConstraint.enabled;
        }

        void SetNavmeshUpdatePositionFlase()
        {
            Debug.LogError("Agentの更新を止めました");
            agent.updatePosition = false;
            SwitchParentConstraint();
        }

        void SetNavmeshUpdatePositionTrue()
        {
            Debug.LogError("Agentの更新を再開しました");
            agent.updatePosition = true;

            SwitchParentConstraint();
            agent.Warp(body.position);
        }
    }
}
