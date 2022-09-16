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
            InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.UI);
            InputManager.Instance.inputActions.UI.Submit.performed += OnClickSubmit;

            // BGMの再生
            SoundManager.Instance.Play("WorkBGM1");

            return true;
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
            Debug.Log("次のシーンを読み込むよ");

            // ローディングシーンを読み込む
            SceneLoader.LoadScene("LoadingScene");

            // 次のシーンを読み込む
            SceneLoader.LoadScene("MainScene");

            // 現在のシーンをアンロード
            SceneLoader.UnloadScene("TitleScene");

            // ローディングシーンをアンロード
            SceneLoader.UnloadScene("LoadingScene");
        }
    }
}
