
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.InputSystem;
using System;

namespace ProjectCronos
{
    /// <summary>
    /// アイテム一覧画面
    /// </summary>
    class ShopItemListView : MonoBehaviour
    {
        /// <summary>
        /// 商品セル情報
        /// </summary>
        public class ItemCellInfo
        {
            public int itemId;
            public string itemName;
            public int itemPrice;

            public ItemCellInfo(int itemId,string itemName, int itemPrice)
            {
                this.itemId = itemId;
                this.itemName = itemName;
                this.itemPrice = itemPrice;
            }
        }

        string shopItemListCellPrefabPath = "Assets/Prefabs/UIs/Shop/ShopItemCell.prefab";

        List<ItemCellInfo> items;

        [SerializeField]
        Transform contentParent;

        List<GameObject> itemsList;

        // スクロール関連
        [SerializeField]
        ScrollRect scrollRect;
        [SerializeField]
        RectTransform viewportRectransform;
        [SerializeField]
        Transform contentTransform;
        [SerializeField]
        RectTransform nodePrefab;
        [SerializeField]
        VerticalLayoutGroup verticalLayoutGroup;

        /// <summary>
        /// 最後に選択したオブジェクト
        /// </summary>
        GameObject lastSelectObj;

        public void Init(Func<(int, int), bool> purchaceItemFunc, float priceRate, int shopGroupId)
        {
            items = new List<ItemCellInfo>();
            var shopItems = MasterDataManager.DB.ShopItemDataTable.All.Where(x => x.GroupId == shopGroupId);
            foreach(var i in shopItems)
            {
                var itemInfo = MasterDataManager.DB.ItemDataTable.FindById(i.ItemId);
                items.Add(new ItemCellInfo(i.ItemId, itemInfo.Name, (int)(itemInfo.BasePrice * priceRate)));
            }

            if (items.Any())
            {
                itemsList = new List<GameObject>();
                foreach (var item in items)
                {
                    GameObject obj = AddressableManager.Instance.GetLoadedObject(shopItemListCellPrefabPath, contentParent);
                    obj.GetComponent<ShopItemCell>().Init(item.itemId, item.itemName, item.itemPrice, purchaceItemFunc);
                    itemsList.Add(obj);
                }

                // 初期選択ボタンの初期化と追加
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(itemsList.First());

                lastSelectObj = EventSystem.current.currentSelectedGameObject;

                var selects = itemsList.Select(x => x.GetComponent<Selectable>()).ToList();
                for (int i = 0; i < selects.Count; i++)
                {
                    var nav = selects[i].navigation;
                    nav.mode = Navigation.Mode.Explicit;
                    nav.selectOnUp = selects[i == 0 ? selects.Count - 1 : i -1];
                    nav.selectOnDown = selects[(i + 1) % selects.Count];
                    selects[i].navigation = nav;
                }

                CancellationToken token = destroyCancellationToken;
                OnCurrentSelectedGameObjectChanged(token).Forget();
            }
        }

        public void SelectedItemCell()
        {
            if (lastSelectObj != null)
            {
                EventSystem.current.SetSelectedGameObject(lastSelectObj);
            }
        }

        async UniTask OnCurrentSelectedGameObjectChanged(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    await UniTask.WaitUntilValueChanged(EventSystem.current,
                        t => t.currentSelectedGameObject, cancellationToken: token);

                    // 値が変わった時の処理
                    var obj = EventSystem.current.currentSelectedGameObject;
                    if (obj != null)
                    {
                        var cell = obj.GetComponent<ShopItemCell>();
                        if (cell != null)
                        {
                            Scroll(cell.NodeNumber);
                            SoundManager.Instance.Play("Button47");
                            lastSelectObj = obj;
                        }
                    }
                    else
                    {
                        EventSystem.current.SetSelectedGameObject(lastSelectObj);
                    }
                }
            }
            catch (System.OperationCanceledException) { }
        }

        /// <summary>
        /// 自動スクロール
        /// </summary>
        void Scroll(int nodeIndex)
        {
            //要素間の間隔
            var spacing = verticalLayoutGroup.spacing;
            //現在のスクロール範囲の数値を計算しやすい様に上下反転
            var p = 1.0f - scrollRect.verticalNormalizedPosition;
            //現在の要素数
            var nodeCount = contentTransform.childCount;
            //描画範囲のサイズ
            var viewportSize = viewportRectransform.sizeDelta.y;
            //描画範囲のサイズの半分
            var harfViewport = viewportSize * 0.5f;
            //１要素のサイズ
            var nodeSize = nodePrefab.sizeDelta.y + spacing;
            //現在の描画範囲の中心座標
            var centerPosition = (nodeSize * nodeCount - viewportSize) * p + harfViewport;
            //現在の描画範囲の上端座標
            var topPosition = centerPosition - harfViewport;
            //現在の現在描画の下端座標
            var bottomPosition = centerPosition + harfViewport;
            // 現在選択中の要素の中心座標
            var nodeCenterPosition = nodeSize * nodeIndex + nodeSize / 2.0f;

            //選択した要素が上側にはみ出ている
            if (topPosition > nodeCenterPosition)
            {
                //選択要素が描画範囲に収まるようにスクロール
                var newP = (nodeSize * nodeIndex) / (nodeSize * nodeCount - viewportSize);
                scrollRect.verticalNormalizedPosition = 1.0f - newP; //反転していたので戻す
                return;
            }

            //選択した要素が下側にはみ出ている
            if (nodeCenterPosition > bottomPosition)
            {
                //選択要素が描画範囲に収まるようにスクロール
                var newP = (nodeSize * (nodeIndex + 1) + spacing - viewportSize) / (nodeSize * nodeCount - viewportSize);
                scrollRect.verticalNormalizedPosition = 1.0f - newP; //反転していたので戻す
            }
        }
    }
}
