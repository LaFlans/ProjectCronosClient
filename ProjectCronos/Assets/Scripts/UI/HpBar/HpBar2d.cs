using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cinemachine;

namespace ProjectCronos
{
    internal class HpBar2d : IHpBar
    {
        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            // 親のスケールを初期化
            parent.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Apply(int value, int hpMax)
        {
            hpText.text = value.ToString();
        }
    }
}
