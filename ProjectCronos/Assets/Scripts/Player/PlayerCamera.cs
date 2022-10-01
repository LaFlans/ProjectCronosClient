using UnityEngine;
using Cinemachine;

namespace ProjectCronos
{
    class PlayerCamera : MonoBehaviour
    {
        const int DEFAULT_FREE_LOOK_CAMERA_PRIORITY = 10;
        const int DEFAULT_ROCKON_CAMERA_PRIORITY = 5;

        /// <summary>
        /// 自由視点カメラ
        /// </summary>
        [SerializeField]
        CinemachineFreeLook freeLookCamera;

        /// <summary>
        /// ロックオンカメラ
        /// </summary>
        [SerializeField]
        CinemachineVirtualCamera rockOnCamera;

        /// <summary>
        /// ロックオンカメラの高さ
        /// </summary>
        [SerializeField]
        float rockOnCamerahHeight = 2;

        [SerializeField]
        float dist = 2;

        [SerializeField, Range(0f, 1f)]
        float cameraMoveSpeed = 0.1f;

        /// <summary>
        /// ロックオン中かどうか
        /// </summary>
        bool isRockOn;

        /// <summary>
        /// プレイヤーのトランスフォーム情報
        /// </summary>
        Transform playerTransform;

        /// <summary>
        /// ターゲットのトランスフォーム情報
        /// </summary>
        Transform targetTransform;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="player">プレイヤーのトランスフォームを取得</param>
        public void Initialize(Transform playerTransform)
        {
            isRockOn = false;
            this.playerTransform = playerTransform;
            SetPriority();
        }

        void FixedUpdate()
        {
            if (isRockOn)
            {
                if (targetTransform == null)
                {
                    CancelRockOn();
                    return;
                }

                RockOnTarget();
            }
        }

        /// <summary>
        /// 対象の敵にロックオンする
        /// </summary>
        public void RockOn(Transform target)
        {
            Debug.Log("ロックオンをするよ");
            targetTransform = target;
            SetPriority(rockOnPriority: 15);
            isRockOn = true;
        }

        /// <summary>
        /// ロックオンを解除する
        /// </summary>
        public void CancelRockOn()
        {
            Debug.Log("ロックオンを解除したよ");
            SetPriority(rockOnPriority: 5);
            isRockOn = false;
        }

        /// <summary>
        /// ロックオン状態かどうか
        /// </summary>
        /// <returns>ロックオン中かどうかを返す</returns>
        public bool IsRockOn()
        {
            return isRockOn;
        }

        /// <summary>
        /// 自由操作カメラの操作が有効か設定
        /// </summary>
        public void EnableFreeLookCamera()
        {
            if (freeLookCamera != null)
            {
                freeLookCamera.GetComponent<CinemachineInputProvider>().enabled =
                    InputManager.Instance.IsMatchInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER);
            }
        }

        /// <summary>
        /// カメラの優先度設定
        /// 値を何も設定しない場合、デフォルトの値が設定される
        /// </summary>
        /// <param name="freeLookPriority">自由操作カメラの優先度</param>
        /// <param name="rockOnPriority">ロックオンカメラの優先度</param>
        void SetPriority(int? freeLookPriority = null, int? rockOnPriority = null)
        {
            freeLookCamera.Priority = freeLookPriority ?? DEFAULT_FREE_LOOK_CAMERA_PRIORITY;
            rockOnCamera.Priority = rockOnPriority ?? DEFAULT_ROCKON_CAMERA_PRIORITY;
        }

        void RockOnTarget()
        {
            if (playerTransform == null || targetTransform == null)
            {
                // ターゲットもしくはプレイヤーのトランスフォーム情報がない場合、何も行わない
                return;
            }

            var vec = Vector3.Normalize(targetTransform.position - this.transform.position);
            var diff = this.transform.position - (vec * dist);

            // 目標地点へゆっくり移動させる
            rockOnCamera.transform.position = Vector3.Lerp(
                rockOnCamera.transform.position,
                new Vector3(diff.x, playerTransform.position.y + rockOnCamerahHeight, diff.z),
                cameraMoveSpeed);

            // 向きの設定
            rockOnCamera.transform.LookAt(targetTransform);
        }
    }
}
