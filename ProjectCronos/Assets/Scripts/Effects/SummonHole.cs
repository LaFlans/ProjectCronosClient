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
        /// アニメーション終了時イベント
        /// </summary>
        void AnimationFinishEvent()
        {
            // 自身を破壊
            // TODO: ワープホールを縮小して破壊
            Destroy(this.gameObject);
        }
    }
}
