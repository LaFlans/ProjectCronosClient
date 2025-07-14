using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

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
        /// セーブ中画像
        /// </summary>
        RawImage NowSavingImage;

        /// <summary>
        /// 操作できるかどうか
        /// </summary>
        bool isOperate;

        /// <summary>
        /// 適用
        /// </summary>
        public async UniTask Apply(SaveAreaInfo saveAreaInfo)
        {
            popupCanvasGroup = GetComponent<CanvasGroup>();

            saveAreaInfoCache = saveAreaInfo;
            isOperate = true;

            await ApplySaveDataSlot();

            savePopupTitleText.text = MasterDataManager.Instance.GetDic("SavePopupTitle");
            saveAreaNameText.text = MasterDataManager.Instance.GetDic(
                MasterDataManager.DB.SaveAreaDataTable.FindById(saveAreaInfo.savePointId).SaveAreaNameDicKey);

            selectSlotNum = 0;
            ApplySlotSelectStatus();
        }

        /// <summary>
        /// セーブデータスロット更新
        /// </summary>
        async UniTask ApplySaveDataSlot()
        {
            foreach (var slot in dataSlots.Select((item, index) => new { item, index }))
            {
                var data = await SaveManager.Instance.Load(slot.index);
                slot.item.Apply(data, slot.index);
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
            if (!isOperate)
            {
                return;
            }

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
            if (!isOperate)
            {
                return;
            }
            
            Debug.Log("下を押したよ！");
            SoundManager.Instance.Play("Button47");

            selectSlotNum++;
            if (selectSlotNum > (dataSlots.Count - 1))
            {
                selectSlotNum = 0;
            }

            ApplySlotSelectStatus();
        }

        public SaveData CreateSaveData(SaveAreaInfo saveAreaInfo)
        {
            // 時間のセーブ情報を作成
            var playTime = (long)TimeManager.Instance.GetPlayTimeFloat();
            var lastSaveTime = Utility.GetUnixTime(DateTime.Now);

            // プレイヤーのセーブ情報を作成  
            var playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            var playerSaveData = new PlayerSaveData(playerStatus.coinNum, playerStatus.itemHolder.CreateSaveItemData());

            // ステージのセーブ情報を作成
            var stageController = GameObject.Find("StageController").GetComponent<StageController>();
            StageSaveData stageData = new StageSaveData(stageController.GetStageSaveData());

            return new SaveData(
                playTime,
                lastSaveTime,
                playerSaveData,
                saveAreaInfo,
                stageData);
        }

        /// <summary>
        /// 選択
        /// </summary>
        /// <param name="context"></param>
        void OnSubmit(InputAction.CallbackContext context)
        {
            if (isOperate)
            {
                // セーブが完了するまで操作不可能状態に
                isOperate = false;

                Debug.Log($"{selectSlotNum}を選択した状態で決定を押したので保存するよ！");
                SoundManager.Instance.Play("Button47");

                // 保存処理
                _ = SaveManager.Instance.Save(
                    CreateSaveData(saveAreaInfoCache),
                    selectSlotNum,
                    () =>
                    {
                        PopupManager.Instance.ShowSystemPopup(
                            new PopupBase.MessageParam("セーブ", "セーブが完了しました。", "はい"),
                            () =>
                            {
                                _ = ApplySaveDataSlot();
                                SoundManager.Instance.Play("Button55");
                                isOperate = true;
                            });
                    });
            }
        }

        /// <summary>
        /// セーブデータを削除
        /// </summary>
        /// <param name="context"></param>
        void OnDelete(InputAction.CallbackContext context)
        {
            if (!isOperate)
            {
                return;
            }

            Debug.Log($"{selectSlotNum}を選択した状態で削除ボタンを押したので削除するよ！");
            SoundManager.Instance.Play("Button47");

            // 削除処理
            SaveManager.Instance.Delete(selectSlotNum);
            _ = ApplySaveDataSlot();
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="context"></param>
        void OnClose(InputAction.CallbackContext context)
        {
            if (!isOperate)
            {
                return;
            }

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
            Debug.Log("SavePopupのアクションを登録");
            InputManager.Instance.inputActions.UI.Submit.performed += OnSubmit;
            InputManager.Instance.inputActions.UI.Up.performed += OnUp;
            InputManager.Instance.inputActions.UI.Down.performed += OnDown;
            InputManager.Instance.inputActions.UI.Close.performed += OnClose;
            InputManager.Instance.inputActions.UI.Delete.performed += OnDelete;
        }

        public override void UnregisterInputActions()
        {
            Debug.Log("SavePopupのアクションを解除");
            InputManager.Instance.inputActions.UI.Submit.performed -= OnSubmit;
            InputManager.Instance.inputActions.UI.Up.performed -= OnUp;
            InputManager.Instance.inputActions.UI.Down.performed -= OnDown;
            InputManager.Instance.inputActions.UI.Close.performed -= OnClose;
            InputManager.Instance.inputActions.UI.Delete.performed -= OnDelete;
        }
    }
}
