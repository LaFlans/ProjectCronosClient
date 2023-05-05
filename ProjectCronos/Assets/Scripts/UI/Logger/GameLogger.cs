using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    /// <summary>
    /// ゲーム中のログを管理するクラス
    /// </summary>
    class GameLogger : MonoBehaviour
    {
        /// <summary>
        /// アイテムログ
        /// </summary>
        [SerializeField]
        ItemLogger itemLogger;

        /// <summary>
        /// ダメージログ
        /// </summary>
        [SerializeField]
        DamageLogger damageLogger;

        /// <summary>
        /// 初期化
        /// </summary>
        public async UniTask<bool> Initialize()
        {
            await itemLogger.Initialize();
            await damageLogger.Initialize();

            return true;
        }
    }
}
