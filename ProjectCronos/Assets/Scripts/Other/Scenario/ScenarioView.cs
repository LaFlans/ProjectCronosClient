using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEditor;

namespace ProjectCronos
{
    public class ScenarioView : MonoBehaviour
    {
        [SerializeField]
        float delayDuration = 0.01f;

        Coroutine showCoroutine;

        List<string> messages;

        [SerializeField]
        GameObject container;

        string playerName;

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

        /// <summary>
        /// 最後に設定したメッセージ
        /// </summary>
        string lastMessage;

        [SerializeField]
        Button talkButton;

        int talkNum = 0;
        Action callback;
        Dictionary<string, string> textKey;

        private void Start()
        {
            playerName = "ノア";
            textKey = new Dictionary<string, string>
            {
                { "playerName", playerName }
            };

            container.SetActive(false);
            nameText.text = "？？？";
            talkButton.onClick.AddListener(() => Show(() => { }));
        }

        public void Show(Action callback)
        {
            messages = ScenarioManager.LoadScenarioScene("002").ToList();

            if (messages.Count < 0)
            {
                return;
            }

            if (showCoroutine != null)
            {
                messageText.maxVisibleCharacters = lastMessage.Length;
                messageText.text = lastMessage;
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
                return;
            }

            this.callback = callback;
            SoundManager.Instance.Play("Button47");
            container.SetActive(true);

            ReadMessage();
        }

        void ReadMessage()
        {
            var line = messages[talkNum];
            if (line.Contains("$"))
            {
                line = line.Replace("$", "");

                if (line.Contains("speaker"))
                {
                    // 名前取得
                    line = line.Replace("speaker=", "");
                    nameText.text = line;
                }
                else if (line.Contains("play_se"))
                {
                    // サウンドを再生
                    line = line.Replace("play_se=", "");
                    SoundManager.Instance.Play(line);
                }
                else if (line.Contains("play_bgm"))
                {
                    // BGMを再生
                    line = line.Replace("play_bgm=", "");
                    SoundManager.Instance.Play(line);
                }

                talkNum++;
                Show(this.callback);
            }
            else if (line.Contains("["))
            {
                line = line.Replace("[", "").Replace("]", "");

                // {}の中身を取得
                if (line.Contains("{"))
                {
                    var str = line.Remove(0, line.IndexOf("{") + 1);
                    var key = str.Remove(str.IndexOf("}"));
                    if (textKey.ContainsKey(key))
                    {
                        line = line.Replace(key, textKey[key]);
                        line = line.Replace("{", "").Replace("}", "");
                    }
                    else
                    {
                        Debug.LogError($"キーが見つからなかったから削除するよ！");
                        line = line.Replace(key, "").Replace("{", "").Replace("}", "");
                    }
                }

                showCoroutine = StartCoroutine(Dialogue(line));
            }
        }

        IEnumerator Dialogue(string message)
        {
            var delay = new WaitForSeconds(delayDuration);
            var length = message.Length;
            lastMessage = message;
            messageText.text = message;

            for (var i = 0; i < length; i++)
            {
                messageText.maxVisibleCharacters = i;

                // 一文字ごとに指定時間待機
                yield return delay;
            }

            messageText.maxVisibleCharacters = length;

            // 次のセリフをセットする。
            talkNum = talkNum + 1;

            showCoroutine = null;
        }
    }
}
