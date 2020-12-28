using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataRunTime
{
    private static PlayerDataRunTime _instance;
    public static PlayerDataRunTime Instance 
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerDataRunTime();
            return _instance;
        }
    }

    private int _initBlood;
    public int InitBlood
    {
        get
        {
            _initBlood = 5;
            return _initBlood;
        }
    }

    private int _initScore;
    public int InitScore
    {
        get
        {
            _initScore = 0;
            return _initScore;
        }
    }

    private Vector3 _initPos;
    public Vector3 InitPos
    {
        get
        {
            _initPos = new Vector3(0,2,0);
            return _initPos;
        }
    }


    public int _curBlood = 5;

    public int _curScore = 0;

    public string _curScene;

    public Vector3 _curPos;
}
