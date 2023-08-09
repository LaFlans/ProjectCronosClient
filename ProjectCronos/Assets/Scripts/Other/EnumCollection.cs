using System.ComponentModel;

namespace ProjectCronos
{
    /// <summary>
    /// Enumの定義を集めた物
    /// </summary>
    namespace EnumCollection
    {
        /// <summary>
        /// 入力関連
        /// </summary>
        namespace Input
        {
            public enum INPUT_STATUS
            {
                /// <summary>
                /// 操作不能
                /// </summary>
                UNCONTROLLABLE,
                /// <summary>
                /// UI側の操作可能
                /// </summary>
                UI,
                /// <summary>
                /// プレイヤー側の操作可能
                /// </summary>
                PLAYER,
                /// <summary>
                /// 要素数
                /// </summary>
                MAXMUM,
            }
        }

        /// <summary>
        /// UI関連
        /// </summary>
        namespace UI
        {
            public enum BAR_SHOW_STATUS
            {
                /// <summary>
                /// 残りの値のみ表示
                /// </summary>
                CURRENT_ONLY,
                /// <summary>
                /// すべて表示
                /// </summary>
                ALL,
                /// <summary>
                /// 要素数
                /// </summary>
                MAXMUM,
            }
        }

        /// <summary>
        /// ゲーム関連
        /// </summary>
        namespace Game
        {
            public enum GAME_STATUS
            {
                /// <summary>
                /// ゲームプレイ中
                /// </summary>
                GAME_PLAY,
                /// <summary>
                /// ゲームクリア
                /// </summary>
                GAME_CLEAR,
                /// <summary>
                /// ゲームオーバー
                /// </summary>
                GAME_OVER,
                /// <summary>
                /// 最大数
                /// </summary>
                MAXMUM,
            }
        }

        /// <summary>
        /// シーン関連
        /// </summary>
        namespace Scene
        {
            public enum SCENE_TYPE
            {
                /// <summary>
                /// すべてのシーンをさす
                /// </summary>
                ALL,
                /// <summary>
                /// タイトルシーン
                /// </summary>
                [Description("TitleScene")]
                TITLE,
                /// <summary>
                /// メインとなるシーン
                /// </summary>
                [Description("MainScene")]
                MAIN,
                /// <summary>
                /// ローディングシーン
                /// </summary>
                [Description("LoadingScene")]
                LOADING,
                /// <summary>
                /// 最大数
                /// </summary>
                MAXMUM,
            }

            /// <summary>
            /// シーンの読み込み状態
            /// </summary>
            public enum SCENE_LOAD_STATUS
            {
                /// <summary>
                /// 待機状態(読み込み前)
                /// </summary>
                WAITING,
                /// <summary>
                /// 読み込み中
                /// </summary>
                LOADING,
                /// <summary>
                /// 読み込み完了
                /// </summary>
                COMPLETE,
                /// <summary>
                /// 最大数
                /// </summary>
                MAXMUM,
            }
        }

        /// <summary>
        /// ポップアップ関連
        /// </summary>
        namespace Popup
        {
            /// <summary>
            /// ポップアップの種類
            /// </summary>
            public enum POPUP_TYPE
            {
                /// <summary>
                /// ベースとなるポップアップ
                /// </summary>
                DEFAULT,
                /// <summary>
                /// アプリケーション終了確認ポップアップ
                /// </summary>
                QUIT_APPLICATION,
                /// <summary>
                /// タイトル遷移確認ポップアップ
                /// </summary>
                TRANSITION_TITLE_CONFIRM,
                /// <summary>
                /// セーブポップアップ
                /// </summary>
                SAVE,
                /// <summary>
                /// 要素数
                /// </summary>
                MAXMUM,
            }

            /// <summary>
            /// ポップアップの選択状態
            /// </summary>
            public enum POPUP_SELECT_STATUS
            {
                /// <summary>
                /// ポジティブの状態
                /// </summary>
                POSITIVE,
                /// <summary>
                /// ネガティブの状態
                /// </summary>
                NEGATIVE,
                /// <summary>
                /// その他の状態
                /// </summary>
                OTHER,
                /// <summary>
                /// 要素数
                /// </summary>
                MAXMUM,
            }

            /// <summary>
            /// ポップアップのボタン状態
            /// </summary>
            public enum POPUP_BUTTON_STATUS
            {
                /// <summary>
                /// ベース(ポジティブとネガティブ)
                /// </summary>
                DEFAULT,
                /// <summary>
                /// すべて
                /// </summary>
                ALL,
                /// <summary>
                /// ポジティブボタンのみ
                /// </summary>
                POSITIVE_ONLY,
                /// <summary>
                /// ネガティブのみ
                /// </summary>
                NEGATIVE_ONLY,
                /// <summary>
                /// その他のみ
                /// </summary>
                OTHER_ONLY,
                /// <summary>
                /// 要素数
                /// </summary>
                MAXMUM,
            }
        }

        /// <summary>
        /// プレイヤー関連
        /// </summary>
        namespace Player
        {
            /// <summary>
            /// ジャンプ状態
            /// </summary>
            public enum PLAYER_JUMP_STATE
            {
                /// <summary>
                /// ジャンプしていない状態
                /// </summary>
                IDOL,
                /// <summary>
                /// ジャンプ開始
                /// </summary>
                START,
                /// <summary>
                /// ジャンプ中
                /// </summary>
                JUMP,
                /// <summary>
                /// 着地
                /// </summary>
                LANDING,
                /// <summary>
                /// 要素数
                /// </summary>
                MAXMUM,
            }
        }

        /// <summary>
        /// スキル関連
        /// </summary>
        namespace Skill
        {
            /// <summary>
            /// 種類
            /// </summary>
            public enum TYPE
            {
                /// <summary>
                /// 攻撃スキル
                /// </summary>
                ATTACK,
                /// <summary>
                /// 補助スキル
                /// </summary>
                SUPPORT,
                /// <summary>
                /// 要素数
                /// </summary>
                MAXMUM,
            }
        }

        /// <summary>
        /// 攻撃(武器・魔法)関連
        /// </summary>
        namespace Attack
        {
            /// <summary>
            /// 攻撃の種類
            /// </summary>
            public enum ATTACK_TYPE
            {
                /// <summary>
                /// プレイヤーの攻撃
                /// </summary>
                PLAYER,
                /// <summary>
                /// 敵の攻撃
                /// </summary>
                ENEMY,
                /// <summary>
                /// 要素数
                /// </summary>
                MAXMUM,
            }

            public enum SKILL_TYPE
            {
                /// <summary>
                /// 時間停止(アガレス)
                /// </summary>
                TIME_STOP,
                /// <summary>
                /// 要素数
                /// </summary>
                MAXMUM,
            }
        }

        /// <summary>
        /// サウンド関連
        /// </summary>
        namespace Sound
        {
            /// <summary>
            /// サウンドの種類
            /// </summary>
            public enum SOUND_TYPE
            {
                /// <summary>
                /// BGM
                /// </summary>
                BGM,
                /// <summary>
                /// SE
                /// </summary>
                SE,
                /// <summary>
                /// 要素数
                /// </summary>
                MAXMUM,
            }
        }
    }
}
