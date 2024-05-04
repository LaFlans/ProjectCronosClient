using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// メインメニューのアイテムビュークラス
    /// </summary>
    public class MainMenuItemView : MonoBehaviour
    {
        /// <summary>
        /// アイテムの種類一覧画面
        /// </summary>
        [SerializeField]
        ItemCategoryListView itemCategoryListView;

        [SerializeField]
        ItemListView itemListView;

        /// <summary>
        /// アイテム詳細画面
        /// </summary>
        [SerializeField]
        ItemDetailView itemDetailView;

        /// <summary>
        /// 事前読み込み
        /// マネージャー系生成後に呼ばれる
        /// </summary>
        /// <returns>UniTask</returns>
        public async UniTask PreLoadAsync()
        {
            // ここで事前に必要な素材を読み込む

            // ここでアイテム画面に必要なリソースをすべて読み込んでおく

        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            itemCategoryListView.Initialize();
            itemListView.Initialize();
        }

        /// <summary>
        /// ビューの更新
        /// </summary>
        public void UpdateView()
        {

        }
    }
}
