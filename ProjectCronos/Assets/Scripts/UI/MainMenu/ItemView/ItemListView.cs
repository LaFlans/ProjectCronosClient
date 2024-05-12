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
        GameObject parent;

        /// <summary>
        /// 生成するセル
        /// </summary>
        [SerializeField]
        GameObject cell;

        /// <summary>
        /// アイテムがない時のテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI noItemText;

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

        EventSystem eventSystem;

        GridLayoutGroup gridLayoutGroup;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize(ItemHolder itemHolder)
        {
            itemCells = new List<ItemCell>();
            eventSystem = EventSystem.current;
            gridLayoutGroup = parent.GetComponent<GridLayoutGroup>();

            // 生成前に初期化しておく
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }

            noItemText.gameObject.SetActive(false);
           
            foreach (var item in itemHolder.ownItems)
            {
                var obj = Instantiate(cell, parent.transform).GetComponent<ItemCell>();
                obj.Initialize(item.Key, item.Value);
                itemCells.Add(obj);
            }

            // アイテムセルの移動先を設定
            SetMoveIndex();

            if (itemHolder.IsHoldItems())
            {
                // 最初のアイテムを選択状態に設定しておく
                UpdateSelectedItemView(0, true);
            }
            else
            {
                noItemText.gameObject.SetActive(true);
            }
        }

        ///// <summary>
        ///// ナビゲーションを設定
        ///// </summary>
        //void SetNavigation()
        //{
        //    // ナビゲーション設定
        //    var selects = itemCells.Select(x => x.toggle.GetComponent<Selectable>()).ToList();
        //    var min = 0;
        //    var max = selects.Count - 1;
        //    var width = gridLayoutGroup.constraintCount;
        //    for (int i = 0; i < selects.Count; i++)
        //    {
        //        var nav = selects[i].navigation;

        //        // 右
        //        var rightIndex = 0;
        //        if ((i + 1) % width == 0)
        //        {
        //            rightIndex = Math.Clamp(i - (width - 1), min, max);
        //        }
        //        else if (i == max)
        //        {
        //            rightIndex = Math.Clamp(i - ((i + 1) % width - 1), min, max);
        //        }
        //        else
        //        {
        //            rightIndex = Math.Clamp(i + 1, min, max);
        //        }

        //        nav.selectOnRight = selects[rightIndex];

        //        // 左
        //        var leftIndex = 0;
        //        if ((i + 1) % width == 1)
        //        {
        //            leftIndex = Math.Clamp(i + (width - 1), min, max);
        //        }
        //        else
        //        {
        //            leftIndex = Math.Clamp(i - 1, min, max);
        //        }

        //        nav.selectOnLeft = selects[leftIndex];

        //        // 上
        //        var upIndex = 0;
        //        if (i < width)
        //        {
        //            upIndex = Math.Clamp(i + ((max / width) * width), min, max);
        //        }
        //        else
        //        {
        //            upIndex = Math.Clamp(i - width, min, max);
        //        }

        //        nav.selectOnUp = selects[upIndex];

        //        // 下
        //        var downIndex = 0;
        //        if (((max / width) * width) <= i)
        //        {
        //            downIndex = Math.Clamp((i % width), min, max);
        //        }
        //        else
        //        {
        //            downIndex = Math.Clamp(i + width, min, max);
        //        }

        //        nav.selectOnDown = selects[downIndex];

        //        selects[i].navigation = nav;
        //    }

        //    if (itemCells.Any())
        //    {
        //        eventSystem.SetSelectedGameObject(itemCells.First().toggle.gameObject);
        //        selectedIndex = 0;
        //    }
        //}

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
