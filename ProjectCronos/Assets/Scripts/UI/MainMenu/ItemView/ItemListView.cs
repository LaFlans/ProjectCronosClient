using ProjectCronos.EnumCollection.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectCronos
{
    public class ItemListView : MonoBehaviour
    {
        /// <summary>
        /// セルを生成する親オブジェクト
        /// </summary>
        [SerializeField]
        RectTransform itemListContent;

        /// <summary>
        /// アイテム一覧の基盤となるビューオブジェクト
        /// </summary>
        [SerializeField]
        RectTransform itemListView;

        /// <summary>
        /// 生成するセル
        /// </summary>
        [SerializeField]
        GameObject cell;

        /// <summary>
        /// アイテムの種類セル
        /// </summary>
        List<ItemCell> itemCells;

        /// <summary>
        /// 選択中のアイテム要素
        /// </summary>
        int selectedIndex;

        /// <summary>
        /// 最後に選択したアイテム要素
        /// </summary>
        int lastSelectedIndex;

        GridLayoutGroup gridLayoutGroup;


        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize(ItemHolder itemHolder, ITEM_CATEGORY category)
        {
            itemCells = new List<ItemCell>();
            gridLayoutGroup = itemListContent.GetComponent<GridLayoutGroup>();
            lastSelectedIndex = 0;

            // 生成前に初期化しておく
            foreach (Transform child in itemListContent.transform)
            {
                Destroy(child.gameObject);
            }

           
            foreach (var item in itemHolder.ownItems)
            {
                if ((int)category == MasterDataManager.DB.ItemDataTable.FindById(item.Key).Category)
                {
                    var obj = Instantiate(cell, itemListContent.transform).GetComponent<ItemCell>();
                    obj.Initialize(item.Key, item.Value);
                    itemCells.Add(obj);
                }
            }

            if (itemCells.Any())
            {
                // アイテムセルの移動先を設定
                SetMoveIndex();

                // 最初のアイテムを選択状態に設定しておく
                UpdateSelectedItemView(0, true);
            }

            itemListContent.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 入力に対してのセルの移動先を設定
        /// </summary>
        void SetMoveIndex()
        {
            // ナビゲーション設定
            var min = 0;
            var max = itemCells.Count - 1;
            var width = gridLayoutGroup.constraintCount;
            for (int i = 0; i < itemCells.Count; i++)
            {
                var cell = itemCells[i];

                // 右
                var rightIndex = 0;
                if ((i + 1) % width == 0)
                {
                    rightIndex = Math.Clamp(i - (width - 1), min, max);
                }
                else if (i == max)
                {
                    rightIndex = Math.Clamp(i - ((i + 1) % width - 1), min, max);
                }
                else
                {
                    rightIndex = Math.Clamp(i + 1, min, max);
                }

                cell.onRightMoveIndex = rightIndex;

                // 左
                var leftIndex = 0;
                if ((i + 1) % width == 1)
                {
                    leftIndex = Math.Clamp(i + (width - 1), min, max);
                }
                else
                {
                    leftIndex = Math.Clamp(i - 1, min, max);
                }

                cell.onLeftMoveIndex = leftIndex;

                // 上
                var upIndex = 0;
                if (i < width)
                {
                    upIndex = Math.Clamp(i + ((max / width) * width), min, max);
                }
                else
                {
                    upIndex = Math.Clamp(i - width, min, max);
                }

                cell.onUpMoveIndex = upIndex;

                // 下
                var downIndex = 0;
                if (((max / width) * width) <= i)
                {
                    downIndex = Math.Clamp((i % width), min, max);
                }
                else
                {
                    downIndex = Math.Clamp(i + width, min, max);
                }

                cell.onDownMoveIndex = downIndex;
            }

            if (itemCells.Any())
            {
                selectedIndex = 0;
            }
        }

        /// <summary>
        /// 選択されているアイテム表示を更新
        /// </summary>
        /// <param name="id"></param>
        public void UpdateSelectedItemView(int id, bool isInit = false)
        {
            if (isInit)
            {
                SelectItem(id);
            }
            else
            {
                if (id != lastSelectedIndex)
                {
                    SelectItem(id);
                }
            }
        }

        void SelectItem(int id)
        {
            // 最後に選択していたアイテムは非選択状態にしておく
            itemCells[lastSelectedIndex].SetSelected(false);

            itemCells[id].SetSelected(true);
            selectedIndex = id;
            lastSelectedIndex = id;

            ScrollToSelectedItem();
        }

        void ScrollToSelectedItem()
        {
            var selected = itemCells[lastSelectedIndex];
            if (selected != null)
            {
                var selectedRect = selected.GetComponent<RectTransform>();
                var itemLocalPos = itemListContent.InverseTransformPoint(selectedRect.position);
                var itemListLocalPos = itemListContent.localPosition;

                itemListLocalPos.y = -itemLocalPos.y;

                if (itemListLocalPos.y <= 0)
                {
                    itemListLocalPos.y = 0;
                }

                itemListContent.localPosition = itemListLocalPos;
            }
        }

        public int GetSelectedItemId()
        {   
            return itemCells[selectedIndex].GetItemId();
        }

        public void OnUp()
        {
            UpdateSelectedItemView(itemCells[selectedIndex].onUpMoveIndex);
        }

        public void OnDown()
        {
            UpdateSelectedItemView(itemCells[selectedIndex].onDownMoveIndex);
        }

        public void OnLeft()
        {
            UpdateSelectedItemView(itemCells[selectedIndex].onLeftMoveIndex);
        }

        public void OnRight()
        {
            UpdateSelectedItemView(itemCells[selectedIndex].onRightMoveIndex);
        }
    }
}
