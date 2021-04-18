using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class UIPlayAnimaUtility
{
    public static void ShakeScreen(Camera camera)
    {
        camera.DOShakePosition(0.5f,1f,3,0);
    }
}
