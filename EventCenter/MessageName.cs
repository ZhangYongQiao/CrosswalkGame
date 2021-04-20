using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MessageName
{
    //自动隐藏UI
    public const string OnAutoHide = "OnAutoHide";
    //播放音效
    public const string OnPlaySoundEffect = "OnPlaySoundEffect";
    //播放音乐
    public const string OnPlaySoundBgm = "OnPlaySoundBgm";
    //自动保存声音大小值
    public const string OnAutoSaveSoundEffectValue = "OnAutoSaveSoundValue";
    public const string OnAutoSaveSoundBgmValue = "OnAutoSaveSoundValue";
    //发送跳转场景事件
    public const string OnToloadScene = "OnToLoadScene";
    //通知读取声音大小
    public const string OnNoticeSetVolume = "OnNoticeSetVolume";
    //得分
    public const string OnAddScore = "OnAddScore";
}
