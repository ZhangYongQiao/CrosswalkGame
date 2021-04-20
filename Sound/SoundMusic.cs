using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMusic : MonoBehaviour
{
    public AudioClip mainPanel;
    public AudioClip oneLevel;

    public AudioSource AudioSource;

    private void Awake()
    {
        AudioSource.playOnAwake = true;
        MessageCenter.Instance.Register(MessageName.OnPlaySoundBgm, PlaySoundBgmHandler);
        MessageCenter.Instance.Register(MessageName.OnAutoSaveSoundBgmValue, OnAutoSaveSoundBgmValueHandler);
    }

    private void OnAutoSaveSoundBgmValueHandler(MessageData obj)
    {

    }

    private void OnDestroy()
    {
        MessageCenter.Instance.Remove(MessageName.OnPlaySoundBgm, PlaySoundBgmHandler);

    }

    private void PlaySoundBgmHandler(MessageData obj)
    {
        LevelBgm one = (LevelBgm)obj._data;
        switch (one)
        {
            case LevelBgm.MainPanel:
                AudioSource.clip = mainPanel;
                break;
            case LevelBgm.one:
                AudioSource.clip = oneLevel;
                break;
            default:
                Log.Error("Bgm播放出错");
                break;

        }
        AudioSource.Play();
    }


}




public enum LevelBgm
{
    MainPanel = 1,
    one = 2
}
