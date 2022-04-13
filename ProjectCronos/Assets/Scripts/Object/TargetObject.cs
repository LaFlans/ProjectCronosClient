using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class TargetObject : MonoBehaviour
    {
        GameObject targetObj;

        [SerializeField]
        Player player;

        [SerializeField]
        GameObject playerHead;

        [SerializeField]
        Cinemachine.CinemachineTargetGroup targetGroup;

        private void Start()
        {
            SetTarget(playerHead);
        }

        void Update()
        {
            if (targetObj == null) player.RockOn();
            this.transform.position = targetObj.transform.position;
        }

        public void SetTarget(GameObject p)
        {
            // ターゲットを全解除
            foreach (var target in targetGroup.m_Targets)
            {
                targetGroup.RemoveMember(target.target);
            }

            if (p != playerHead)
            {
                targetGroup.AddMember(playerHead.transform, 1, 1);
            }
            targetGroup.AddMember(p.transform, 1, 1);
            targetObj = p;
        }

        /// <summary>
        /// ターゲットが敵かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsTargetEnemy()
        {
            return targetObj != null && targetObj != playerHead;
        }
    }
}
