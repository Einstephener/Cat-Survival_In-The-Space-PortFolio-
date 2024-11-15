using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioMixer audioMixer;
    private AudioMixerGroup bgmMixer;
    private AudioMixerGroup sfxMixer;
    public List<AudioSource> sfxSources;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SetAudioMixers();
    }

    private void SetAudioMixers()
    {
        bgmMixer = audioMixer.FindMatchingGroups("BGM")[0];
        sfxMixer = audioMixer.FindMatchingGroups("SFX")[0];
    }

    public void PlayBGM(string clipName, float volume)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sound/BGM/" + clipName);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.volume = volume;
            musicSource.loop = true;
            musicSource.outputAudioMixerGroup = bgmMixer;
            musicSource.Play();
        }
    }

    public void PlaySFX(string clipName, float volume)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sound/SFX/" + clipName);
        if (clip != null)
        {
            // 사용 가능한 AudioSource 찾기
            foreach (var source in sfxSources)
            {
                if (!source.isPlaying)
                {
                    source.clip = clip;
                    source.volume = volume;
                    source.loop = false;
                    source.outputAudioMixerGroup= sfxMixer;
                    source.Play();
                    return;
                }
            }
            // 만약 모든 AudioSource가 사용 중이라면, 추가적인 처리 필요 (예: 경고 메시지)
        }
    }
}
