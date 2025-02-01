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
using System.Threading;
using UnityEngine.EventSystems;

namespace ProjectCronos
{
    public class ScenarioView : MonoBehaviour
    {
        enum ViewType
        {
            Message, // メッセージ
            Choice,  // 選択肢
        }

        ViewType currentViewType;

        [SerializeField]
        float delayDuration = 0.01f;

        [SerializeField]
        GameObject container;

        [SerializeField]
        GameObject messageBoxContainer;

        [SerializeField]
        GameObject nameBoxContainer;

        [SerializeField]
        GameObject choiceContainer;

        [SerializeField]
        ScenarioChoiceButton[] choiceButtons;

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

        int talkNum = 0;
        int selectIndex = 0;
        int choiceCount = 0;
        string playerName;
        bool isChoice = false;
        bool isChoiceStart = false;
        Action onCompleted;
        Coroutine showCoroutine;
        Coroutine choiceCoroutine;
        List<string> messages;
        Dictionary<string, string> textKey;

        private void Start()
        {
            playerName = "ノア";
            textKey = new Dictionary<string, string>
            {
                { "playerName", playerName }
            };

            container.SetActive(false);
            nameBoxContainer.SetActive(false);
            messageBoxContainer.SetActive(false);
            choiceContainer.SetActive(false);
            currentViewType = ViewType.Message;
            nameText.text = string.Empty;
            messageText.text = string.Empty;
        }

        public void ShowScenario(string loadScenarioTitle, Action onCompleted)
        {
            // プレイヤー操作可能状態だった場合、UI操作可能状態にする
            if (InputManager.Instance.IsMatchInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER))
            {
                InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.UI);
            }

            RegisterInputActions();

            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>().StopPlayerMove();

            this.onCompleted = onCompleted;
            messages = ScenarioManager.LoadScenarioScene(loadScenarioTitle).ToList();

            if (messages.Count < 0)
            {
                return;
            }

            container.SetActive(true);

            NextMessage();
        }

        void NextMessage()
        {            
            if (choiceCoroutine != null)
            {
                // 選択肢が出ている間は何もしない
                return;
            }

            SoundManager.Instance.Play("Button47");

            if (showCoroutine != null)
            {
                messageText.maxVisibleCharacters = lastMessage.Length;
                messageText.text = lastMessage;
                talkNum++;
                StopCoroutine(showCoroutine);
                showCoroutine = null;

                return;
            }

            // 次のセリフがない場合、初期化して戻す
            if (talkNum + 1 > messages.Count)
            {
                UnregisterInputActions();

                // UI操作可能状態だった場合、プレイヤー操作可能状態にする
                if (InputManager.Instance.IsMatchInputStatus(EnumCollection.Input.INPUT_STATUS.UI))
                {
                    InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER);
                }

                Init();

                return;
            }

            ReadMessage();
        }

        void Init()
        {
            onCompleted?.Invoke();
            onCompleted = null;
            container.SetActive(false);
            talkNum = 0;
            nameText.text = string.Empty;
            messageText.text = string.Empty;
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
                else if(line.Contains("choice"))
                {
                    // 選択肢
                    InitChoice();

                    line = line.Replace("choice=", "");

                    // まず選択肢の文言を取得
                    var str = line.Remove(0, line.IndexOf("[") + 1);
                    var choiceMessages = str.Remove(str.IndexOf("]")).Split(',');

                    // 必要のなくなった分を削除
                    line = line.Remove(0, str.IndexOf("[") + 1);

                    // 次に選択肢を選んだ時のコマンドを取得
                    str = line.Remove(0, line.IndexOf("[") + 1);
                    var actionMessages = str.Remove(str.IndexOf("]")).Split(',');

                    choiceCoroutine = StartCoroutine(Choice(choiceMessages, actionMessages));
                    return;
                }

                talkNum++;
                NextMessage();
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
                        line = line.Replace(key, textKey[key]).Replace("{", "").Replace("}", "");
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

        void InitChoice()
        {
            foreach (var button in choiceButtons)
            {
                button.gameObject.SetActive(false);
            }
        }

        void SetViewType(ViewType viewType)
        {
            currentViewType = viewType;

            var IsExistMessage = messageText.text != string.Empty;
            nameBoxContainer.SetActive(nameText.text != string.Empty && IsExistMessage);
            messageBoxContainer.SetActive(IsExistMessage);
            choiceContainer.SetActive(currentViewType != ViewType.Message);
        }

        IEnumerator Choice(string[] choiceMessages, string[] actionCommands)
        {
            SetViewType(ViewType.Choice);

            selectIndex = 0;
            choiceCount = choiceMessages.Length;

            ApplySelectChoiceStatus();

            var index = 0;
            foreach (var message in choiceMessages)
            {
                choiceButtons[index].gameObject.SetActive(true);
                choiceButtons[index].Init(
                    message,
                    () =>
                    {
                        // FIXME: 最初に選択肢が出る時に呼ばれてしまい、即次のメッセージに行くので一旦ここに来る一回目は何もしないようにしている
                        if (isChoiceStart)
                        {
                            isChoice = true;
                        }

                        isChoiceStart = true;
                    });

                index++;
            }

            yield return new WaitUntil(() => isChoice == true);

            messages = ScenarioManager.LoadScenarioScene(actionCommands[selectIndex]);
            talkNum = 0;
            choiceCoroutine = null;
            isChoiceStart = false;
            isChoice = false;

            NextMessage();
        }

        IEnumerator Dialogue(string message)
        {
            var delay = new WaitForSeconds(delayDuration);
            var length = message.Length;
            lastMessage = message;
            messageText.text = message;

            SetViewType(ViewType.Message);

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

        /// <summary>
        /// 選択
        /// </summary>
        /// <param name="context"></param>
        void OnSubmit(InputAction.CallbackContext context)
        {
            if (currentViewType == ViewType.Message)
            {
                NextMessage();
            }
            else
            {
                if (choiceCoroutine != null)
                {
                    SoundManager.Instance.Play("Button47");
                    isChoice = true;
                }
            }
        }

        /// <summary>
        /// 上選択
        /// </summary>
        /// <param name="context"></param>
        void OnUp(InputAction.CallbackContext context)
        {
            if (currentViewType != ViewType.Choice)
            {
                return;
            }

            selectIndex--;
            if (selectIndex < 0)
            {
                selectIndex = choiceCount - 1;
            }

            SoundManager.Instance.Play("Button47");
            ApplySelectChoiceStatus();
        }

        /// <summary>
        /// 下選択
        /// </summary>
        /// <param name="context"></param>
        void OnDown(InputAction.CallbackContext context)
        {
            if (currentViewType != ViewType.Choice)
            {
                return;
            }

            selectIndex++;
            if (selectIndex > (choiceCount - 1))
            {
                selectIndex = 0;
            }

            SoundManager.Instance.Play("Button47");
            ApplySelectChoiceStatus();
        }

        /// <summary>
        /// 選択肢の選択状態を更新
        /// </summary>
        void ApplySelectChoiceStatus()
        {
            foreach (var button in choiceButtons.Select((item, index) => new { item, index }))
            {
                if (button.index == selectIndex)
                {
                    button.item.SelectButton();
                }
            }
        }

        public void RegisterInputActions()
        {
            InputManager.Instance.inputActions.UI.Submit.performed += OnSubmit;
            InputManager.Instance.inputActions.UI.Up.performed += OnUp;
            InputManager.Instance.inputActions.UI.Down.performed += OnDown;
        }

        public void UnregisterInputActions()
        {
            InputManager.Instance.inputActions.UI.Submit.performed -= OnSubmit;
            InputManager.Instance.inputActions.UI.Up.performed -= OnUp;
            InputManager.Instance.inputActions.UI.Down.performed -= OnDown;
        }
    }
}
