using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource AudioSource;

    private void Awake()
    {
        MessageCenter.Instance.Register(MessageName.OnPlaySoundEffect, PlaySoundEffectHandler);
        MessageCenter.Instance.Register(MessageName.OnAutoSaveSoundEffectValue, SaveSoundEffectValueHandler);
    }

    private void SaveSoundEffectValueHandler(MessageData obj)
    {
        if(obj.data is SoundEffect)
        {
            DataUtility.SetSoundValue("SoundEffect", (int)obj.param);
        }
    }

    public void PlaySoundEffectHandler(MessageData data)
    {
        AudioSource.Play();
    }

    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageName.OnPlaySoundEffect, PlaySoundEffectHandler);
        MessageCenter.Instance.Remove(MessageName.OnAutoSaveSoundEffectValue, SaveSoundEffectValueHandler);
    }
}
