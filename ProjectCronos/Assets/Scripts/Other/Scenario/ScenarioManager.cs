using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MessagePack;

namespace ProjectCronos
{
    public class ScenarioManager : MonoBehaviour
    {
        const string DIRECTORY_PATH = "Assets/ProjectCronosAssets/ScenarioScenes/";
        const string SCENARIO_FILE_EXTENSION = ".asset";

        /// <summary>
        /// 指定のシナリオデータを読み込みます
        /// 読み込みに失敗したらnullを返します
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public static List<string> LoadJsonScenarioScene(string sceneId)
        {
            string path = DIRECTORY_PATH + sceneId + SCENARIO_FILE_EXTENSION;

            if (File.Exists(path))
            {
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var stream = new StreamReader(fileStream, Encoding.UTF8);
                var json = stream.ReadToEnd();

                //// 複合化
                //var decrypted = EncryptUtil.DecryptStringFromStringAes(json);

                var bytes = MessagePackSerializer.ConvertFromJson(json);
                var result = MessagePackSerializer.Deserialize<List<string>>(bytes);


                stream.Close();

                return result;
            }

            Debug.Log($"シナリオファイルが見つからなかったよ…{path}");
            return null;
        }

        public static bool SaveJsonScenarioScene(string sceneId, List<string> scenarioData)
        {
            string path = DIRECTORY_PATH + sceneId + SCENARIO_FILE_EXTENSION;

            var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            var json = MessagePackSerializer.SerializeToJson(scenarioData);

            //// 暗号化
            //var encrypted = EncryptUtil.EncryptStringToStringAes(json);

            streamWriter.Write(json);
            streamWriter.Close();

            return true;
        }

        public static bool IsExistScenarioScene(string sceneId)
        {
            string path = DIRECTORY_PATH + sceneId + SCENARIO_FILE_EXTENSION;
            return File.Exists(path);
        }
    }
}
