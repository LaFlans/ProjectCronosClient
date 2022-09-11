using UnityEngine;
using System;
using System.Collections.Generic;

namespace ProjectCronos
{
    internal class VerticalMove : MonoBehaviour
    {
        enum TRANSFORM_TYPE
        {
            X, Y, Z
        }
        [SerializeField]
        TRANSFORM_TYPE type;

        /// <summary>
        /// 振幅
        /// </summary>
        [SerializeField]
        float range = 5;

        /// <summary>
        /// 周期
        /// </summary>
        [SerializeField]
        private float period = 1;

        /// <summary>
        /// 位相
        /// </summary>
        [SerializeField, Range(0, 1)]
        private float phase = 0;

        Vector3 moveVec;

        Action<float> moveAction;
        float basePos;

        void Start()
        {
            switch (type)
            {
                case TRANSFORM_TYPE.X:
                    basePos = this.transform.position.x;
                    moveAction += MoveX;
                    break;
                case TRANSFORM_TYPE.Y:
                    basePos = this.transform.position.y;
                    moveAction += MoveY;
                    break;
                case TRANSFORM_TYPE.Z:
                    basePos = this.transform.position.z;
                    moveAction += MoveZ;
                    break;
            }
        }

        void Update()
        {
            // 周期と位相を考慮した現在時間計算
            var t = 4 * range * (Time.time / period + phase + 0.25f);

            // 往復した値を計算
            var value = Mathf.PingPong(t, 2 * range) - range;

            moveAction(value);
        }

        void MoveX(float val)
        {
            this.transform.position = new Vector3(basePos + val, this.transform.position.y, this.transform.position.z);
        }

        void MoveY(float val)
        {
            this.transform.position = new Vector3(this.transform.position.x, basePos + val, this.transform.position.z);
        }
        void MoveZ(float val)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, basePos + val);
        }

    }
}
