using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ProjectCronos
{
    class PopupManager : Singleton<PopupManager>
    {
        public class Param
        {
            public PopupBase popup;

            public Param(PopupBase popup)
            {
                this.popup = popup;
            }
        }

        [SerializeField]
        Stack<Param> popupParams;

        PopupView popupView;

        /// <summary>
        /// ポップアッププレハブ取得
        /// 表示するキャンバスの親オブジェクトがない場合、nullを返す
        /// </summary>
        /// <param name="type">ポップアップの種類</param>
        /// <returns>読み込まれたプレハブオブジェクト</returns>
        public GameObject GetPopupObject(EnumCollection.Popup.POPUP_TYPE type)
        {
            var obj = AddressableManager.Instance.GetLoadedObject(GetPopupPath(type));
            var parent = GameObject.Find("PopupParent");
            obj.transform.parent = parent.transform;
            obj.transform.localPosition = Vector3.zero;
            popupView = parent.GetComponent<PopupView>();
            PushPopup(new Param(obj.GetComponent<PopupBase>()));
            return obj;
        }

        public void ShowSystemPopup(PopupBase.MessageParam messageParam, Action closeCallback = null)
        {
            var obj = PopupManager.Instance.GetPopupObject(
                EnumCollection.Popup.POPUP_TYPE.TRANSITION_TITLE_CONFIRM);

            if (obj != null)
            {
                obj.GetComponent<PopupBase>().Setup(
                    EnumCollection.Popup.POPUP_BUTTON_STATUS.POSITIVE_ONLY,
                    new PopupBase.Param(closeAction:closeCallback),
                    messageParam);
            }
        }

        string GetPopupPath(EnumCollection.Popup.POPUP_TYPE type)
        {
            switch(type)
            {
                case EnumCollection.Popup.POPUP_TYPE.DEFAULT:
                    return "Assets/Resources_moved/Prefabs/Popup/DefaultPopup.prefab";
                case EnumCollection.Popup.POPUP_TYPE.QUIT_APPLICATION:
                    return "Assets/Resources_moved/Prefabs/Popup/DefaultPopup.prefab";
                case EnumCollection.Popup.POPUP_TYPE.TRANSITION_TITLE_CONFIRM:
                    return "Assets/Resources_moved/Prefabs/Popup/DefaultPopup.prefab";
                case EnumCollection.Popup.POPUP_TYPE.SAVE:
                    return "Assets/Resources_moved/Prefabs/Popup/SavePopup.prefab";
                case EnumCollection.Popup.POPUP_TYPE.SHOP:
                    return "Assets/Resources_moved/Prefabs/Popup/ShopPopup.prefab";
                default:
                    // デフォルトのポップアップのパスを返す
                    return "Assets/Resources_moved/Prefabs/Popup/DefaultPopup.prefab";  
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>初期化に成功したかどうか</returns>
        public override async UniTask<bool> Initialize()
        {
            popupParams = new Stack<Param>();

            // ポップアッププレハブの事前ロード
            for (int i = 0; i < (int)EnumCollection.Popup.POPUP_TYPE.MAXMUM; i++)
            {
                await AddressableManager.Instance.Load(GetPopupPath((EnumCollection.Popup.POPUP_TYPE)i));
            }

            Debug.Log("PopupManager初期化");

            return true;
        }

        /// <summary>
        /// ポップアップをスタックする
        /// </summary>
        /// <param name="param">対象のポップアップ</param>
        public void PushPopup(Param param)
        {
            if (popupParams.Count > 0)
            {
                popupParams.Peek().popup.UnregisterInputActions();
            }

            popupParams.Push(param);
            param.popup.RegisterInputActions();
            
            // ポップアップを表示した時、プレイヤー操作可能状態だった場合、UI操作可能状態にする
            if (InputManager.Instance.IsMatchInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER))
            {
                InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.UI);
                TimeManager.Instance.StopTime();
                popupView.SetActiveUIObjects(false);
            }
        }

        /// <summary>
        /// 一番上のポップアップを取り除く
        /// </summary>
        public void PopPopup()
        {
            if (popupParams.Count > 0)
            {
                popupParams.Peek().popup.UnregisterInputActions();
                popupParams.Pop();

                if (popupParams.Count > 0)
                {
                    popupParams.Peek().popup.RegisterInputActions();
                }
            }

            // ポップアップを取り除いた時、UI操作可能状態だった場合、プレイヤー操作可能状態にする
            if (popupParams.Count == 0 && InputManager.Instance.IsMatchInputStatus(EnumCollection.Input.INPUT_STATUS.UI))
            {
                InputManager.Instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER);
                TimeManager.Instance.ApplyTimeScale();
                popupView.SetActiveUIObjects(true);
            }
        }

        /// <summary>
        /// 現在表示されているポップアップの数を返す
        /// </summary>
        /// <returns></returns>
        public int GetPopupCount()
        {
            return popupParams.Count();
        }
    }
}
