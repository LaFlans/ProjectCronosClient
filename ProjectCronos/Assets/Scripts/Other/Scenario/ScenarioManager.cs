using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine.InputSystem;

namespace ProjectCronos
{
    public class ScenarioManager : MonoBehaviour
    {
        const string DIRECTORY_PATH = "Assets/ProjectCronosAssets/ScenarioScenes/";
        const string SCENARIO_FILE_EXTENSION = ".txt";

        public static List<string> LoadScenarioScene(string sceneId)
        {
            string path = DIRECTORY_PATH + sceneId + SCENARIO_FILE_EXTENSION;
            string line = string.Empty;
            List<string> texts = new List<string>();
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("Shift_JIS")))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    texts.Add(line);
                }
            }

            return texts;
        }
    }
}
