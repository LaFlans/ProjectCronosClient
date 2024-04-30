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
        float onGroundDist = 0.3f;

        [SerializeField]
        LayerMask groundLayer;

        Action onLanding;
        Action onTakeOff;
        Collider col;

        /// <summary>
        /// 浮いているかどうか
        /// </summary>
        bool isFloating = true;

        /// <summary>
        /// 宙に浮いている時間
        /// </summary>
        float floatingTime = 0.0f;

        void Start()
        {
            isFloating = IsGrounded();

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
            //if (isFloating)
            //{
            //    floatingTime += Time.deltaTime;
            //    if (floatingTime >= 1.0f)
            //    {
            //        isFloating = false;
            //        floatingTime = 0.0f;
            //    }

            //    if (!IsGrounded())
            //    {
            //        onTakeOff?.Invoke();
            //        isFloating = false;
            //        floatingTime = 0.0f;
            //    }
            //}
            //else
            //{

            //}

            //Debug.DrawRay(transform.position, Vector3.down * onGroundDist, Color.red);


            //if (IsGrounded())
            //{
            //    if (isFloating)
            //    {
            //        isFloating = false;
            //        onLanding?.Invoke();
            //    }
            //}
            //else
            //{
            //    if (!isFloating)
            //    {
            //        isFloating = true;
            //        onTakeOff?.Invoke();
            //    }
            //}

        }

        /// <summary>
        /// 地面に設置しているかどうかを返す
        /// </summary>
        /// <returns>設置している場合、Trueを返す</returns>
        public bool IsGrounded()
        {
            Debug.DrawRay(transform.position, new Vector3(0,-onGroundDist, 0), Color.cyan);

            return Physics.Raycast(
                transform.position,
                Vector3.down,
                onGroundDist,
                groundLayer);
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
                if (!IsGrounded())
                {
                    return;
                }

                //isFloating = true;
                onTakeOff?.Invoke();
            }
        }
    }
}
