using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CurMonster
{
    private static CurMonster _instance;
    public static CurMonster Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CurMonster();
            return _instance;
        }
        private set { }
    }

    private CurMonster() { }

    public List<Transform> PosList;
}

[Serializable]
public class Monster
{
    public List<Transform> PosList;
}

