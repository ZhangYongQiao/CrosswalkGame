using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Music
{
    public List<MusicPathInfo> _musicInfos;
}

[System.Serializable]
public class MusicPathInfo
{
    public string _sceneName;
    public string _clipName;
}
