using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 召喚スキル
    /// </summary>

    public class SummonSkill : MonoBehaviour
    {
        EnumCollection.Skill.TYPE skillType;

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 召喚時に行う行動
        /// </summary>
        public virtual void Action()
        {

        }
    }
}
