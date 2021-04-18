using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioClip one;
    public AudioClip two;

    public AudioSource AudioSource;

    private void Awake()
    {
        MessageCenter.Instance.Register(MessageName.OnPlaySoundEffect, PlaySoundEffectHandler);
        MessageCenter.Instance.Register(MessageName.OnAutoSaveSoundEffectValue, SaveSoundEffectValueHandler);
        MessageCenter.Instance.Register(MessageName.OnPlayPointerEnterEffect, PlayPointerEnterSoundEffectHandler);
    }

    private void SaveSoundEffectValueHandler(MessageData obj)
    {
        if(obj._type is SoundEffect)
        {
            DataUtility.SetSoundValue("SoundEffect", (int)obj._data);
        }
    }

    public void PlaySoundEffectHandler(MessageData data)
    {
        AudioSource.clip = one;
        AudioSource.Play();
    }

    public void PlayPointerEnterSoundEffectHandler(MessageData data)
    {
        AudioSource.clip = two;
        if(!AudioSource.isPlaying)
            AudioSource.Play();
    }

    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageName.OnPlaySoundEffect, PlaySoundEffectHandler);
        MessageCenter.Instance.Remove(MessageName.OnAutoSaveSoundEffectValue, SaveSoundEffectValueHandler);
        MessageCenter.Instance.Remove(MessageName.OnPlayPointerEnterEffect, PlayPointerEnterSoundEffectHandler);

    }
}
