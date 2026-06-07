using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace MazeGame
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        [SerializeField] private int maxSeNum = 10;
        [SerializeField] private List<AudioSource> seSource;
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioMixerGroup masterAudioMixerGroup;
        [SerializeField] private AudioMixerGroup bgmAudioMixerGroup;
        [SerializeField] private AudioMixerGroup seAudioMixerGroup;
        [Range(0f, 1f)] public float volumeSize = 0.5f;
        [Range(0f, 1f)] public float bgmVolumeSize = 0.5f;
        [Range(0f, 1f)] public float seVolumeSize = 0.5f;

        private int seNum;
        private Coroutine currentFade;
        public bool CanPlaySound { get; set; }


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
            seNum = 0;
            CanPlaySound = false;
            seSource = new List<AudioSource>();
        }

        public void StopBgm()
        {
            if (bgmSource != null)
            {
                bgmSource.Stop();
                bgmSource.clip = null; // メモリ解放のためにクリップも外す
            }
        }

        public void StopAllSe()
        {
            foreach (var source in seSource)
            {
                if (source != null && source.isPlaying)
                {
                    source.Stop();
                    source.clip = null; // メモリ解放
                }
            }
        }
        public void ClearAllSound()
        {
            StopBgm();
            StopAllSe();
        }

        private float DbToNormalized(float db)
        {
            // 10^(db/20) を計算（Mathf.Log10 の逆）
            return Mathf.Pow(10f, db / 20f);
        }

        public void ChangeVolume(float normalizedVolume)
        {
            // 常用対数で人の感覚に合わせる
            float db = Mathf.Log10(Mathf.Clamp(normalizedVolume, 0.0001f, 1f)) * 20f;
            audioMixer.SetFloat("Master", db);
        }

        public void ChangeBgmVolume(float normalizedVolume)
        {
            // 常用対数で人の感覚に合わせる
            float db = Mathf.Log10(Mathf.Clamp(normalizedVolume, 0.0001f, 1f)) * 20f;
            audioMixer.SetFloat("BGMVolume", db);
        }
        public void ChangeSeVolume(float normalizedVolume)
        {
            // 常用対数で人の感覚に合わせる
            float db = Mathf.Log10(Mathf.Clamp(normalizedVolume, 0.0001f, 1f)) * 20f;
            audioMixer.SetFloat("SEVolume", db);
        }

        public void Initialize()
        {
            InitializeMaster();
            InitializeBGM();
            InitializeSE();
        }
        public void InitializeMaster()
        {
            ChangeVolume(1.0f);
        }
        public void InitializeBGM()
        {
            ChangeBgmVolume(1.0f);
        }
        public void InitializeSE()
        {
            ChangeSeVolume(1.0f);
        }

        public float GetMasterVolume()
        {
            float retVal;
            audioMixer.GetFloat("Master", out retVal);
            return DbToNormalized(retVal);
        }
        public float GetSEVolume()
        {
            float retVal;
            audioMixer.GetFloat("SEVolume", out retVal);
            return DbToNormalized(retVal);
        }
        public float GetBGMVolume()
        {
            float retVal;
            audioMixer.GetFloat("BGMVolume", out retVal);
            return DbToNormalized(retVal);
        }

        public void StartBgm(SoundData soundData, bool isFade = false)
        {
            // 同じ曲が既に再生中なら何もしない
            if (bgmSource.isPlaying && bgmSource.clip == soundData.clip) return;

            if (isFade)
            {
                // 既にフェード処理中であれば中断して初期化
                if (currentFade != null)
                {
                    StopCoroutine(currentFade);
                    bgmSource.volume = 1.0f;
                }
                currentFade = StartCoroutine(FadeAndSwitchBgm(soundData));
            }
            else
            {
                // フェードなしの場合は即切り替え
                bgmSource.Stop();
                ApplyBgmSettings(soundData);
                bgmSource.Play();
            }
        }

        private void ApplyBgmSettings(SoundData soundData)
        {
            bgmSource.clip = soundData.clip;
            bgmSource.volume = soundData.volume;
            bgmSource.pitch = soundData.pitch;
            bgmSource.loop = soundData.loop;
            bgmSource.spatialBlend = 0.0f;
            bgmSource.outputAudioMixerGroup = bgmAudioMixerGroup;
        }

        private IEnumerator FadeAndSwitchBgm(SoundData soundData)
        {
            // フェードアウト
            float startVol = bgmSource.volume;
            float duration = 1.0f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startVol, 0, elapsed / duration);
                yield return null;
            }

            // BGMの切り替え
            bgmSource.Stop();
            ApplyBgmSettings(soundData);
            bgmSource.Play();

            // 必要であればここでフェードイン処理を追加可能

            currentFade = null; // 終了後にnullに戻す
        }

        public bool RequestSe(SoundData soundData, Vector3 position, bool is3D = true)
        {
            if (!CanPlaySound) return false;
            // 空いているAudioSourceを探索
            AudioSource freeSource = seSource.FirstOrDefault(s => !s.isPlaying);

            if (freeSource == null)
            {
                if (seNum < maxSeNum)
                {
                    freeSource = gameObject.AddComponent<AudioSource>();
                    freeSource.outputAudioMixerGroup = seAudioMixerGroup;
                    seSource.Add(freeSource);
                    seNum++;
                }
                else
                {
                    // どれも空いていなかった失敗
                    Debug.LogWarning("SEの最大同時再生数を超えました");
                    return false;
                }
            }
            // 空いていたらSeを再生
            freeSource.clip = soundData.clip;
            freeSource.volume = soundData.volume;
            freeSource.pitch = soundData.pitch + UnityEngine.Random.Range(-0.05f, 0.05f);
            freeSource.loop = soundData.loop;

            freeSource.transform.position = position;
            freeSource.spatialBlend = is3D ? 1.0f : 0.0f; 
            freeSource.minDistance = 1f;
            freeSource.maxDistance = 20f;
            freeSource.rolloffMode = AudioRolloffMode.Logarithmic;

            freeSource.Play();
            return true;



        }

    }
}
