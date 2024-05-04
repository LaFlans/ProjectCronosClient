using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    /// <summary>
    /// ダメージ系のログクラス
    /// </summary>
    internal class DamageLogger : MonoBehaviour
    {
        /// <summary>
        /// 初期化済みか
        /// </summary>
        static bool isInitialize;
        
        /// <summary>
        /// セルプレハブのパス
        /// </summary>
        static string prefabPath = "Assets/Prefabs/UIs/Log/DamageLogCell.prefab";

        private void Start()
        {
            isInitialize = false;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public async UniTask<bool> Initialize()
        {
            await AddressableManager.Instance.Load(prefabPath);
            InputManager.Instance.inputActions.DebugActions.Test.performed += AddItemLog;

            isInitialize = true;
            return true;
        }

        /// <summary>
        /// ログ表示
        /// </summary>
        /// <param name="damageVal">表示するダメージの数値</param>
        /// <param name="position">ダメージが発生した座標</param>
        public static void ShowLog(int damageVal, Vector3 position, EnumCollection.Attack.ATTACK_TYPE type)
        {
            if (isInitialize)
            {
                GameObject obj = AddressableManager.Instance.GetLoadedObject(prefabPath);
                obj.transform.SetParent(GameObject.Find("DamageLogger").transform);
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(position);
                obj.GetComponent<DamageLogCell>().Initialize(damageVal.ToString(), type);
            }
        }

        /// <summary>
        /// ログ追加
        /// </summary>
        /// <param name="context"></param>
        public void AddItemLog(InputAction.CallbackContext context)
        {
            GameObject obj = AddressableManager.Instance.GetLoadedObject(prefabPath);
            obj.transform.SetParent(GameObject.Find("DamageLogger").transform);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<RectTransform>().position = new Vector3(Random.Range(0, 1500), Random.Range(0, 750), 0);
            obj.GetComponent<DamageLogCell>().Initialize($"{Random.Range(0, 1000)}", (EnumCollection.Attack.ATTACK_TYPE)Random.Range(0,2));
        }
    }
}
