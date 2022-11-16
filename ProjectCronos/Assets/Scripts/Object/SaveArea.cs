using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjectCronos
{
    public class SaveArea : MonoBehaviour
    {
        [SerializeField]
        GameObject saveAreaPopup;

        bool canSave;

        [SerializeField]
        Transform respawnPos;

        /// <summary>
        /// セーブポイントのID(ユニーク)
        /// </summary>
        int saveAreaId;

        /// <summary>
        /// セーブポイント名
        /// </summary>
        string savePointName;

        void Start()
        {
            canSave = false;
            saveAreaPopup.SetActive(false);
        }

        void Update()
        {
            if (canSave && Input.GetKeyUp(KeyCode.S))
            {
                SaveManager.Instance.Save(PlayerSaveData.Create(saveAreaId), 0);
            }
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                //Debug.Log("SaveArea内に入ったよ");
                saveAreaPopup.SetActive(true);
                canSave = true;
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                //Debug.Log("SaveAreaを出たよ");
                saveAreaPopup.SetActive(false);
                canSave = false;
            }
        }
    }
}
