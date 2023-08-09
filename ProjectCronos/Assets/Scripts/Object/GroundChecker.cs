using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem.Layouts;

namespace ProjectCronos
{
    /// <summary>
    /// 地面接地判定用クラス
    /// </summary>
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField]
        float onGroundDist = 0.5f;

        [SerializeField]
        LayerMask groundLayer;

        /// <summary>
        /// Ray
        /// </summary>
        Ray downRay;

        Action onLanding;
        Action onTakeOff;
        Collider col;
        bool isFloating = true;

        /// <summary>
        /// 宙に浮いている時間
        /// </summary>
        float floatingTime = 0.0f;

        void Start()
        {
            isFloating = GetOnGroundStatus();

            if (col == null)
            {
                col = GetComponent<Collider>();
                col.enabled = false;
            }
        }

        void Update()
        {
            // オブジェクトから離れた時、すぐに浮く判定にするのではなく
            // そこからN秒間チェックしてプレイヤーの下にあるレイに何も当たらなかったら浮いた判定
            if (isFloating)
            {
                floatingTime += Time.deltaTime;
                if (floatingTime >= 1.0f)
                {
                    isFloating = false;
                    floatingTime = 0.0f;
                }

                if (!GetOnGroundStatus())
                {
                    onTakeOff?.Invoke();
                    isFloating = false;
                    floatingTime = 0.0f;
                }
            }

            //Debug.DrawRay(transform.position, Vector3.down * onGroundDist, Color.red);
        }

        public bool GetOnGroundStatus()
        {
            return Physics.Raycast(
                transform.position,
                Vector3.down,
                onGroundDist,
                groundLayer); ;
        }

        /// <summary>
        /// グラウンド接地時のイベント初期化
        /// </summary>
        /// <param name="onLanding">地面に着地した時のイベント</param>
        /// <param name="onTakeOff">地面から離れた時のイベント</param>
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
                isFloating = true;
                //onTakeOff?.Invoke();
            }
        }
    }
}
