using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace ProjectCronos
{
    public class WallGimmick : Gimmick
    {
        [SerializeField]
        PlayableDirector director;

        [SerializeField]
        TimelineAsset openTimelineAsset;

        [SerializeField]
        TimelineAsset closeTimelineAsset;

        /// <summary>
        /// アニメーター
        /// </summary>
        [SerializeField]
        Animator anim;

        /// <summary>
        /// 演出を毎回再生するか
        /// </summary>
        [SerializeField]
        bool isPlayTimelineEveryTime;

        /// <summary>
        /// 演出を一回見たか
        /// </summary>
        bool isFirstPlay;

        [SerializeField]
        UIMovieDirector movieDirector;

        public override void Initialize(EnumCollection.Stage.GIMMICK_STATUS status)
        {
            isFirstPlay = false;
            director.stopped += OnPlayableDirectorStopped;
        }

        void OnPlayableDirectorStopped(PlayableDirector aDirector)
        {
            if (director == aDirector)
            {
                movieDirector.EndMovie();
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
                movieDirector.StartMovie();
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
                movieDirector.StartMovie();
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
