using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ProjectCronos
{
    /// <summary>
    /// ギミック(仕掛け)クラス
    /// </summary>
    public class Gimmick : MonoBehaviour
    {
        /// <summary>
        /// ギミックの種類
        /// </summary>
        [SerializeField]
        EnumCollection.Stage.GIMMICK_TYPE gimmickType;

        /// <summary>
        /// アニメーター
        /// </summary>
        [SerializeField]
        Animator anim;

        [SerializeField]
        PlayableDirector director;

        [SerializeField]
        TimelineAsset openTimelineAsset;

        [SerializeField]
        TimelineAsset closeTimelineAsset;

        /// <summary>
        /// 演出を毎回再生するか
        /// </summary>
        [SerializeField]
        bool isPlayTimelineEveryTime;

        /// <summary>
        /// 演出を一回見たか
        /// </summary>
        bool isFirstPlay;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            gimmickType = EnumCollection.Stage.GIMMICK_TYPE.Switch;
            isFirstPlay = false;
            director.stopped += OnPlayableDirectorStopped;
        }

        void OnPlayableDirectorStopped(PlayableDirector aDirector)
        {
            if (director == aDirector)
            {
                TimeManager.Instance.ApplyTimeScale();
                //Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
            }
        }

        public void Open()
        {
            if (isFirstPlay)
            {
                anim.SetTrigger("Open");
            }
            else
            {
                director.Play(openTimelineAsset);
                TimeManager.Instance.StopTime();

                if (!isPlayTimelineEveryTime)
                {
                    isFirstPlay = true;
                }
            }
        }

        public void Close()
        {
            if (isFirstPlay)
            {
                anim.SetTrigger("Close");
            }
            else
            {
                director.Play(closeTimelineAsset);
                TimeManager.Instance.StopTime();

                if (!isPlayTimelineEveryTime)
                {
                    isFirstPlay = true;
                }
            }
        }
    }
}
