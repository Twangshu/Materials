using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cloudtian;

public class AudioManager :MonoBehaviour
{
    private static AudioManager _instance;
    public List<AudioClip> AudioClipList = new List<AudioClip>();                 

    private static Dictionary<string, AudioClip> AudioDic = new Dictionary<string, AudioClip>(); 
    private static AudioSource audioBGM;                  
    private static AudioSource[] audioSources;
    private UnityAction<string,float> cachePlay;
    private string cacheClipName;
    private float cacheSpeed;


    public Slider volumeSlider;
    public float Volume { get; set; }
    public static AudioManager Instance { get => _instance;  }

    public void Awake()
    {
        _instance = this;
     //   volumeSlider.onValueChanged.AddListener((value) => SetVolume());
        audioBGM = gameObject.AddComponent<AudioSource>();
        audioSources = GetComponents<AudioSource>();

        Volume = 50;
    }

    //public void LoadAudioClips()
    //{
    //    var clips = Resources.LoadAll("music");
    //    foreach (var item in clips)
    //    {
    //        AudioClipList.Add(item as AudioClip); 
    //        AudioDic.Add(item.name, item as AudioClip);
    //    }
    //}

    public void LoadAudioClipComplete(object res)
    {
        var clip = res as AudioClip;
        if(!AudioDic.ContainsKey(cacheClipName))
        {
            AudioClipList.Add(clip);
            AudioDic.Add(cacheClipName, clip);
            cachePlay(cacheClipName, cacheSpeed);
        }
        
    }

    public void PlayEffect(string acPath, float speed = 1)
    {
 
        if (AudioDic.ContainsKey(acPath) && !string.IsNullOrEmpty(acPath))
        {
            AudioClip ac = AudioDic[acPath];
            PlayEffect(ac,speed);
        }
        else
        {
            ResManager.Instance.LoadAsync(acPath, LoadAudioClipComplete);
            cacheClipName = acPath;
            cacheSpeed = speed;
            cachePlay = PlayEffect;
        }
    }

    private void PlayEffect(AudioClip ac,float speed=1)
    {
        if (ac)
        {

            audioSources = gameObject.GetComponents<AudioSource>();
            for (int i = 1; i < audioSources.Length; i++)
            {
                if (audioSources[i].isPlaying&&audioSources[i].clip.Equals(ac))
                {
                    return;
                }
            }
            for (int i = 1; i < audioSources.Length; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    audioSources[i].loop = false;
                    audioSources[i].clip = ac;
                    audioSources[i].volume = Volume;
                    audioSources[i].Play();
                    audioSources[i].pitch = speed;
                    return;
                }
            }

            AudioSource newAs = gameObject.AddComponent<AudioSource>();
            newAs.loop = false;
            newAs.clip = ac;
            newAs.volume = Volume;
            newAs.Play();
        }
    }

    public void StopEffect(string acName)
    {
        AudioDic.TryGetValue(acName, out var ac);
        if (ac == null)
            return;
        audioSources = gameObject.GetComponents<AudioSource>();
        for (int i = 1; i < audioSources.Length; i++)
        {
            if (audioSources[i].isPlaying && audioSources[i].clip.Equals(ac))
            {
                audioSources[i].Stop();
            }
        }
    }

    public void PlayBGM(string acPath,float speed=1)
    {
        if (AudioDic.ContainsKey(acPath) && !string.IsNullOrEmpty(acPath))
        {
            AudioClip ac = AudioDic[acPath];
            PlayBGM(ac,speed);
        }
        else
        {
            ResManager.Instance.LoadAsync(acPath, LoadAudioClipComplete);
            cacheClipName = acPath;
            cacheSpeed = speed;
            cachePlay = PlayBGM;
        }

    }

    private void PlayBGM(AudioClip ac,float speed = 1)
    {
        if (ac)
        {
            audioBGM.clip = ac;
            audioBGM.loop = true;
            audioBGM.volume = Volume;
            audioBGM.pitch = speed;
            audioBGM.Play();
        }
    }

    public void StopPlayBGM()
    {
        audioBGM.Stop();
    }

    public void SetVolume()
    {
        Volume =volumeSlider.value;
        for (int i = 0; i < audioSources.Length; i++)
            audioSources[i].volume = Volume;
    }

    public void PlayClickEffect()
    {
        PlayEffect("Music/SE/click");
    }
}