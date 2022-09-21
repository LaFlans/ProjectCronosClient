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

        bool isShowPopup;

        /// <summary>
        /// シーンの初期化
        /// </summary>
        /// <returns>UniTask</returns>
        public override async UniTask<bool> Initialize()
        {
            // 現在シーンの設定
            ManagerScene.SetCurrentScene(EnumCollection.Scene.SCENE_TYPE.MAIN);

            isShowPopup = false;

            // BGMの再生
            SoundManager.Instance.Play("WorkBGM2");

            // プレイヤーの初期化
            await player.Initialize();

            // 敵の初期化
            enemyController.Initialize();

            // 入力イベント設定
            InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER);
            InputManager.Instance.inputActions.UI.Escape.performed += OnTransitionTitleConfirm;

            return true;
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
