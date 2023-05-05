using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectCronos
{
    public class EnemyAnimatorFunctions : MonoBehaviour
    {
        [SerializeField]
        public Action firstAction;

        [SerializeField]
        private UnityEvent secondAction;

        public void SecondAction()
        {
            secondAction.Invoke();
        }
    }
}
