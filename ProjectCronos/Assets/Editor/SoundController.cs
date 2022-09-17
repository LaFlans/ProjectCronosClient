using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace ProjectCronos
{
    [InitializeOnLoad]
    public static class SoundController
    {
        static bool isMute;
        static ProjectCronosSettings settings;

        static SoundController()
        {
            settings = AssetDatabase.LoadAssetAtPath<ProjectCronosSettings>(ProjectCronosSettings.settingsPath);
            isMute = settings.IsMuteSound;

            ToolbarExtender.RightToolbarGUI.Add(OnClickMuteButton);
        }

        /// <summary>
        /// ミュートボタンを推した時の処理
        /// </summary>
        public static void OnClickMuteButton()
        {
            if (TextButton(isMute ? "Mute[ON]" : "Mute[OFF]"))
            {
                settings.IsMuteSound = !settings.IsMuteSound;

                isMute = settings.IsMuteSound;

                //ダーティとしてマークする(変更があった事を記録する)
                EditorUtility.SetDirty(settings);

                //保存する
                AssetDatabase.SaveAssets();
            }

            GUILayout.FlexibleSpace();
        }

        /// <summary>
        /// テキスト付きのボタンを表示する
        /// </summary>
        /// <param name="text">表示する文字列</param>
        /// <returns>ボタン</returns>
        private static bool TextButton(string text)
        {
            return GUILayout.Button(text, GUILayout.Height(22));
        }
    }
}
