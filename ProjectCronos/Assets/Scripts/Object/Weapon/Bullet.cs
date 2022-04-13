using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class Bullet : Weapon
    {
        [SerializeField]
        float speed = 10;
        Vector3 vec;

        void FixedUpdate()
        {
            this.transform.position += vec * speed;
        }

        public void Initialize(Vector3 vec, float lifeTime)
        {
            this.vec = vec;
            Destroy(this.gameObject, lifeTime);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<Enemy>().Damage(1);
                Utility.CreatePrefab("Prefabs/BulletHitEffect",this.transform.position,0.5f);
                Destroy(this.gameObject);
            }

            if (col.gameObject.tag == "Ground")
            {
                Utility.CreatePrefab("Prefabs/BulletHitEffect", this.transform.position, 0.5f);
                Destroy(this.gameObject);
            }
        }
    }
}
