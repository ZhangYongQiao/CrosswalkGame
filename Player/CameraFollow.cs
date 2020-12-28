using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{   
    Transform cameraPos;
    GameObject cMCamera;
    CinemachineVirtualCamera virtualCamera;
    GameObject player;

    private void Awake()
    {
        cameraPos = GetComponent<Transform>();
        cMCamera = GameObject.Find("CM vcam1");
        virtualCamera = cMCamera.GetComponent<CinemachineVirtualCamera>();
    }

    void LateUpdate()
    {
        player =  GameObject.FindGameObjectWithTag("Player");

        if (player)
        {   
            if(virtualCamera)
            {
                virtualCamera.Follow = player.transform;
            }
            
        }

    }
}
