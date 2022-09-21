using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ProjectCronos
{
    class PopupManager : ISingleton<PopupManager>
    {
        public class Param
        {
            public GameObject obj;

            public Param(GameObject obj)
            {
                this.obj = obj;
            }
        }

        GameObject parentCanvas;

        Stack<Param> popupParams;

        /// <summary>
        /// ポップアッププレハブ取得
        /// 表示するキャンバスの親オブジェクトがない場合、nullを返す
        /// </summary>
        /// <param name="type">ポップアップの種類</param>
        /// <returns>読み込まれたプレハブオブジェクト</returns>
        public GameObject GetPopupObject(EnumCollection.Popup.POPUP_TYPE type)
        {
            if (parentCanvas == null)
            {
                parentCanvas = GameObject.Find("PopupParent");
                if (parentCanvas == null)
                {
                    //　一度探して該当するオブジェクトがない場合、何もしない
                    return null;
                }
            }

            var obj = Instantiate(AddressableManager.Instance.GetLoadedObject(GetPopupPath(type)), parentCanvas.transform);
            PushPopup(new Param(obj));
            return obj;
        }

        string GetPopupPath(EnumCollection.Popup.POPUP_TYPE type)
        {
            switch(type)
            {
                case EnumCollection.Popup.POPUP_TYPE.DEFAULT:
                    return "Assets/Resources_moved/Prefabs/Popup/DefaultPopup.prefab";
                case EnumCollection.Popup.POPUP_TYPE.QUIT_APPLICATION:
                    return "Assets/Resources_moved/Prefabs/Popup/ApplicationQuitPopup.prefab";
                case EnumCollection.Popup.POPUP_TYPE.TRANSITION_TITLE_CONFIRM:
                    return "Assets/Resources_moved/Prefabs/Popup/TransitionTitleConfirmPopup.prefab";
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
            // 表示対処のキャンパスを設定
            if (parentCanvas == null)
            {
                parentCanvas = GameObject.Find("PopupParent");
            }

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
            popupParams.Push(param);

            // ポップアップを表示した時、プレイヤー操作可能状態だった場合、UI操作可能状態にする
            if (InputManager.instance.IsMatchInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER))
            {
                InputManager.instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.UI);
                TimeManager.instance.StopTime();
            }
        }

        /// <summary>
        /// 一番上のポップアップを取り除く
        /// </summary>
        public void PopPopup()
        {
            popupParams.Pop();

            if (popupParams.Count == 0 && InputManager.instance.IsMatchInputStatus(EnumCollection.Input.INPUT_STATUS.UI))
            {
                InputManager.instance.SetInputStatus(EnumCollection.Input.INPUT_STATUS.PLAYER);
                TimeManager.instance.ApplyTimeScale();
            }
        }

        /// <summary>
        /// 現在表示されているポップアップの数を返す
        /// </summary>
        /// <returns></returns>
        public int PopupCount()
        {
            return popupParams.Count();
        }

        /// <summary>
        /// ポップアップ描画対象のキャンバスを設定
        /// </summary>
        /// <param name="target">対象となるキャンバス</param>
        public void SetParentCanvas(GameObject target)
        {
            parentCanvas = target;
        }
    }
}
