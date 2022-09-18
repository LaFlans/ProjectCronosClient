using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    class TitleEntryPoint : IEntryPoint
    {
        /// <summary>
        /// シーンの初期化
        /// </summary>
        /// <returns>UniTask</returns>
        public override async UniTask<bool> Initialize()
        {
            // 現在シーンの設定
            ManagerScene.SetCurrentScene(EnumCollection.Scene.SCENE_TYPE.TITLE);

            // 入力イベント設定
            InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.UI);
            InputManager.Instance.inputActions.UI.Submit.performed += OnClickSubmit;

            // BGMの再生設定
            SoundManager.Instance.Play("WorkBGM1");

            return true;
        }

        void OnDestroy()
        {
            InputManager.Instance.inputActions.UI.Submit.performed -= OnClickSubmit;
        }

        /// <summary>
        /// 事前に読み込んでおきたいアセットをここで読み込む
        /// </summary>
        /// <returns>UniTask</returns>
        public override async UniTask PreLoadAsset()
        {
        }

        /// <summary>
        /// 決定ボタンを押したときの処理
        /// </summary>
        /// <param name="context"></param>
        void OnClickSubmit(InputAction.CallbackContext context)
        {
            // メインシーンへ遷移を行う
            SceneLoader.TransitionScene(EnumCollection.Scene.SCENE_TYPE.MAIN);
        }
    }
}
