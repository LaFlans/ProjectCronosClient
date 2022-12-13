using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    class MainEntryPoint : IEntryPoint
    {
        [SerializeField]
        Player player;

        [SerializeField]
        EnemyController enemyController;

        [SerializeField]
        SaveAreaController saveAreaController;

        [SerializeField]
        ItemLogger itemLogger;

        bool isShowPopup;
        bool isGameOver;

        [SerializeField]
        GameOverUIEffect gameOverEffect;

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

            // プレイ時間周りの設定
            // FIXME: ここは一旦1つ目のセーブデータを参照しているので後で修正
            TimeManager.instance.SetPlayTimeFloat(SaveManager.instance.Load(0).playTime);
            TimeManager.instance.StartMeasurePlayTime();

            isShowPopup = false;
            isGameOver = false;

            // BGMの再生
            SoundManager.Instance.Play("WorkBGM2");

            // プレイヤーの初期化
            await player.Initialize();

            // 敵の初期化
            enemyController.Initialize();

            // セーブエリアの初期化
            saveAreaController.Initialize();

            // アイテムロガーの初期化
            await itemLogger.Initialize();

#if UNITY_EDITOR
            // デバックメニューの初期化
            this.GetComponent<DebugMenu>().Initialize();
#endif

            // 入力イベント設定
            InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER);
            InputManager.Instance.inputActions.UI.Escape.performed += OnTransitionTitleConfirm;

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
                        TimeManager.instance.FinishMeasurePlayTime();

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
            InputManager.Instance.inputActions.UI.Escape.performed -= OnTransitionTitleConfirm;
        }

        /// <summary>
        /// 事前に読み込んでおきたいアセットをここで読み込む
        /// </summary>
        /// <returns>UniTask</returns>
        public override async UniTask PreLoadAsset()
        {
            // プレイヤーの事前読み込み
            await player.PreLoadAsync();
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
                    obj.GetComponent<PopupBase>().Setup(() => { isShowPopup = false; });
                }
            }
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
                    obj.GetComponent<PopupBase>().Setup(() => { isShowPopup = false; });
                }
            }
        }
    }
}
