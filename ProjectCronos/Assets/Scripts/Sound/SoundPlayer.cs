using System.Collections;
using System.Collections.Generic;using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
namespace ProjectCronos{    /// <summary>    /// サウンドプレイヤー    /// </summary>    internal class SoundPlayer : MonoBehaviour    {        const int SOURCE_BGM_NUM = 2;        const int SOURCE_SE_NUM = 16;        float masterVolume = 1.0f;
        float bgmVolume = 0.0f;
        float seVolume = 1.0f;        List<SoundSource> bgmSources;        List<SoundSource> seSources;        [SerializeField]        Transform sourceSeParent;        [SerializeField]        Transform sourceBgmParent;

        Coroutine FadeInOutCoroutine;

        /// <summary>        /// 初期化        /// </summary>        public bool Init()        {            // リスト初期化            bgmSources = new List<SoundSource>();            seSources = new List<SoundSource>();            // SEのSoundSourceを作成            for (int i = 0; i < SOURCE_SE_NUM; i++)            {                var obj = new GameObject("SoundSource").AddComponent<SoundSource>();                obj.transform.SetParent(sourceSeParent);                obj.Init();                seSources.Add(obj);            }            // BGMのSoundSourceを作成            for (int i = 0; i < SOURCE_BGM_NUM; i++)            {                var obj = new GameObject("SoundSource").AddComponent<SoundSource>();                obj.transform.SetParent(sourceBgmParent);                obj.Init();                bgmSources.Add(obj);            }            // サウンドソースのボリューム設定            SetSourceVolume();
            return true;        }        /// <summary>        /// BGM再生        /// </summary>        /// <param name="key">再生したいサウンドデータのキー</param>        public void Play(string key)        {            var data = MasterDataManager.DB.SoundTable.FindByKey(key);            switch ((EnumCollection.Sound.SOUND_TYPE)data.Type)            {                case EnumCollection.Sound.SOUND_TYPE.BGM:                    PlayBGM(data.Path);                    break;                case EnumCollection.Sound.SOUND_TYPE.SE:                    PlaySE(data.Path);                    break;                case EnumCollection.Sound.SOUND_TYPE.MAXMUM:                    Debug.Log($"BGMかSEを指定してね！KEY:{data.Key}");                    break;            }        }        /// <summary>        /// マスターボリュームを設定する        /// </summary>        /// <param name="val">設定する値</param>        public void SetMasterVolume(float val)        {            masterVolume = val;            // サウンドソースのボリューム設定            SetSourceVolume();        }        /// <summary>        /// BGM再生        /// </summary>        /// <param name="path">対象のリソースのAddressableパス</param>        async void PlayBGM(string path)        {            var clip = AddressableManager.Instance.GetLoadedClip(path);

            //　nullチェック
            if (clip == null)            {                Debug.Log("再生しようとしているクリップがnullだよ…");                return;            }

            if (bgmSources.Any(x => x.isMain) && bgmSources.Count > 1)
            {
                var next = bgmSources.Where(x => !x.isMain).First();
                next.Play(clip);

                if (FadeInOutCoroutine == null)
                {
                    FadeInOutCoroutine = StartCoroutine(FadeBGM());
                }
                else
                {
                    StopCoroutine(FadeInOutCoroutine);
                    FadeInOutCoroutine = StartCoroutine(FadeBGM());
                }
            }
            else
            {
                var source = bgmSources.FirstOrDefault();
                if (source != null)
                {
                    source.isMain = true;
                    source.Play(clip);
                }
            }
        }

        IEnumerator FadeBGM()
        {
            var fadeInSeconds = 5.0f;
            var fadeDeltaTime = 0.0f;

            var current = bgmSources.Where(x => x.isMain).First();
            var next = bgmSources.Where(x => !x.isMain).First();

            var tempCurrentVolume = current.GetVolume();
            var tempNextVolume = next.GetVolume();
            var currentTargetVolume = 0.0f;
            var nextTargetVolume = masterVolume * bgmVolume;

            current.isMain = false;
            next.isMain = true;

            var rate = 0.0f;
            while(rate < 1.0f)
            {
                fadeDeltaTime += Time.deltaTime;
                rate = (float)(fadeDeltaTime / fadeInSeconds);
                current.SetVolume(tempCurrentVolume * (float)(1.0f - rate));
                next.SetVolume(((nextTargetVolume - tempNextVolume) * rate) + tempNextVolume);
                yield return null;
            }

            current.SetVolume(currentTargetVolume);
            next.SetVolume(nextTargetVolume);
            current.Stop();

            // フェード処理終了時にコルーチンをnullにする
            FadeInOutCoroutine = null;
        }
        /// <summary>        /// SE再生        /// </summary>        /// <param name="path">対象のリソースのAddressableパス</param>        void PlaySE(string path)        {            var clip = AddressableManager.Instance.GetLoadedClip(path);            //　nullチェック            if (clip == null)            {                Debug.Log("再生しようとしているクリップがnullだよ…");                return;            }            foreach (var item in seSources)            {                if (!item.IsPlaying())                {                    item.Play(clip);                    break;                }            }        }        /// <summary>        /// サウンドソースのボリューム設定        /// </summary>        void SetSourceVolume()        {
            var volume = masterVolume;

//#if UNITY_EDITOR
//            var settings = ProjectCronosSettings.GetSerializedSettings();
//            var result = settings.FindProperty("IsMuteSound").boolValue;
//            if (result)
//            {
//                volume = 0.0f;
//            }
//#endif

            foreach (var item in bgmSources)            {                item.SetVolume(volume * bgmVolume);            }            foreach (var item in seSources)            {                item.SetVolume(volume * seVolume);            }        }    }}