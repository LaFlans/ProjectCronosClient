using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectCronos
{
    public class GroundChecker : MonoBehaviour
    {
        private Player player;

        void Start()
        {
            player = this.gameObject.transform.parent.gameObject.GetComponent<Player>();
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Ground")
            {
                player.OnLanding();
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Ground")
            {
                player.OnTakeoff();
            }
        }
    }
}
