using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    [RequireComponent(typeof(EnemyStatus))]
    public class Enemy : Character, IEnemy
    {
        /// <summary>
        /// ‰Šú‰»
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Debug.Log("“G‰Šú‰»");
        }

        /// <summary>
        /// oŒ»ˆ—
        /// </summary>
        public override void Appear()
        {
            Debug.Log("“GoŒ»");
        }

        /// <summary>
        /// ”í’eˆ—
        /// </summary>
        /// <param name="value">ƒ_ƒ[ƒW‚Ì’l</param>
        public override void Damage(int value)
        {
            base.Damage(value);
            Debug.Log("“G”í’e");
        }

        /// <summary>
        /// €–Sˆ—
        /// </summary>
        public override void Death()
        {
            base.Death();

            Debug.Log("“G€–S");
            // ©g‚ğ”j‰ó
            Destroy(this.gameObject);
        }
    }
}
