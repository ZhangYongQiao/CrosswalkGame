using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GemCherryInfos
{
    [System.NonSerialized]
    private static GemCherryInfos _instance;
    public static GemCherryInfos Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GemCherryInfos();
            return _instance;
        }
    }

    public string _sceneName;

    public List<Vector3> _cherryVecLists;
    public List<Vector3> _gemVecLists;
}
