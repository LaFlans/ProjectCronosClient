using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;

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
        public void Save(PlayerSaveData data, int saveDataNumber)
        {
            var filePath = string.Format(defaultFilePath, saveDataNumber);
            Debug.Log($"現在の状態を保存するよ！{filePath}");
            string json = JsonUtility.ToJson(data);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }

        /// <summary>
        /// 指定の番号のセーブデータを読み込む
        /// </summary>
        /// <param name="saveDataNumber">読み込みたいセーブデータ番号</param>
        /// <returns>読み込んだセーブデータ情報を返す(ファイルが見つからない場合、nullを返す)</returns>
        public PlayerSaveData Load(int saveDataNumber)
        {
            var filePath = string.Format(defaultFilePath, saveDataNumber);
            if (File.Exists(filePath))
            {
                StreamReader streamReader;
                streamReader = new StreamReader(filePath);
                string data = streamReader.ReadToEnd();
                streamReader.Close();
                return JsonUtility.FromJson<PlayerSaveData>(data);
            }

            Debug.LogError($"ファイルが見つからなかったよ…{filePath}");
            return null;
        }
    }
}
