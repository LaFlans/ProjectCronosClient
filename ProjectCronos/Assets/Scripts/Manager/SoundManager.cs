using UnityEngine;

namespace ProjectCronos
{
    public class SoundManager : ISingleton<SoundManager>
    {
        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        protected override bool Initialize()
        {
            return true;
        }
    }
}