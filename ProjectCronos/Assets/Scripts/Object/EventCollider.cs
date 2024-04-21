using UnityEngine;
using System;
using System.Linq;

namespace ProjectCronos
{
    /// <summary>
    /// イベントを設定できるコライダ
    /// </summary>
    internal class EventCollider : MonoBehaviour
    {
        [SerializeField]
        Action action;

        [SerializeField]
        GameObject[] appearObject;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            // 最初は非アクティブ状態にしておく
            if (appearObject.Any())
            {
                foreach (var obj in appearObject)
                {
                    obj.SetActive(false);
                }
            }
        }

        public void SetAction(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// 当たり判定に入った時に呼ばれる(トリガー用)
        /// </summary>
        /// <param name="col"></param>
        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                action?.Invoke();

                // このオブジェクトの当たり判定を非活性化
                gameObject.GetComponent<Collider>().enabled = false;

                if (appearObject != null)
                {
                    foreach (var obj in appearObject)
                    {
                        obj.SetActive(true);
                    }
                }
            }
        }
    }
}
