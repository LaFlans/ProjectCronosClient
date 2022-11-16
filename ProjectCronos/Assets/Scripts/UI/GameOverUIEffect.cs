using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Cysharp.Threading.Tasks;

namespace ProjectCronos
{
    public class GameOverUIEffect : MonoBehaviour
    {
        [SerializeField]
        Image backgroundImage;
        [SerializeField]
        TextMeshProUGUI gameOverTitle;
        [SerializeField]
        TextMeshProUGUI gameOverMessage;

        /// <summary>
        /// 演出再生中か
        /// </summary>
        bool isPlaying;

        async void Start()
        {
            isPlaying = false;
            SetUIActive(false);
        }

        void Update()
        {

        }

        public async void Apply(Action callback)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                SetUIActive(true);

                await GameOverEffect();
                callback?.Invoke();
            }
        }

        async UniTask GameOverEffect()
        {
            await UniTask.WaitUntil(() => InputManager.Instance.inputActions.UI.Submit.WasPerformedThisFrame());
        }

        void SetUIActive(bool result)
        {
            backgroundImage.gameObject.SetActive(result);
            gameOverTitle.gameObject.SetActive(result);
            gameOverMessage.gameObject.SetActive(result);

            backgroundImage.enabled = result;
            gameOverTitle.enabled = result;
            gameOverMessage.enabled = result;
        }
    }
}
