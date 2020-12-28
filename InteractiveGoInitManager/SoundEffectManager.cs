using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager
{

    private static SoundEffectManager _instance;
    public static SoundEffectManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SoundEffectManager();
            return _instance;
        }
    }

    private AudioSource _soundEffect;
    public AudioSource SoundEffect
    {
        get
        {
            if (_soundEffect == null)
            {
                _soundEffect = GameObject.Find("Audio Source").transform.GetComponent<AudioSource>();
                GameObject.DontDestroyOnLoad(_soundEffect.gameObject);
            }
            return _soundEffect;
        }
    }

    public void PlaySoundEffect()
    {
        SoundEffect.Play();
    }
}
