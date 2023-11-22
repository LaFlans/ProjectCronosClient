using UnityEngine;

namespace ProjectCronos
{
    class MagicCircleCollider : MonoBehaviour
    {
        string demonHandPrefabPath = "Assets/Resources_moved/Prefabs/DemonHand.prefab";
        string demonHandTrapPrefabPath = "Assets/Resources_moved/Prefabs/MagicCircle/DemonHandTrap.prefab";

        /// <summary>
        /// 魔法陣の当たり判定
        /// </summary>
        Collider col;

        /// <summary>
        /// 結合できるかどうか
        /// </summary>
        public bool isCombine;

        MagicCircle baseMagicCircle;

        int level;

        void Start()
        {
            isCombine = true;
            col = GetComponent<Collider>();
            baseMagicCircle = this.transform.parent.GetComponent<MagicCircle>();
            level = baseMagicCircle.level;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "MagicCircle")
            {
                var magicCircleCollider = other.GetComponent<MagicCircleCollider>();
                if (magicCircleCollider.CheckCombine(this))
                {
                    isCombine = false;
                    var newLevel = level + 1;
                    var attack = 10 * newLevel;

                    // 二つの陣の間に新しい魔法陣を生成する
                    if (baseMagicCircle.summonAttackType == EnumCollection.Attack.SUMMON_ATTACK_TYPE.DIRECT)
                    {
                        GameObject obj = AddressableManager.Instance.GetLoadedObject(demonHandPrefabPath);
                        obj.transform.position = (other.transform.position + transform.position) * 0.5f;
                        obj.transform.rotation = Quaternion.Lerp(other.transform.rotation, transform.rotation, 0.5f);
                        obj.transform.localScale = new Vector3(newLevel, newLevel, newLevel);
                        obj.GetComponent<DemonHand>().Initialize(attack, newLevel);
                    }
                    else
                    {
                        GameObject obj = AddressableManager.Instance.GetLoadedObject(demonHandTrapPrefabPath);
                        obj.transform.position = (other.transform.position + transform.position) * 0.5f;
                        obj.transform.rotation = Quaternion.Lerp(other.transform.rotation, transform.rotation, 0.5f);
                        obj.transform.localScale = new Vector3(newLevel, newLevel, newLevel);
                        obj.GetComponent<DemonHand>().Initialize(attack, newLevel, true);
                        obj.GetComponent<DemonHand>().PutTrap();
                    }

                    // 自身の魔法陣と接触先の魔法陣を削除
                    magicCircleCollider.DestroyMagicCircle();
                    DestroyMagicCircle();
                }
            }
        }

        public void SetColliderEnable(bool isEnable)
        {

            if (col != null)
            {
                col.enabled = isEnable;
            }
        }

        public void DestroyMagicCircle()
        {
            // FIXME: 一旦そのまま削除していますが、拡縮で小さくして言って最後に削除
            Destroy(this.gameObject.transform.parent.gameObject);
        }

        /// <summary>
        /// 結合できるかどうか確認
        /// </summary>
        /// <param name="magicCircleCollider"></param>
        /// <returns>結合できる場合true、出来ない場合false</returns>
        public bool CheckCombine(MagicCircleCollider magicCircleCollider)
        {
            // 結合できない場合
            if (!isCombine)
            {
                return false;
            }

            // レベルが一緒の場合
            if (magicCircleCollider.level != level)
            {
                return false;
            }

            return true;
        }
    }
}
