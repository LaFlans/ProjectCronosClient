using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 魔法陣ベースクラス
    /// </summary>
    public abstract class MagicCircle : MonoBehaviour
    {
        EnumCollection.Skill.TYPE skillType;

        [SerializeField]
        public EnumCollection.Attack.SUMMON_ATTACK_TYPE summonAttackType;

        /// <summary>
        /// 魔法陣のレベル
        /// </summary>
        public int level = 1;

        /// <summary>
        /// トラップ設置
        /// </summary>
        public abstract void PutTrap();

        /// <summary>
        /// トラップ発動処理
        /// </summary>
        public abstract void TriggerTrap();

        /// <summary>
        /// トラップを設置できるか
        /// </summary>
        /// <returns></returns>
        public abstract bool IsPutTrap();
    }
}
