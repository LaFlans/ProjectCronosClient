using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    public class ScenarioView : MonoBehaviour
    {
        [SerializeField]
        float delayDuration = 0.1f;

        private Coroutine showCoroutine;

        List<string> messages;

        [SerializeField]
        GameObject container;

        /// <summary>
        /// 名前テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI nameText;

        /// <summary>
        /// メッセージテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI messageText;

        [SerializeField]
        Button talkButton;

        int talkNum = 0;

        Action callback;

        private void Start()
        {
            container.SetActive(false);
            nameText.text = "？？？";
            messages = new List<string>()
                {
                    "ああああああああああああああああああああああああああああああああああああああああああああああああああああああああああ" ,
                    "うおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおおお！！！！！",
                    "なんか買ええええええええええええええええええええええええええええええええええええええええええええええええええええええええええええええええええええええええええ！！！",
                    "まいど"};

            talkButton.onClick.AddListener(() => Show(() => { }));
        }

        public void Show(Action callback)
        {
            if (messages.Count < 0)
            {
                return;
            }

            if (showCoroutine != null)
            {
                messageText.maxVisibleCharacters = messages[talkNum].Length;
                messageText.text = messages[talkNum];
                talkNum = talkNum + 1;
                StopCoroutine(showCoroutine);
                showCoroutine = null;

                return;
            }

            // 次のセリフがない場合、初期化して戻す

            if (talkNum + 1 > messages.Count)
            {
                callback?.Invoke();
                callback = null;
                container.SetActive(false);
                talkNum = 0;
                Debug.LogError("次のセリフがありません");
                return;
            }

            this.callback = callback;
            SoundManager.Instance.Play("Button47");
            container.SetActive(true);
            messageText.text = messages[talkNum];
            showCoroutine = StartCoroutine(Dialogue());
        }

        IEnumerator Dialogue()
        {
            //// 半角スペースで文字を分割する。
            //var words = messages[talkNum].Split(' ');

            //foreach (var word in words)
            //{
            //    // 0.1秒刻みで１文字ずつ表示する。
            //    messageText.text = messageText.text + word;
            //    yield return new WaitForSeconds(0.3f);
            //}

            var delay = new WaitForSeconds(delayDuration);

            var length = messages[talkNum].Length;

            for (var i = 0; i < length; i++)
            {
                messageText.maxVisibleCharacters = i;

                // 一文字ごとに0.2秒待機
                yield return delay;
            }

            messageText.maxVisibleCharacters = length;

            // 次のセリフをセットする。
            talkNum = talkNum + 1;

            showCoroutine = null;
        }
    }
}
