using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ProjectCronos
{
    public abstract class Singleton<T> : MonoBehaviour
    {
        private static T instance;

        /// <summary>
        /// インスタンスを取得等行う
        /// </summary>
        void Awake()
        {
            instance = GetComponent<T>();
        }

        /// <summary>
        /// 初期化処理
        /// 派生クラスではStart()を使用しないようにする為
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        public abstract UniTask<bool> Initialize();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("instanceが生成されていません！");
                }

                return instance;
            }
        }
    }
}
