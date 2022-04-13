using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace ProjectCronos
{
    public class Utility : MonoBehaviour
    {
        #region Addressables
        /// <summary>
        /// Addressableなゲームオブジェクトを生成
        /// </summary>
        /// <param name="path">アドレス</param>
        public static void CreateObject(string path)
        {
            Addressables.LoadAssetAsync<GameObject>(path).Completed += op => { Instantiate(op.Result); };
        }

        /// <summary>
        /// Addressableなゲームオブジェクトを生成
        /// </summary>
        /// <param name="path">アドレス</param>
        /// <param name="callback">読み込み完了時コールバック</param>
        public static void CreateObject(string path, Action<GameObject> callback)
        {
            Addressables.LoadAssetAsync<GameObject>(path).Completed += op => {
                Instantiate(op.Result);
                callback(op.Result);
            };
        }

        /// <summary>
        /// Addressableなゲームオブジェクトを生成
        /// </summary>
        /// <param name="path">アドレス</param>
        /// <param name="callback">読み込み完了時コールバック</param>
        /// <param name="parent">プレハブの親にするオブジェクト</param>
        public static void CreateObject(string path, Action<GameObject> callback, Transform parent)
        {
            Addressables.LoadAssetAsync<GameObject>(path).Completed += op => {
                Instantiate(op.Result, parent);
                callback(op.Result);
            };
        }

        /// <summary>
        /// Addressableなゲームオブジェクトを生成
        /// </summary>
        /// <param name="path">アドレス</param>
        /// <param name="pos">オブジェクトを生成する位置</param>
        public static void CreateObject(string path, Vector3 pos)
        {
            Addressables.LoadAssetAsync<GameObject>(path).Completed += op =>
            {
                Instantiate(op.Result);
                op.Result.transform.position = pos;
            };
        }

        /// <summary>
        /// Addressableなゲームオブジェクトを生成
        /// </summary>
        /// <param name="path">アドレス</param>
        /// <param name="pos">オブジェクトを生成する位置</param>
        /// <param name="time">プレハブを消す時間</param>
        public static void CreateObject(string path, Vector3 pos, float time)
        {
            Addressables.LoadAssetAsync<GameObject>(path).Completed += op =>
            {
                GameObject obj = Instantiate(op.Result);
                obj.transform.position = pos;
                Destroy(obj, time);
            };
        }

        /// <summary>
        /// Addressableなゲームオブジェクトを生成
        /// </summary>
        /// <param name="path">アドレス</param>
        /// <param name="pos">オブジェクトを生成する位置</param>
        /// <param name="rotation">プレハブを生成する角度</param>
        public static void CreateObject(string path, Vector3 pos, Quaternion rotation)
        {
            Addressables.LoadAssetAsync<GameObject>(path).Completed += op =>
            {
                Instantiate(op.Result);
                op.Result.transform.position = pos;
                op.Result.transform.rotation = rotation;
            };
        }

        /// <summary>
        /// Addressableなゲームオブジェクトを生成
        /// </summary>
        /// <param name="path">アドレス</param>
        /// <param name="transform">生成するオブジェクトのトランスフォーム</param>
        public static void CreateObject(string path, Transform transform)
        {
            Addressables.LoadAssetAsync<GameObject>(path).Completed += op =>
            {
                GameObject obj = Instantiate(op.Result);
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.transform.localScale = transform.localScale;
            };
        }

        #endregion

        #region Prefab

        /// <summary>
        /// スクリーン座標でのプレハブを生成
        /// </summary>
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="parent">プレハブの親にするオブジェクト</param>
        public static void CreateUIPrefab(string path, Vector3 pos, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            Vector3 rectPos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
            Instantiate(prefab, rectPos, Quaternion.identity, parent.transform);
        }

        /// <summary>
        /// スクリーン座標でのプレハブを生成
        /// </summary>
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="time">プレハブを消す時間</param>
        /// <param name="parent">プレハブの親にするオブジェクト</param>
        public static void CreateUIPrefab(string path, Vector3 pos, float time, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            Vector3 rectPos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
            GameObject obj = Instantiate(prefab, rectPos, Quaternion.identity, parent.transform);
            Destroy(obj, time);
        }

        /// <summary>
        /// プレハブのサウンドを生成して指定した時間後に消す
        /// <param name="path">サウンドプレハブのパス</param>
        /// <param name="time">プレハブを消す時間</param>
        /// </summary>
        public static void CreateSE(string path, float time)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab);
            Destroy(obj, time);
        }

        /// <summary>
        ///　プレハブを生成してゲームオブジェクトを返す
        /// </summary>
        /// <param name="path"> プレハブのパス</param>
        /// <returns>プレハブのゲームオブジェクト</returns>
        public static GameObject CreatePrefab(string path)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            return Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// プレハブを生成して指定した時間後に消す
        /// <param name="path">プレハブのパス</param>
        /// <param name="time">プレハブを消す時間</param>
        /// </summary>
        public static void CreatePrefab(string path, float time)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            Destroy(obj, time);
        }

        /// <summary>
        /// プレハブを指定した位置に生成して指定した時間後に消す
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="time">プレハブを消す時間</param>
        /// </summary>
        public static void CreatePrefab(string path, Vector3 pos, float time)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            Destroy(obj, time);
        }

        /// <summary>
        /// プレハブを指定した位置に生成して指定した親をセットして指定した時間後に消す
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="time">プレハブを消す時間</param>
        /// <param name="parent">プレハブの親にするオブジェクト</param>
        /// </summary>
        public static void CreatePrefab(string path, Vector3 pos, float time, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity, parent.transform);
            Destroy(obj, time);
        }

        /// <summary>
        /// プレハブを指定した位置と回転で生成して指定した時間後に消す
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="rotation">プレハブを生成する角度</param>
        /// <param name="time">プレハブを消す時間</param>
        /// </summary>
        public static void CreatePrefab(string path, Vector3 pos, Quaternion rotation, float time)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, rotation);
            Destroy(obj, time);
        }
        /// <summary>
        /// プレハブを指定した位置と回転で生成して指定した時間後に消す
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="rotation">プレハブを生成する角度</param>
        /// <param name="time">プレハブを消す時間</param>
        /// <param name="parent">プレハブの親にするオブジェクト</param>
        /// </summary>
        public static void CreatePrefab(string path, Vector3 pos, Quaternion rotation, float time, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, rotation, parent.transform);
            Destroy(obj, time);
        }

        /// <summary>
        /// プレハブを指定した位置と回転で生成する
        /// </summary>
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="rotation">プレハブを生成する角度</param>
        public static void CreatePrefab(string path, Vector3 pos, Quaternion rotation)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            Instantiate(prefab, pos, rotation);
        }

        /// <summary>
        /// プレハブを指定した位置とオイラー角で生成する
        /// </summary>
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="eularAngle">プレハブを生成する角度(オイラー角)</param>
        public static void CreatePrefab(string path, Vector3 pos, Vector3 eularAngle)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.transform.eulerAngles = eularAngle;
        }

        /// <summary>
        /// プレハブを指定した位置とオイラー角で生成する
        /// </summary>
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="eularAngle">プレハブを生成する角度(オイラー角)</param>
        /// <param name="parent">プレハブの親にするオブジェクト</param>
        public static void CreatePrefab(string path, Vector3 pos, Vector3 eularAngle, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity, parent.transform);
            obj.transform.eulerAngles = eularAngle;
        }
        /// <summary>
        /// プレハブを指定した位置とオイラー角で生成する
        /// </summary>
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="eularAngle">プレハブを生成する角度(オイラー角)</param>
        /// <param name="time">プレハブを消す時間</param>
        public static void CreatePrefab(string path, Vector3 pos, Vector3 eularAngle, float time)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.transform.eulerAngles = eularAngle;
            Destroy(obj, time);
        }


        /// <summary>
        /// プレハブを指定した位置で生成する
        /// </summary>
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        public static void CreatePrefab(string path, Vector3 pos)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            Instantiate(prefab, pos, Quaternion.identity);
        }
        /// <summary>
        /// プレハブを指定した位置に生成して指定した親をセットして指定した時間後に消す
        /// <param name="path">プレハブのパス</param>
        /// <param name="time">プレハブを消す時間</param>
        /// <param name="parent">プレハブの親にするオブジェクト</param>
        /// </summary>
        public static void CreatePrefab(string path, float time, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
            Destroy(obj, time);
        }
        /// <summary>
        /// プレハブを指定した位置に生成して指定した親をセットして指定した時間後に消す
        /// <param name="path">プレハブのパス</param>
        /// <param name="pos">プレハブを生成する位置</param>
        /// <param name="parent">プレハブの親にするオブジェクト</param>
        /// </summary>
        public static void CreatePrefab(string path, Vector3 pos, GameObject parent)
        {
            GameObject prefab = (GameObject)Resources.Load(path);
            Instantiate(prefab, pos, Quaternion.identity, parent.transform);
        }

        #endregion

        #region Clone
        //****************************************************************************************************************************************************************************************
        //　クローン
        //****************************************************************************************************************************************************************************************
        /// <summary>
        /// ゲームオブジェクトのクローンを生成する
        /// </summary>
        /// <param name="go"></param>
        /// <returns>クローンゲームオブジェクト</returns>
        public static GameObject CreateClone(GameObject go)
        {
            var clone = GameObject.Instantiate(go) as GameObject;
            clone.transform.localPosition = go.transform.localPosition;
            clone.transform.localScale = go.transform.localScale;
            return clone;
        }
        #endregion

        #region Material
        /// <summary>
        /// ターゲットの子オブジェクト含むすべてのオブジェクトのマテリアルカラーを指定した色に変える
        /// </summary>
        /// <param name="targetObject">ターゲットオブジェクト</param>
        /// <param name="color">変更したい色</param>
        public static void ChangeMaterialOfGameObject(GameObject targetObject, Material mat)
        {
            //入力されたオブジェクトのRendererを全て取得し、さらにそのRendererに設定されている全Materialを変える
            foreach (Renderer targetRenderer in targetObject.GetComponents<Renderer>())
            {
                targetRenderer.material = mat;
                //foreach (Material material in targetRenderer.materials)
                //{
                //    material.color = color;
                //}
            }

            //入力されたオブジェクトの子にも同様の処理を行う
            for (int i = 0; i < targetObject.transform.childCount; i++)
            {
                ChangeMaterialOfGameObject(targetObject.transform.GetChild(i).gameObject, mat);
            }
        }
        /// <summary>
        /// ターゲットの子オブジェクト含むすべてのオブジェクトのマテリアルカラーを指定した色に変える
        /// </summary>
        /// <param name="targetObject">ターゲットオブジェクト</param>
        /// <param name="color">変更したい色</param>
        public static void ChangeColorOfGameObject(GameObject targetObject, Color color)
        {
            //入力されたオブジェクトのRendererを全て取得し、さらにそのRendererに設定されている全Materialの色を変える
            foreach (Renderer targetRenderer in targetObject.GetComponents<Renderer>())
            {
                foreach (Material material in targetRenderer.materials)
                {
                    material.color = color;
                }
            }

            //入力されたオブジェクトの子にも同様の処理を行う
            for (int i = 0; i < targetObject.transform.childCount; i++)
            {
                ChangeColorOfGameObject(targetObject.transform.GetChild(i).gameObject, color);
            }
        }
        /// <summary>
        /// ターゲットの子オブジェクト含むすべてのオブジェクトの指定したマテリアルカラーを指定した色に変える
        /// </summary>
        /// <param name="targetObject">ターゲットオブジェクト</param>
        /// <param name="colorName">変更したいカラーの名前</param>
        /// <param name="color">変更したい色</param>
        public static void ChangeColorOfGameObject(GameObject targetObject, string colorName, Color color)
        {
            //入力されたオブジェクトのRendererを全て取得し、さらにそのRendererに設定されている全Materialの色を変える
            foreach (Renderer targetRenderer in targetObject.GetComponents<Renderer>())
            {

                foreach (Material material in targetRenderer.materials)
                {
                    material.SetColor(colorName, color);
                }
            }

            //入力されたオブジェクトの子にも同様の処理を行う
            for (int i = 0; i < targetObject.transform.childCount; i++)
            {
                ChangeColorOfGameObject(targetObject.transform.GetChild(i).gameObject, colorName, color);
            }
        }



        #endregion

        #region IEnumerator

        //　目的地(target)と目的地までの時間(time)
        public static IEnumerator ToTargetPos(GameObject obj, Vector3 target, float time)
        {
            Vector3 startPos = obj.transform.position;
            Vector3 endPos = target;

            float startTime = Time.time; //　到着予定時間
            var diff = Time.time - startTime;
            while (time > diff)
            {
                diff = Time.time - startTime; //　経過時間
                var rate = diff / time;
                obj.transform.position = Vector3.Lerp(startPos, endPos, rate);
                yield return null;
            }
            obj.transform.position = new Vector3(endPos.x, endPos.y, endPos.z);

            yield return null;
        }
        //　目的地(target)と目的地までの時間(time)
        public static IEnumerator ToTargetLookPos(GameObject obj, Vector3 target, float time)
        {
            Vector3 startPos = obj.transform.position;
            Vector3 endPos = target;

            float startTime = Time.time; //　到着予定時間
            var diff = Time.time - startTime;
            while (time > diff)
            {
                diff = Time.time - startTime; //　経過時間
                var rate = diff / time;
                obj.transform.position = Vector3.Lerp(startPos, endPos, rate);

                obj.transform.LookAt(target);
                yield return null;
            }
            obj.transform.position = new Vector3(endPos.x, endPos.y, endPos.z);

            yield return null;
        }

        //　フェードイン
        public static IEnumerator FadeIn(Image fadeImage, float speed)
        {
            while (fadeImage.color.a > 0)
            {
                fadeImage.color -= new Color(0, 0, 0, speed);
                yield return new WaitForSeconds(0.01f);
            }
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
            yield return null;
        }
        //　フェードアウト
        public static IEnumerator FadeOut(Image fadeImage, float speed)
        {
            while (fadeImage.color.a < 1)
            {
                fadeImage.color += new Color(0, 0, 0, speed);
                yield return new WaitForSeconds(0.01f);
            }
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            yield return null;
        }
        #endregion

        #region UI
        /// <summary>
        /// ターゲットの子オブジェクト含むすべてのオブジェクトのテキストカラーを指定した色に変える
        /// </summary>
        /// <param name="targetText">ターゲットテキスト</param>
        /// <param name="color">変更したい色</param>
        public static void ChangeColorOfText(GameObject targetText, Color color)
        {
            //入力されたオブジェクトのTextを全て取得し、テキストカラーを変える
            foreach (Text target in targetText.GetComponents<Text>())
            {
                target.color = color;
            }

            //入力されたオブジェクトの子にも同様の処理を行う
            for (int i = 0; i < targetText.transform.childCount; i++)
            {
                ChangeColorOfGameObject(targetText.transform.GetChild(i).gameObject, color);
            }
        }

        #endregion

        #region Scene

        /// <summary>
        /// すでにシーンが読み込まれているかを返す
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsAlreadyLoadScene(string name)
        {
            var count = SceneManager.sceneCount;
            for (int i = 0; i < count;i++)
            {
                if (SceneManager.GetSceneAt(i).name == name)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Application
        /// <summary>
        /// アプリケーション終了処理
        /// </summary>
        public static void ApplicationQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#endif
        }
        #endregion
    }
}