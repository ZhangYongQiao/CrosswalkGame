using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class CurPlayer
{
    private static CurPlayer _instance;
    public static CurPlayer Instance
    {
        get
        {
            if (_instance == null) 
                _instance = new CurPlayer();
            return _instance;
        }
        private set { }
    }

    private CurPlayer() { }

    public Vector3 Pos;
    public int Blood;
    public int Score;
    public string Scene;
}

[Serializable]
public class Player
{
    public Vector3 Pos;
    public int Blood;
    public int Score;
    public string Scene;
}