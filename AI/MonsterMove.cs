using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    GameObject player;

    Transform monsterTrans;
    Rigidbody2D monsterRigid;
    public float moveSpeed = 5f;

    void Start()
    {
        monsterTrans = GetComponent<Transform>();
        monsterRigid = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player)
        {
            if(Vector2.Distance(player.transform.position,monsterTrans.position)<20)
            {
                float num = monsterTrans.position.x - player.transform.position.x;
                bool positive = num > 0 ? true : false;
                if(positive)
                    monsterRigid.velocity = new Vector2(-moveSpeed,0);
                else
                    monsterRigid.velocity = new Vector2(moveSpeed, 0);
            }
        }

    }
}
