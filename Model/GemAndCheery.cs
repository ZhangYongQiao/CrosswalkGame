using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CurGem
{
    private static CurGem _instance;
    public static CurGem Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CurGem();
            return _instance;
        }
        private set { }
    }

    private CurGem() { }

    public List<Vector3> PosList;
}

public class CurCherry
{
    private static CurCherry _instance;
    public static CurCherry Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CurCherry();
            return _instance;
        }
        private set { }
    }

    private CurCherry() { }

    public List<Vector3> PosList;
}

[Serializable]
public class Gem
{
    public List<Vector3> PosList;
}

[Serializable]
public class Cherry
{
    public List<Vector3> PosList;
}