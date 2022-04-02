using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    public class SummonHole : MonoBehaviour
    {
        [SerializeField] GameObject target;
        Material mat;
        Animator anim;

        void Start()
        {
            SummonMatInit();
            anim = target.GetComponent<Animator>();
            target.GetComponent<DefaultAnimationEvent>().Init(finishAction: AnimationFinishEvent);
        }

        void SummonMatInit()
        {
            mat = target.GetComponent<Renderer>().material;
            mat.SetVector("_Up", this.transform.up.normalized);
            mat.SetVector("_CenterPos", this.transform.position);
        }

        /// <summary>
        /// �A�j���[�V�����I�����C�x���g
        /// </summary>
        void AnimationFinishEvent()
        {
            // ���g��j��
            // TODO: ���[�v�z�[�����k�����Ĕj��
            Destroy(this.gameObject);
        }
    }
}
