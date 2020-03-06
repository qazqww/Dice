using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType
{
    // Clip 파일명 기입
    bgm_start,
    bgm_board,
    bgm_combat,
    combat_defeat,
    combat_victory,
    logo,
    End
}

public enum SoundType
{
    diceput = BackgroundType.End, // => BackgroundType 끝에 이어서 번호 붙이기
    diceroll,
    dicethrow,
    item_buy,
    item_disrupt,
    item_potion,
    item_use,
    land_desert,
    land_lake,
    land_mine,
    rock_impact_small_hit_01,
    rock_impact_small_hit_02,
    rock_impact_small_hit_03,
    victory,
    voice_female_c_attack_01,
    voice_female_c_attack_03,
    voice_female_c_attack_08,
    voice_female_c_death_02,
    voice_female_c_death_03,
    voice_female_c_death_04,
    walking
}

public class AudioManager : MonoBehaviour
{
    AudioSource background;
    AudioSource uiSource;
    AudioListener listener;
    float volume; // 전체적인 사운드 볼륨 제어, Audio Listener에 사용할 값

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("AudioManager", typeof(AudioManager));
                instance = obj.GetComponent<AudioManager>();

                instance.background = obj.AddComponent<AudioSource>();
                instance.AudioSetting(instance.background);
                instance.uiSource = obj.AddComponent<AudioSource>();
                instance.AudioSetting(instance.uiSource);

                //AudioClip clip = Resources.Load<AudioClip>("");

                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    // 사운드 출력 기본설정 함수
    void AudioSetting(AudioSource audio, bool loop = false)
    {
        if (audio == null)
            return;

        audio.loop = loop;
        audio.playOnAwake = true;
        audio.spatialBlend = 0; // 공간감 0(2D) ~ 1(3D)
        audio.priority = 0;
        audio.pitch = 1;
        audio.panStereo = 0;
        audio.volume = volume;
    }

    public void Play(AudioSource source, string soundType, bool loop = false, float volume = 1.0f)
    {
        if (background != null)
        {
            if (audioClips.ContainsKey(soundType))
            {
                background.clip = audioClips[soundType];
                background.volume = volume;
                background.loop = loop;
                background.Play();
            }
        }
    }

    public void PlayUI(AudioSource source, string soundType, bool loop = false, float volume = 1.0f)
    {
        if (uiSource != null)
        {
            if (audioClips.ContainsKey(soundType))
            {
                uiSource.clip = audioClips[soundType];
                uiSource.volume = volume;
                uiSource.loop = loop;
                uiSource.Play();
            }
        }
    }

    public void PlayBackground(string bgType, bool loop = true, float volume = 0.5f)
    {
        Play(background, bgType, loop, volume);
    }

    public void PlayBackground(BackgroundType bgType, bool loop = true, float volume = 0.5f)
    {
        Play(background, bgType.ToString(), loop, volume);
    }

    public void PlayUISound(string soundType, bool loop = false, float volume = 0.5f)
    {
        PlayUI(uiSource, soundType, loop, volume);
    }

    public void PlayUISound(SoundType soundType, bool loop = false, float volume = 0.5f)
    {
        PlayUI(uiSource, soundType.ToString(), loop, volume);
    }

    public void AddClip(string soundType, string path)
    {
        if (!audioClips.ContainsKey(soundType))
        {
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip != null)
                audioClips.Add(soundType, clip);
        }
    }

    public void LoadClip<T>(string path)
    {
        T[] files = (T[])Enum.GetValues(typeof(T));

        for (int i = 0; i < files.Length; i++)
            AddClip(files[i].ToString(), path + files[i].ToString());
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetPause(bool state)
    {
        AudioListener.pause = state;
    }

    // 특정 사운드 파일의 볼륨이 유독 크거나 작을 경우, 따로 관리할 수 있는 함수
    public void SetEffectVolume(string path, float volume)
    {
        //if (sourceDic.ContainsKey(path))
        //    sourceDic[path].volume = volume;
    }
}
