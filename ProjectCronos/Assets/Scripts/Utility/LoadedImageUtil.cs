using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ProjectCronos
{
    /// <summary>
    /// 読み込み済みの画像を読み込む便利クラス
    /// </summary>
    public static class LoadedImageUtil
    {
        /// <summary>
        /// 指定したゲームパッドのボタンの画像パスを返す
        /// </summary>
        /// <param name="gamepadButtonType">ゲームパットのボタンの種類</param>
        /// <returns>ボタンの画像パス</returns>
        public static string SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON gamepadButtonType) => gamepadButtonType switch
        {
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.A => "button_A_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.B => "button_B_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.X => "button_X_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.Y => "button_Y_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.L => "button_L_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.R => "button_R_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.LB => "button_LB_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.RB => "button_RB_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.LT => "button_LT_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.RT => "button_RT_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.L3 => "button_L3_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.R3 => "button_R3_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.CROSS => "button_cross_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.CROSS_UP => "button_cross_up_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.CROSS_DOWN => "button_cross_down_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.CROSS_LEFT => "button_cross_left_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.CROSS_RIGHT => "button_cross_right_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.JOYSTICK => "dummy_image_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.JOYSTICK_UP => "dummy_image_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.JOYSTICK_DOWN => "dummy_image_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.JOYSTICK_LEFT => "dummy_image_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.JOYSTICK_RIGHT => "dummy_image_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.START => "button_Start_1",
            EnumCollection.Input.INPUT_GAMEPAD_BUTTON.SELECT => "button_Select_1",
            _ => "dummy_image_1"
        };

        /// <summary>
        /// 事前読み込み
        /// </summary>
        /// <returns>UniTask</returns>
        public static async UniTask PreLoadAsync()
        {
            // ここで事前に必要な素材を読み込む
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.A));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.B));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.X));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.Y));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.L));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.R));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.LT));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.RT));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.LB));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.RB));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.L3));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.R3));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.START));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.SELECT));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.CROSS));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.CROSS_UP));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.CROSS_DOWN));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.CROSS_LEFT));
            await AddressableManager.Instance.LoadTexture(SwitchGamepadButtonImagePath(EnumCollection.Input.INPUT_GAMEPAD_BUTTON.CROSS_RIGHT));
        }

        public static Texture2D GetGamepadButtonImageTexture(EnumCollection.Input.INPUT_GAMEPAD_BUTTON gamepadButtonType)
        {
            return AddressableManager.Instance.GetLoadedTextures(SwitchGamepadButtonImagePath(gamepadButtonType));
        }
    }
}
