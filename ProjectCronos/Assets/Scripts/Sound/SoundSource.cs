using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// サウンドソース
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    internal class SoundSource : MonoBehaviour
    {
        AudioSource source;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // AudioSourceを取得
            source = this.gameObject.GetComponent<AudioSource>();
        }

        /// <summary>
        /// 再生
        /// </summary>
        public void Play(AudioClip clip)
        {
            //　設定してあるクリップが違う場合、登録
            if (source.clip != clip)
            {
                source.clip = clip;
            }

            source.Play();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        /// <summary>
        /// サウンドを再生中か
        /// </summary>
        /// <returns>再生中の場合trueを返す</returns>
        public bool IsPlaying()
        {
            return source.isPlaying;
        }

        /// <summary>
        /// ボリューム設定
        /// </summary>
        /// <param name="val"></param>
        public void SetVolume(float val)
        {
            source.volume = val;
        }
    }
}
