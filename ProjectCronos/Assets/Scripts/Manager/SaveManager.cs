using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;
using System;
using MessagePack;

namespace ProjectCronos
{
    /// <summary>
    /// セーブ機能管理クラス
    /// </summary>
    class SaveManager : Singleton<SaveManager>
    {
        public static readonly int SAVE_DATA_COUNT = 3;
        string defaultFilePath;

        /// <summary>
        /// 最後に読み込んだセーブデータ
        /// </summary>
        public SaveData lastLoadSaveData { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        public override async UniTask<bool> Initialize()
        {
            defaultFilePath = Application.persistentDataPath + "/" + "savedata{0}.json";
            Debug.Log("SaveManager初期化");

            return true;
        }

        /// <summary>
        /// データを保存
        /// </summary>
        /// <param name="data">プレイヤーの保存したいデータ</param>
        /// <param name="saveDataNumber">保存先のセーブデータ番号</param>
        public void Save(SaveData data, int saveDataNumber, Action callback)
        {
            var filePath = string.Format(defaultFilePath, saveDataNumber);
            Debug.Log($"現在の状態を保存！{filePath}");
            string json = JsonUtility.ToJson(data);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();

            callback?.Invoke();
        }

        /// <summary>
        /// 指定の番号のセーブデータを読み込む
        /// </summary>
        /// <param name="saveDataNumber">読み込みたいセーブデータ番号</param>
        /// <returns>読み込んだセーブデータ情報を返す(ファイルが見つからない場合、nullを返す)</returns>
        public SaveData Load(int saveDataNumber)
        {
            var filePath = string.Format(defaultFilePath, saveDataNumber);
            if (File.Exists(filePath))
            {
                StreamReader streamReader;
                streamReader = new StreamReader(filePath);
                string data = streamReader.ReadToEnd();
                streamReader.Close();
                var saveData = JsonUtility.FromJson<SaveData>(data);

                // 最後に読み込んだセーブデータ情報を保存
                lastLoadSaveData = saveData;

                return saveData;
            }

            Debug.Log($"ファイルが見つからなかったよ…{filePath}");
            return default;
        }

        /// <summary>
        /// 指定の番号のセーブデータを削除
        /// </summary>
        /// <param name="saveDataNumber">削除したいセーブデータ番号</param>
        public void Delete(int saveDataNumber)
        {
            var filePath = string.Format(defaultFilePath, saveDataNumber);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"ファイルを削除したよ！{filePath}");
            }

            Debug.Log($"ファイルが見つからなかったよ…{filePath}");
        }
    }
}
