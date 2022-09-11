using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectCronos
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField]
        private Player player;

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
