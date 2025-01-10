using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using System;
using System.Collections;
using UnityEditor.Searcher;

namespace ProjectCronos
{
    /// <summary>
    /// ドロップアイテム
    /// </summary>
    public class DropItem : MonoBehaviour
    {
        /// <summary>
        /// 当たり判定
        /// </summary>
        Collider col;

        int tempItemId;
        int tempItemAmount;

        [SerializeField]
        VisualEffect[] appearItemEffect;

        [SerializeField]
        VisualEffect getItemEffect;

        void Start()
        {
            col = GetComponent<Collider>();
            col.gameObject.SetActive(false);
            SetActiveEffect(false);
        }

        public void Init(int itemId, int itemAmount)
        {
            tempItemId = itemId;
            tempItemAmount = itemAmount;
            col.gameObject.SetActive(true);
            SetActiveEffect(true);
        }

        void SetActiveEffect(bool result)
        {
            foreach (var effect in appearItemEffect)
            {
                effect.gameObject.SetActive(result);
            }

            getItemEffect.gameObject.SetActive(result);
        }

        void GetItem(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("GetDropItem");

            // 当たり判定とアイテムエフェクトを消す
            col.enabled = false;
            StartCoroutine(GetItemEffect());

            var playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.itemHolder.AddItem(tempItemId, tempItemAmount);

                MainEntryPoint.guideView.HideControlGuide();
                InputManager.Instance.inputActions.Player.Action.performed -= GetItem;
            }
        }

        IEnumerator GetItemEffect()
        {
            foreach (var effect in appearItemEffect)
            {
                effect.SendEvent("OnStop");
            }

            getItemEffect.SendEvent("OnPlay");

            yield return new WaitForSeconds(2.0f);

            SetActiveEffect(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                MainEntryPoint.guideView.ShowControlGuide(
                    "アイテムを拾う",
                    EnumCollection.Input.INPUT_GAMEPAD_BUTTON.B);
                InputManager.Instance.inputActions.Player.Action.performed += GetItem;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                MainEntryPoint.guideView.HideControlGuide();
                InputManager.Instance.inputActions.Player.Action.performed -= GetItem;
            }
        }
    }
}
