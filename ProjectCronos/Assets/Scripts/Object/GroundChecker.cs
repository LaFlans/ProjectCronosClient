using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace ProjectCronos
{
    public class GroundChecker : MonoBehaviour
    {
        Action onLanding;
        Action onTakeOff;
        Collider col;

        void Start()
        {
            if (col == null)
            {
                col = GetComponent<Collider>();
                col.enabled = false;
            }
        }

        /// <summary>
        /// グラウンド接地時のイベント初期化
        /// </summary>
        /// <param name="onLanding">地面に着地した時</param>
        /// <param name="onTakeOff">地面から離れた時</param>
        public void Initialized(Action onLanding, Action onTakeOff)
        {
            this.onLanding = onLanding;
            this.onTakeOff = onTakeOff;

            if (col == null)
            {
                col = GetComponent<Collider>();
                col.enabled = false;
            }

            col.enabled = true;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Ground")
            {
                onLanding?.Invoke();
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Ground")
            {
                onTakeOff?.Invoke();
            }
        }
    }
}
