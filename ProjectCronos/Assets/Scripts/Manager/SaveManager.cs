using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        /// 新規データ作成
        /// </summary>
        public async UniTask CreateNewData()
        {
            PlayerSaveData playerData = new PlayerSaveData(0, null);
            SaveAreaInfo info = new SaveAreaInfo(0, Vector3.zero, Quaternion.identity);
            StageSaveData stageData = new StageSaveData(new List<int>());
            var data = new SaveData(0, Utility.GetUnixTime(DateTime.Now), playerData, info, stageData);
            await Save(data, 0, () => { Debug.Log("初期データを追加しました"); });
            lastLoadSaveData = data;
        }

        /// <summary>
        /// データを保存
        /// </summary>
        /// <param name="data">プレイヤーの保存したいデータ</param>
        /// <param name="saveDataNumber">保存先のセーブデータ番号</param>
        public async UniTask Save(SaveData data, int saveDataNumber, Action callback)
        {
            var filePath = string.Format(defaultFilePath, saveDataNumber);
            Debug.Log($"現在の状態を保存！{filePath}");
            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            var json = MessagePackSerializer.SerializeToJson(data);
            await streamWriter.WriteAsync(json);
            streamWriter.Close();

            callback?.Invoke();
        }

        /// <summary>
        /// 指定の番号のセーブデータを読み込む
        /// </summary>
        /// <param name="saveDataNumber">読み込みたいセーブデータ番号</param>
        /// <returns>読み込んだセーブデータ情報を返す(ファイルが見つからない場合、nullを返す)</returns>
        public async UniTask<SaveData> Load(int saveDataNumber)
        {
            var filePath = string.Format(defaultFilePath, saveDataNumber);
            if (File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var stream = new StreamReader(fileStream, Encoding.UTF8);
                var json = await stream.ReadToEndAsync();
                var bytes = MessagePackSerializer.ConvertFromJson(json);
                var result = MessagePackSerializer.Deserialize<SaveData>(bytes);
                stream.Close();
                lastLoadSaveData = result;

                return result;
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
