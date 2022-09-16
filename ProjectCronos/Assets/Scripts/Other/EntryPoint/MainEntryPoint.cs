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

        bool isShowQuitPopup;

        /// <summary>
        /// シーンの初期化
        /// </summary>
        /// <returns>UniTask</returns>
        public override async UniTask<bool> Initialize()
        {
            isShowQuitPopup = false;

            // BGMの再生
            SoundManager.Instance.Play("WorkBGM2");

            // プレイヤーの初期化
            player.Initialize();

            // 敵の初期化
            enemyController.Initialize();

            InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER);
            InputManager.Instance.inputActions.UI.Escape.performed += OnApplicationQuitConfirm;

            return true;
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
            if (!isShowQuitPopup)
            {
                isShowQuitPopup = true;
                var obj = PopupManager.Instance.GetPopupObject(
                    EnumCollection.Popup.POPUP_TYPE.QUIT_APPLICATION);

                obj.GetComponent<PopupBase>().Setup(() => { isShowQuitPopup = false; });
            }
        }
    }
}
