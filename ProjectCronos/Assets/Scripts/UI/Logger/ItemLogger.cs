using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    /// <summary>
    /// アイテム系のログクラス
    /// </summary>
    class ItemLogger : MonoBehaviour
    {
        /// <summary>
        /// セルプレハブのパス
        /// </summary>
        static string prefabPath = "Assets/Prefabs/UIs/ItemLogCell.prefab";

        /// <summary>
        /// 初期化処理
        /// </summary>
        public async UniTask<bool> Initialize()
        {
            await AddressableManager.Instance.Load(prefabPath);
            InputManager.Instance.inputActions.Debug.Test.performed += AddItemLog;
            return true;
        }

        /// <summary>
        /// ログ表示
        /// </summary>
        /// <param name="message"></param>
        public void ShowItemLog(string message)
        {
            GameObject obj = AddressableManager.Instance.GetLoadedObject(prefabPath);
            obj.transform.parent = this.transform;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<ItemLogCell>().Initialize(message);
        }

        /// <summary>
        /// ログ追加
        /// </summary>
        /// <param name="context"></param>
        public void AddItemLog(InputAction.CallbackContext context)
        {
            GameObject obj = AddressableManager.Instance.GetLoadedObject(prefabPath);
            obj.transform.parent = this.transform;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<ItemLogCell>().Initialize($"{Random.Range(0, 100)}を取得しました");
        }
    }
}
