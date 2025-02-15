using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProjectCronos
{
    internal class ScenarioEditor : EditorWindow
    {
        const string SCENARIO_DIRECTORY_PATH = "Assets/ProjectCronosAssets/ScenarioScenes";

        /// <summary>
        /// シナリオのコマンドタイプ
        /// </summary>
        public enum CommandType
        {
            Message,    // メッセージ
            PlaySE,     // SE再生
            PlayBGM,    // BGM再生
            SetSpeaker, // 喋る人の設定
            Choice,     // 選択肢
            CustomKey,  // カスタムキー
        }

        public class ScenarioCommand
        {
            /// <summary>
            /// シナリオコマンドタイプ
            /// </summary>
            public CommandType type;

            /// <summary>
            /// シナリオテキスト
            /// 表示するメッセージ以外にもアセット名や表示する名前等でも使用する想定　　
            /// </summary>
            public string text;

            /// <summary>
            /// 選択肢コマンド
            /// </summary>
            public List<ScenarioChoiceCommand> choiceCommands;

            public ScenarioCommand(CommandType type, string text, List<ScenarioChoiceCommand> choiceCommands = null)
            {
                this.type = type;
                this.text = text;
                this.choiceCommands = choiceCommands ?? new List<ScenarioChoiceCommand>() { new ScenarioChoiceCommand("message", "command")};
            }
        }

        /// <summary>
        /// シナリオ選択肢コマンド
        /// </summary>
        public class ScenarioChoiceCommand
        {
            /// <summary>
            /// 選択肢の文字
            /// </summary>
            public string message;

            /// <summary>
            /// 選択肢コマンド
            /// </summary>
            public string command;

            public ScenarioChoiceCommand(string message, string command)
            {
                this.message = message;
                this.command = command;
            }
        }

        List<ScenarioCommand> scenarioCommands = new List<ScenarioCommand>();

        private Vector2 scrollPosition = Vector2.zero;
        string useFileName = string.Empty;
        string memoText = string.Empty;

        [MenuItem("Cronos/ScenarioEditor")]
        static void Open()
        {
            var window = GetWindow<ScenarioEditor>();
            window.titleContent = new GUIContent("ScenarioEditor");
        }

        void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope("BOX"))
            {
                ScenarioConfirmView();
                GUILayout.FlexibleSpace();
                ScenarioEditView();
            }
        }

        /// <summary>
        /// シナリオ確認画面
        /// </summary>
        void ScenarioConfirmView()
        {
            GUILayout.TextField(
                string.Join("\n", scenarioCommands.Select(x => GetScenarioCommandConfirmText(x))),
                GUILayout.MinWidth(300),
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true));
        }

        /// <summary>
        /// シナリオ編集画面
        /// </summary>
        void ScenarioEditView()
        {
            using (new EditorGUILayout.VerticalScope("BOX"))
            {
                using (new EditorGUILayout.HorizontalScope("BOX"))
                {
                    GUILayout.FlexibleSpace();

                    var style = new GUIStyle(EditorStyles.label);
                    style.richText = true;
                    string test = $"<b><size=50><color=#4169e1>SCENARIO EDITOR</color></b>";
                    GUILayout.Label(test, style);

                    GUILayout.FlexibleSpace();
                }

                using (new EditorGUILayout.HorizontalScope("BOX"))
                {
                    GUILayout.Label("メモ");
                    if (GUILayout.Button("シナリオフォルダを開く"))
                    {
                        var obj = AssetDatabase.LoadAssetAtPath<Object>("Assets/ProjectCronosAssets/ScenarioScenes/ScenarioPingData.txt");
                        if(obj != null)
                        {
                            EditorGUIUtility.PingObject(obj);
                        }
                    }
                }
                memoText = EditorGUILayout.TextArea(memoText, GUILayout.MinWidth(400), GUILayout.MinHeight(40));

                using (new EditorGUILayout.HorizontalScope("BOX"))
                {
                    using (new EditorGUILayout.HorizontalScope("BOX"))
                    {
                        GUILayout.Label("FileName");
                        useFileName = EditorGUILayout.TextArea(useFileName,GUILayout.ExpandWidth(true));
                    }

                    if (GUILayout.Button("CLEAR"))
                    {
                        scenarioCommands.Clear();
                    }

                    if (GUILayout.Button("LOAD"))
                    {
                        Load();
                    }

                    if (scenarioCommands.Count > 0)
                    {
                        // セーブボタンはコマンドが１つ以上ある時のみ
                        if (GUILayout.Button("SAVE"))
                        {
                            Save();
                        }
                    }
                }

                if (scenarioCommands.Count > 0)
                {
                    using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition))
                    {
                        scrollPosition = scrollView.scrollPosition;

                        int index = -1;
                        var commands = scenarioCommands.ToArray();
                        foreach (var scenarioCommand in commands)
                        {
                            index += 1;
                            CreateScenarioCommandArea(index, scenarioCommand);
                        }
                    }
                }
                else
                {
                    GUILayout.Label("表示する物がないよ！");
                    using (new EditorGUILayout.HorizontalScope("BOX"))
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
                        {
                            scenarioCommands.Add(new ScenarioCommand(CommandType.Message, string.Empty));
                        }
                        GUILayout.FlexibleSpace();
                    }
                }
            }
        }

        void CreateScenarioCommandArea(int index, ScenarioCommand command)
        {
            var guiColor = GUI.backgroundColor;
            GUI.backgroundColor = GetGUIBackgroundColor(command.type);

            using (new EditorGUILayout.HorizontalScope("BOX"))
            {
                GUI.backgroundColor = guiColor;

                var style = new GUIStyle(EditorStyles.label);
                style.richText = true;
                string  test = $"<b><size=50>{index + 1}</b>";
                GUILayout.Label(test, style);

                if (command.type == CommandType.Choice)
                {
                    if (command.choiceCommands != null)
                    {
                        using (new EditorGUILayout.VerticalScope("BOX", GUILayout.ExpandWidth(true)))
                        {
                            foreach (var choiceCommand in command.choiceCommands)
                            {
                                using (new EditorGUILayout.HorizontalScope("BOX", GUILayout.ExpandWidth(true)))
                                {
                                    choiceCommand.message = EditorGUILayout.TextArea(choiceCommand.message, GUILayout.MinWidth(200), GUILayout.MinHeight(20));
                                    choiceCommand.command = EditorGUILayout.TextArea(choiceCommand.command, GUILayout.MinWidth(200), GUILayout.MinHeight(20));
                                }
                            }
                        }

                        using (new EditorGUILayout.VerticalScope("BOX", GUILayout.ExpandWidth(true)))
                        {
                            if (command.choiceCommands.Count < 3)
                            {
                                if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
                                {
                                    command.choiceCommands.Add(new ScenarioChoiceCommand("message", "command"));
                                }
                            }

                            if (command.choiceCommands.Count > 1)
                            {
                                if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                                {
                                    command.choiceCommands.RemoveAt(command.choiceCommands.Count - 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        using (new EditorGUILayout.VerticalScope("BOX", GUILayout.ExpandHeight(true)))
                        {
                            GUILayout.FlexibleSpace();

                            if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
                            {
                                command.choiceCommands = new List<ScenarioChoiceCommand>() { new ScenarioChoiceCommand("message", "command") };
                            }

                            GUILayout.FlexibleSpace();
                        }
                    }
                }
                else
                {
                    using (new EditorGUILayout.VerticalScope("BOX", GUILayout.ExpandWidth(true)))
                    {
                        GUILayout.Label(GetLabelText(command.type));
                        command.text = EditorGUILayout.TextArea(command.text, GUILayout.MinWidth(400), GUILayout.MinHeight(40));
                    }

                    using (new EditorGUILayout.VerticalScope("BOX"))
                    {
                        GUILayout.Label("CommandType");
                        command.type = (CommandType)EditorGUILayout.EnumPopup(command.type, GUILayout.MinWidth(100));
                    }
                }

                using (new EditorGUILayout.VerticalScope("BOX"))
                {
                    if (GUILayout.Button("↑", GUILayout.MaxWidth(50)))
                    {
                        scenarioCommands.Insert(index, new ScenarioCommand(CommandType.Message, string.Empty));
                    }

                    GUILayout.Label("追加", GUILayout.ExpandWidth(true));

                    if (GUILayout.Button("↓", GUILayout.MaxWidth(50)))
                    {
                        scenarioCommands.Insert(index + 1, new ScenarioCommand(CommandType.Message, string.Empty));
                    }
                }

                if (GUILayout.Button("-"))
                {
                    scenarioCommands.Remove(scenarioCommands[index]);
                }
            }
        }

        void Save()
        {
            if (useFileName == string.Empty)
            {
                Debug.LogError("セーブするファイル名を指定してね！");
                return;
            }

            var commands = scenarioCommands
                .Where(x => x.text.Length > 0 || x.type == CommandType.Choice)
                .Select(x => GetScenarioCommandText(x));       
            string path = SCENARIO_DIRECTORY_PATH + "/" + useFileName + ".txt";
            if (File.Exists(path))
            {
                //ダイアログ表示、返り値でOKボタンが押されたかどうかを受け取る
                bool isOK = EditorUtility.DisplayDialog(
                    "確認",
                    "既にあるファイルを上書きしようとしています。上書きしますか？",
                    "OK",
                    "Cancel");
                if (isOK)
                {
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        // 1行目にメモを記載
                        sw.WriteLine($"$memo={memoText}");

                        foreach (var command in commands)
                        {
                            sw.WriteLine(command);
                        }
                    }

                    AssetDatabase.Refresh();
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    foreach (var command in commands)
                    {
                        sw.WriteLine(command);
                    }
                }

                AssetDatabase.Refresh();
            }
        }

        void Load()
        {
            if (useFileName == string.Empty)
            {
                Debug.LogError("読み込むファイル名を指定してね！");
                return;
            }

            string path = SCENARIO_DIRECTORY_PATH + "/" + useFileName + ".txt";
            if (!File.Exists(path))
            {
                Debug.LogError("読み込むファイル名が存在しないよ！");
                return;
            }

            scenarioCommands.Clear();

            // 初期状態でもし無ければメモであることのテンプレ分を入れておく
            memoText = "ここはエディタ上でしか使用しないメモ";

            string line = string.Empty;
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    // エディタ上でしか使用しないメモコマンド
                    if (line.Contains("$memo"))
                    {
                        memoText = line.Replace("$memo=", "");
                        continue;
                    }

                    scenarioCommands.Add(ConvertScenarioCommand(line));
                }
            }
        }

        ScenarioCommand ConvertScenarioCommand(string line)
        {
            if (line.Contains("$"))
            {
                line = line.Replace("$", "");

                if (line.Contains("speaker"))
                {
                    // 名前取得
                    line = line.Replace("speaker=", "");
                    return new ScenarioCommand(CommandType.SetSpeaker, line);
                }
                else if (line.Contains("play_se"))
                {
                    // サウンドを再生
                    line = line.Replace("play_se=", "");
                    return new ScenarioCommand(CommandType.PlaySE, line);
                }
                else if (line.Contains("play_bgm"))
                {
                    // BGMを再生
                    line = line.Replace("play_bgm=", "");
                    return new ScenarioCommand(CommandType.PlayBGM, line);
                }
                else if (line.Contains("choice"))
                {
                    // 選択肢
                    line = line.Replace("choice=", "");

                    // まず選択肢の文言を取得
                    var str = line.Remove(0, line.IndexOf("[") + 1);
                    var choiceMessages = str.Remove(str.IndexOf("]")).Split(',');

                    // 必要のなくなった分を削除
                    line = line.Remove(0, str.IndexOf("[") + 1);

                    // 次に選択肢を選んだ時のコマンドを取得
                    str = line.Remove(0, line.IndexOf("[") + 1);
                    var actionMessages = str.Remove(str.IndexOf("]")).Split(',');

                    var choiceCommands = new List<ScenarioChoiceCommand>();
                    for (int i = 0; i < choiceMessages.Length;i++)
                    {
                        choiceCommands.Add(new ScenarioChoiceCommand(choiceMessages[i], actionMessages[i]));
                    }

                    return new ScenarioCommand(CommandType.Choice, line, choiceCommands);
                }
                else if (line.Contains("custom_key"))
                {
                    // カスタムキー
                    line = line.Replace("custom_key=", "");
                    return new ScenarioCommand(CommandType.CustomKey, line);
                }
            }
            else if (line.Contains("["))
            {
                line = line.Replace("[", "").Replace("]", "");
                return new ScenarioCommand(CommandType.Message, line);
            }

            // 基本ここには来ないはず
            return new ScenarioCommand(CommandType.Message, $"コマンド構文が正しくないよ！:{line}");
        }

        Color GetGUIBackgroundColor(CommandType type) => type switch
        {
            CommandType.Message => GUI.backgroundColor,
            CommandType.SetSpeaker => Color.blue,
            CommandType.PlayBGM => Color.cyan,
            CommandType.PlaySE => Color.cyan,
            CommandType.Choice => Color.red,
            CommandType.CustomKey => Color.green,
            _ => GUI.backgroundColor,
        };

        string GetLabelText(CommandType type) => type switch
        {
            CommandType.Message => "表示するメッセージ",
            CommandType.SetSpeaker => "喋っている人の名前",
            CommandType.PlayBGM => "BGMのAddressableキー",
            CommandType.PlaySE => "SEのAddressableキー",
            CommandType.Choice => "選択肢",
            CommandType.CustomKey => "カスタムキー(現在はintのみ対応しています)",
            _ => string.Empty,
        };

        string GetScenarioCommandText(ScenarioCommand command) => command.type switch
        {
            CommandType.Message => $"[{command.text}]",
            CommandType.SetSpeaker => $"$speaker={command.text}",
            CommandType.PlayBGM => $"$play_bgm={command.text}",
            CommandType.PlaySE => $"$play_se={command.text}",
            CommandType.Choice => ConvertChoiceCommand(command, true),
            CommandType.CustomKey => $"$custom_key={command.text}",
            _ => string.Empty,
        };

        string GetScenarioCommandConfirmText(ScenarioCommand command) => command.type switch
        {
            CommandType.Message => $"{command.text}",
            CommandType.SetSpeaker => $"{command.text}",
            CommandType.PlayBGM => $"{command.text}",
            CommandType.PlaySE => $"{command.text}",
            CommandType.Choice => ConvertChoiceCommand(command, false),
            CommandType.CustomKey => $"{command.text}",
            _ => string.Empty,
        };

        string ConvertChoiceCommand(ScenarioCommand command, bool showCommand)
        {
            var messages = command.choiceCommands.Select(x => $"{x.message}").ToArray();
            var commands = command.choiceCommands.Select(x => $"{x.command}").ToArray();
            return (showCommand ? "$choice=" : "")
                + $"[{string.Join(",",messages)}][{string.Join(",",commands)}]";
        }
    }
}
