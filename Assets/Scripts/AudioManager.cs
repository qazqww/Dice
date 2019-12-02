using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType
{
    casual_04_loop = 0,
    End
}

public enum SoundType
{
    btn_click = BackgroundType.End, // => BackgroundType 파일 끝에 이어서 번호가 붙게 됨
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
                instance.Setting2D(instance.background);
                instance.uiSource = obj.AddComponent<AudioSource>();
                instance.Setting2D(instance.uiSource);

                //AudioClip clip = Resources.Load<AudioClip>("");

                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    // 사운드 출력 설정 함수
    void Setting2D(AudioSource audio, bool loop = false)
    {
        if (audio == null) return;
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

    public void PlayBackground(string sType, bool loop = true, float volume = 1.0f)
    {
        Play(background, sType, loop, volume);
    }

    public void PlayUISound(string sType, bool loop = false, float volume = 1.0f)
    {
        Play(uiSource, sType, loop, volume);
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
