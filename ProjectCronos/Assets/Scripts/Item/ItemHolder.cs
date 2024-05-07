using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// プレイヤーのアイテム所持情報
    /// </summary>
    public class ItemHolder : MonoBehaviour
    {
        /// <summary>
        /// 所持しているアイテム
        /// </summary>
        public Dictionary<int, int> ownItems { get; set; }

        /// <summary>
        /// アイテム初期化
        /// </summary>
        public void Initialize()
        {
            Debug.Log("所持アイテム初期化");
            ownItems = new Dictionary<int, int>();
        }

        /// <summary>
        /// 所持アイテム情報を読み込む
        /// </summary>
        /// <param name="items">アイテムの所持情報(ID(string)と所持数)</param>
        public void LoadOwnItems(Dictionary<string, int> items)
        {
            Debug.Log("所持アイテム読み込み");
            foreach (var item in items)
            {
                if (int.TryParse(item.Key, out var id))
                {
                    ownItems.Add(id, item.Value);
                }
                else
                {
                    // アイテムIDのint変換に失敗
                    Debug.LogError($"{item.Key}をintに変換できませんでした。");
                    break;
                }
            }
        }

        /// <summary>
        /// セーブデータに保存する用のアイテム情報を作成
        /// </summary>
        /// <returns>セーブデータ用にアイテム情報を変換したものを返す</returns>
        public Dictionary<string, int> CreateSaveItemData()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var item in ownItems)
            {
                result.Add(item.Key.ToString(), item.Value);
            }

            return result;
        }

        /// <summary>
        /// アイテムを所持しているかどうか
        /// </summary>
        /// <returns>一つでも所持ている場合、Trueを返す</returns>
        public bool IsHoldItems()
        {
            return ownItems.Any();
        }

        /// <summary>
        /// 指定したIDのアイテムを所持しているかどうかを返す
        /// </summary>
        /// <param name="itemId">アイテムID</param>
        /// <returns>所持している場合、Trueを返す</returns>
        public bool IsHoldItem(int itemId)
        {
            return ownItems.ContainsKey(itemId);
        }

        /// <summary>
        /// 指定したIDのアイテム所持数を取得
        /// 所持していない場合、0を返す
        /// </summary>
        /// <param name="itemId">アイテムID</param>
        /// <returns>所持している場合、そのアイテムの所持数を、所持していない場合、0を返す</returns>
        public int GetHoldItemCount(int itemId)
        {
            return ownItems.ContainsKey(itemId) ? ownItems[itemId] : 0;
        }

        /// <summary>
        /// アイテムを追加
        /// </summary>
        /// <param name="itemId">追加するアイテムのID</param>
        /// <param name="amount">追加するアイテムの個数</param>
        public void AddItem(int itemId, int amount)
        {
            if (ownItems.ContainsKey(itemId))
            {
                ownItems[itemId] += amount;
                Debug.Log($"更新アイテムID:{itemId}を{amount}個追加(現在{ownItems[itemId]}個)");
            }
            else
            {
                ownItems.Add(itemId, amount);
                Debug.Log($"新規アイテムID:{itemId}を{amount}個追加(現在{ownItems[itemId]}個)");
            }
        }

        /// <summary>
        /// アイテムを消費
        /// </summary>
        /// <param name="itemId">消費するアイテムのID</param>
        /// <param name="amount">消費数</param>
        /// <returns>所持しているアイテムが足りずに消費に失敗したらfalse,消費に成功したらtrue</returns>
        public bool SubItem(int itemId, int amount)
        {
            if (ownItems.ContainsKey(itemId))
            {
                if (ownItems[itemId] >= amount)
                {
                    ownItems[itemId] -= amount;
                    Debug.Log($"アイテムID:{itemId}を{amount}個消費しました");
                    return true;
                }
            }

            return false;
        }
    }
}
