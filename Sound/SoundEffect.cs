using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioClip BtnClickEff;
    public AudioClip EnterPointerEff;
    public AudioClip PopWindowEff;
    public AudioClip RewardEff;
    public AudioClip MonsterDie;

    public AudioSource AudioSource;

    private void Awake()
    {
        MessageCenter.Instance.Register(MessageName.OnPlaySoundEffect, PlaySoundEffectHandler);
        MessageCenter.Instance.Register(MessageName.OnAutoSaveSoundEffectValue, SaveSoundEffectValueHandler);
    }

    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageName.OnPlaySoundEffect, PlaySoundEffectHandler);
        MessageCenter.Instance.Remove(MessageName.OnAutoSaveSoundEffectValue, SaveSoundEffectValueHandler);
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
        EffectType type = (EffectType)data._data;
        switch (type)
        {
            case EffectType.Pop:
                AudioSource.clip = PopWindowEff;
                break;
            case EffectType.Button:
                AudioSource.clip = BtnClickEff;
                break;
            case EffectType.ButtonEnter:
                AudioSource.clip = EnterPointerEff;
                break;
            case EffectType.GetReward:
                AudioSource.clip = RewardEff;
                break;
            case EffectType.Win:
                break;
            case EffectType.MonsterDie:
                AudioSource.clip = MonsterDie;
                break;
            default:
                Log.Error("音效消息出错");
                break;
        }
        AudioSource.Play();
    }
}

enum EffectType
{
    Pop = 1,
    Button = 2,
    ButtonEnter = 4,
    GetReward = 8,
    Win = 16,
    MonsterDie = 32
}
