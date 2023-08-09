using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace ProjectCronos
{
    internal class SavePopup : PopupBase
    {
        /// <summary>
        /// セーブデータスロットのプレハブパス
        /// </summary>
        const string prefabPath = "Assets/Prefabs/UIs/Save/SaveDataSlot.prefab";

        Transform slotParent;

        /// <summary>
        /// 画面のタイトルテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI savePopupTitleText;

        /// <summary>
        /// セーブエリア名テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI saveAreaNameText;

        /// <summary>
        /// セーブデータスロット
        /// </summary>
        [SerializeField]
        List<SaveDataSlot> dataSlots;

        int selectSlotNum;

        /// <summary>
        /// セーブエリア情報キャッシュ
        /// </summary>
        SaveAreaInfo saveAreaInfoCache;

        /// <summary>
        /// 適用
        /// </summary>
        public void Apply(SaveAreaInfo saveAreaInfo)
        {
            saveAreaInfoCache = saveAreaInfo;

            ApplySaveDataSlot();

            savePopupTitleText.text = MasterDataManager.Instance.GetDic("SavePopupTitle");
            saveAreaNameText.text = MasterDataManager.Instance.GetDic(
                MasterDataManager.DB.SaveAreaDataTable.FindById(saveAreaInfo.savePointId).SaveAreaNameDicKey);

            selectSlotNum = 0;
            ApplySlotSelectStatus();
        }

        /// <summary>
        /// セーブデータスロット更新
        /// </summary>
        void ApplySaveDataSlot()
        {
            foreach (var slot in dataSlots.Select((item, index) => new { item, index }))
            {
                slot.item.Apply(SaveManager.Instance.Load(slot.index), slot.index);
            }
        }

        /// <summary>
        /// スロットの選択状態を更新
        /// </summary>
        void ApplySlotSelectStatus()
        {
            Debug.Log($"スロットの選択状態を更新:{selectSlotNum}");
            foreach (var slot in dataSlots.Select((item, index) => new {item, index}))
            {
                if (slot.index == selectSlotNum)
                {
                    Debug.Log($"スロットの選択状態をON:{selectSlotNum}");
                    slot.item.SetSelectStatus(true);
                    continue;
                }

                Debug.Log($"スロットの選択状態をOFF:{slot.index}");
                slot.item.SetSelectStatus(false);
            }
        }

        /// <summary>
        /// 上選択
        /// </summary>
        /// <param name="context"></param>
        void OnUp(InputAction.CallbackContext context)
        {
            Debug.Log("上を押したよ！");
            SoundManager.Instance.Play("Button47");

            selectSlotNum--;
            if (selectSlotNum < 0)
            {
                selectSlotNum = dataSlots.Count - 1;
            }

            ApplySlotSelectStatus();
        }

        /// <summary>
        /// 下選択
        /// </summary>
        /// <param name="context"></param>
        void OnDown(InputAction.CallbackContext context)
        {
            Debug.Log("下を押したよ！");
            SoundManager.Instance.Play("Button47");

            selectSlotNum++;
            if (selectSlotNum > (dataSlots.Count - 1))
            {
                selectSlotNum = 0;
            }

            ApplySlotSelectStatus();
        }

        /// <summary>
        /// 選択
        /// </summary>
        /// <param name="context"></param>
        void OnSubmit(InputAction.CallbackContext context)
        {
            Debug.Log($"{selectSlotNum}を選択した状態で決定を押したので保存するよ！");
            SoundManager.Instance.Play("Button47");

            // 保存処理
            SaveManager.Instance.Save(
                SaveData.Create(saveAreaInfoCache),
                selectSlotNum,
                () =>
                {
                    PopupManager.Instance.ShowSystemPopup(
                        new PopupBase.MessageParam("セーブ", "セーブが完了しました。", "はい"),
                        () =>
                        {
                            ApplySaveDataSlot();
                        });
                });
        }

        /// <summary>
        /// セーブデータを削除
        /// </summary>
        /// <param name="context"></param>
        void OnDelete(InputAction.CallbackContext context)
        {
            Debug.Log($"{selectSlotNum}を選択した状態で削除ボタンを押したので削除するよ！");
            SoundManager.Instance.Play("Button47");

            // 削除処理
            SaveManager.Instance.Delete(selectSlotNum);
            ApplySaveDataSlot();
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="context"></param>
        void OnClose(InputAction.CallbackContext context)
        {
            Debug.Log("閉じるを押したよ！");
            SoundManager.Instance.Play("Button47");

            Close();
        }

        protected override void Close()
        {
            base.Close();
        }


        public override void RegisterInputActions()
        {
            Debug.LogError("SavePopupのアクションを登録");
            InputManager.Instance.inputActions.UI.Submit.performed += OnSubmit;
            InputManager.Instance.inputActions.UI.Up.performed += OnUp;
            InputManager.Instance.inputActions.UI.Down.performed += OnDown;
            InputManager.Instance.inputActions.UI.Close.performed += OnClose;
            InputManager.Instance.inputActions.UI.Delete.performed += OnDelete;
        }

        public override void UnregisterInputActions()
        {
            Debug.LogError("SavePopupのアクションを解除");
            InputManager.Instance.inputActions.UI.Submit.performed -= OnSubmit;
            InputManager.Instance.inputActions.UI.Up.performed -= OnUp;
            InputManager.Instance.inputActions.UI.Down.performed -= OnDown;
            InputManager.Instance.inputActions.UI.Close.performed -= OnClose;
            InputManager.Instance.inputActions.UI.Delete.performed -= OnDelete;
        }
    }
}
