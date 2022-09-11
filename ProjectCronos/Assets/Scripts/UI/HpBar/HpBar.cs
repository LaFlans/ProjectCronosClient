using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProjectCronos
{
    public abstract class IHpBar : MonoBehaviour
    {
        /// <summary>
        /// HP表示テキスト
        /// </summary>
        [SerializeField]
        protected TextMeshPro hpText;

        /// <summary>
        /// HPバーの親トランスフォーム
        /// </summary>
        [SerializeField]
        protected Transform parent = null;

        /// <summary>
        /// 初期化処理
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// 更新処理
        /// </summary>
        public abstract void Apply(int value, int hpMax);
    }
}
