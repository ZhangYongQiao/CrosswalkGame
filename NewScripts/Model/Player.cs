using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class CurPlayer
{
    public static Vector3 Pos;
    public static byte Blood;
    public static ushort Score;
    public static string Scene;
}

[Serializable]
public class Player
{
    public Vector3 Pos;
    public byte Blood;
    public ushort Score;
    public string Scene;

    public Player(Vector3 pos,byte blood,ushort score,string scene)
    {
        Pos = pos;
        Blood = blood;
        Score = score;
        Scene = scene;
    }
}