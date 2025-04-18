using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    class MainEntryPoint : EntryPoint
    {
        [SerializeField]
        Player player;

        [SerializeField]
        StageController stageController;

        [SerializeField]
        EnemyController enemyController;

        [SerializeField]
        SaveAreaController saveAreaController;

        [SerializeField]
        GameLogger gameLogger;

        bool isShowPopup;
        bool isGameOver;

        [SerializeField]
        GameOverUIEffect gameOverEffect;

        [SerializeField]
        MainMenu mainMenu;

        [SerializeField]
        bool isInitSaveData;

        /// <summary>
        /// ガイド画面
        /// </summary>
        public static GuideView guideView;

        /// <summary>
        /// シーンの初期化
        /// </summary>
        /// <returns>UniTask</returns>
        public override async UniTask<bool> Initialize()
        {
            // 現在シーンの設定
            ManagerScene.SetCurrentScene(EnumCollection.Scene.SCENE_TYPE.MAIN);

            // ゲーム状態を設定
            ManagerScene.SetGameStatus(EnumCollection.Game.GAME_STATUS.GAME_PLAY);

            // セーブデータの読み込み
            if (isInitSaveData)
            {
                await SaveManager.Instance.CreateNewData();
            }
            else
            {
                // FIXME: ここは一旦1つ目のセーブデータを参照しているので後で修正
                await SaveManager.Instance.Load(0);
            }

            var saveData = SaveManager.Instance.lastLoadSaveData;

            // プレイ時間周りの設定
            TimeManager.Instance.SetPlayTimeFloat(saveData.playTime);
            TimeManager.Instance.StartMeasurePlayTime();

            isShowPopup = false;
            isGameOver = false;

            // BGMの再生
            SoundManager.Instance.Play("BGM1");

            // プレイヤーの初期化
            await player.Initialize();

            // ステージの初期化
            stageController.Initialize(saveData.stageSaveData.gimmicStatus);

            // 敵の初期化
            enemyController.Initialize();

            // セーブエリアの初期化
            saveAreaController.Initialize();

            // アイテムロガーの初期化
            await gameLogger.Initialize();

            // ガイド画面の初期化
            guideView = GameObject.Find("GuideView").GetComponent<GuideView>();
            guideView.Initialize();

            // メインメニューを非アクティブに
            mainMenu.gameObject.SetActive(false);

#if UNITY_EDITOR
            // デバックメニューの初期化
            this.GetComponent<DebugMenu>().Initialize();
#endif

            // 入力イベント設定
            InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER);
            InputManager.Instance.inputActions.Player.OpenMenu.performed += OnOpenMainMenu;

            return true;
        }

        void Update()
        {
            switch (ManagerScene.GetGameStatus())
            {
                case EnumCollection.Game.GAME_STATUS.GAME_PLAY:
                    break;
                case EnumCollection.Game.GAME_STATUS.GAME_CLEAR:
                    break;
                case EnumCollection.Game.GAME_STATUS.GAME_OVER:
                    if (player == null && !isGameOver)
                    {
                        // プレイ時間計測終了
                        TimeManager.Instance.FinishMeasurePlayTime();

                        isGameOver = true;
                        gameOverEffect.Apply(
                            () =>
                            {
                                // FIXME: 死んだらタイトルに戻っているが、本来はロードをはさんでシーン再読み込みを行って
                                //      　最後に保存したセーブポイントからスタートすべき
                                SceneLoader.TransitionScene(EnumCollection.Scene.SCENE_TYPE.TITLE);
                            });
                    }
                    break;
            }
        }

        void OnDestroy()
        {
            Debug.Log("遷移イベント削除");
            InputManager.Instance.inputActions.Player.OpenMenu.performed -= OnOpenMainMenu;
        }

        /// <summary>
        /// 事前に読み込んでおきたいアセットをここで読み込む
        /// </summary>
        /// <returns>UniTask</returns>
        public override async UniTask PreLoadAsset()
        {
            await player.PreLoadAsync();
            await mainMenu.PreLoadAsync();
        }

        /// <summary>
        /// アプリケーション終了確認
        /// </summary>
        /// <param name="context"></param>
        void OnApplicationQuitConfirm(InputAction.CallbackContext context)
        {
            if (!isShowPopup)
            {
                var obj = PopupManager.Instance.GetPopupObject(
                    EnumCollection.Popup.POPUP_TYPE.QUIT_APPLICATION);

                if (obj != null)
                {
                    isShowPopup = true;
                    obj.GetComponent<PopupBase>().Setup(
                        EnumCollection.Popup.POPUP_BUTTON_STATUS.DEFAULT,
                        new PopupBase.Param(null, null, null, () => { isShowPopup = false; }));
                }
            }
        }

        void OnSystemPopupTest(InputAction.CallbackContext context)
        {
            if (!isShowPopup)
            {
                isShowPopup = true;
                PopupManager.Instance.ShowSystemPopup(
                    new PopupBase.MessageParam("テスト", "テストポップアップです", "YES"),
                    () =>
                    {
                        isShowPopup = false;
                    });
            }
        }

        /// <summary>
        /// メインメニューを開く
        /// </summary>
        /// <param name="context"></param>
        void OnOpenMainMenu(InputAction.CallbackContext context)
        {
            // 入力イベント設定
            InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.MAIN_MENU);
            TimeManager.Instance.StopTime();
            mainMenu.gameObject.SetActive(true);
            mainMenu.Initialize();
        }

        /// <summary>
        /// タイトル遷移確認
        /// </summary>
        /// <param name="context"></param>
        void OnTransitionTitleConfirm(InputAction.CallbackContext context)
        {
            if (!isShowPopup)
            {
                var obj = PopupManager.Instance.GetPopupObject(
                    EnumCollection.Popup.POPUP_TYPE.TRANSITION_TITLE_CONFIRM);

                if (obj != null)
                {
                    isShowPopup = true;
                    var messageParam = new PopupBase.MessageParam(
                        MasterDataManager.Instance.GetDic("ApplicationPopupQuitTitle"),
                        MasterDataManager.Instance.GetDic("ApplicationPopupQuitMessage"),
                        MasterDataManager.Instance.GetDic("ApplicationPopupQuitYesButtonMessage"),
                        MasterDataManager.Instance.GetDic("ApplicationPopupQuitNoButtonMessage"));

                    obj.GetComponent<PopupBase>().Setup(
                        EnumCollection.Popup.POPUP_BUTTON_STATUS.DEFAULT,
                        new PopupBase.Param(
                            () =>
                            {
                                // タイトルに遷移を行う
                                SceneLoader.TransitionScene(EnumCollection.Scene.SCENE_TYPE.TITLE);
                            },
                            closeAction:() =>
                            {
                                isShowPopup = false;
                            })
                        ,messageParam);
                }
            }
        }
    }
}
