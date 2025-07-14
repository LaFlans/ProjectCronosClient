using UnityEngine;

namespace ProjectCronos
{
    public class CoinControlButton : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーステータス参照
        /// </summary>
        PlayerStatus playerStatus;

        public enum CalcType
        {
            ADD, // 加算
            SUB, // 減算
            SET  // 設定
        }

        [SerializeField]
        public CalcType calcType;

        [SerializeField]
        public int num;

        public void Initalize()
        {
            // プレイヤー取得、イベント登録
            playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        }

        public void ControlCoin()
        {
            if (playerStatus != null)
            {
                switch (calcType)
                {
                    case CalcType.ADD:
                        SoundManager.Instance.Play("Button47");
                        playerStatus.AddCoin(num);
                        break;
                    case CalcType.SUB:
                        SoundManager.Instance.Play("Button30");
                        playerStatus.SubCoin(-num);
                        break;
                    case CalcType.SET:
                        SoundManager.Instance.Play("Button47");
                        playerStatus.SetCoin(num);
                        break;
                }
            }
        }
    }
}
