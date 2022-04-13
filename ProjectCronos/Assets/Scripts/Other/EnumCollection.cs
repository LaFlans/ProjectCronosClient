namespace ProjectCronos
{
    /// <summary>
    /// Enumの定義を集めた物
    /// </summary>
    namespace EnumCollection
    {   
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
                TITLE,
                /// <summary>
                /// メインとなるシーン
                /// </summary>
                MAIN,
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
    }
}
