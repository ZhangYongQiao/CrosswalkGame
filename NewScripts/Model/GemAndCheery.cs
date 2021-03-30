using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CurGemAndCheery
{
    private static CurGemAndCheery _instance;
    public static CurGemAndCheery Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CurGemAndCheery();
            return _instance;
        }
        private set { }
    }

    private CurGemAndCheery() { }

    public List<Transform> PosList;
}

[Serializable]
public class GemAndCheery
{
    public List<Transform> PosList;
}