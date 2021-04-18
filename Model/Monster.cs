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

    public List<Monster> MonsterList;

    //public List<Vector3> PosList;
    //public List<MonsterType> monsterTypeList;
}

[Serializable]
public class Monster
{
    public Vector3 Pos;
    public MonsterType monsterType;
}

public enum MonsterType
{
    Eagle = 1,
    Frog =2,
    Opossum = 4
}

