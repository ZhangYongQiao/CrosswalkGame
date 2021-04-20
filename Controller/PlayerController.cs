using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    public int   moveSpeed;                      //自定义速度和跳跃值
    public float jumpForce;
    [HideInInspector]
    public float speedY;
    [HideInInspector]
    public float moveX;                          //X轴参数
    [HideInInspector]
    public bool isGrounded = false;              //是否接地
    private bool airCanJump = false;             //标志-二段跳

    private readonly float _setInvincibleTime = 2f;
    private float _invincibleTime = 2f;         //无敌时间
    private bool  _canHurt = true;              //是否能受伤

    private Rigidbody2D rigid;                  //初始化组件
    private Transform trans;
    private Collider2D coll;

    private LayerMask ground = 1 << 8;          //获取地面层

    private Animator _childAnim;                //子物体Animator组件

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        coll  = GetComponent<Collider2D>();

        _childAnim = trans.Find("playerModule").GetComponent<Animator>();
    }

    private void Update()
    {
        speedY = rigid.velocity.y;
        moveX = Input.GetAxis("Horizontal");
        isGrounded = coll.IsTouchingLayers(ground);                         //是否接地
        rigid.velocity = new Vector2(moveX * moveSpeed,speedY);

        //跳跃
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            airCanJump = !airCanJump;
            rigid.velocity = new Vector2(rigid.velocity.x,jumpForce);
        }

        //二段跳
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && airCanJump)
        {
            airCanJump = false;
            rigid.velocity = new Vector2(rigid.velocity.x,jumpForce);
            //Camera camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>();
            //UIPlayAnimaUtility.ShakeScreen(camera);
        }

        //转向
        if (moveX < -0.05)
            trans.localScale = new Vector2(-1, 1);
        if (moveX > 0.05)
            trans.localScale = new Vector2(1, 1);

        //受伤后进入2s无敌时间
        if (!_canHurt && _invincibleTime > 0)
            _invincibleTime -= Time.deltaTime;

        if (_invincibleTime <= 0)
        {
            _invincibleTime = _setInvincibleTime;
            _canHurt = !_canHurt;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem"))
        {
            GetGem(collision);
        }
        else if (collision.CompareTag("Cherry"))
        {
            GetCherry(collision);
        }
        else if(collision.CompareTag("Monster"))
        {
            GetMonsterScore(collision);
        }
    }

    Animator anim;                                          //共用动画状态机
    private void GetMonsterScore(Collider2D collision)
    {
        CommonFuncToGetScore(EffectType.GetReward, ScoreType.DieMonster);
        //DelayDesGo(collision, 0.3f);
    }

    private void GetGem(Collider2D collision)
    {
        SetAnimatorState(collision, "Get");
        CommonFuncToGetScore(EffectType.GetReward, ScoreType.Gem);
        DelayDesGo(collision, 0.3f);
    }

    private void GetCherry(Collider2D collision)
    {
        SetAnimatorState(collision, "Eat");
        CommonFuncToGetScore(EffectType.GetReward, ScoreType.Cherry);
        DelayDesGo(collision, 0.3f);
    }

#region 提取相同部分

    private void SetAnimatorState(Collider2D collision,string triggerName)
    {
        anim = collision.GetComponent<Animator>();
        if (anim)
            anim.SetTrigger(triggerName);
    }

    private void CommonFuncToGetScore(EffectType type,ScoreType scoreType)
    {
        MessageData data = new MessageData(type);
        MessageCenter.Instance.Send(MessageName.OnPlaySoundEffect, data);

        MessageData score = new MessageData(scoreType);
        MessageCenter.Instance.Send(MessageName.OnAddScore, score);
    }

    private void DelayDesGo(Collider2D coll,float del = 0.3f)
    {
        coll.enabled = false;
        Destroy(coll.gameObject, del);
    }

#endregion
}

public enum ScoreType
{
    Gem = 1,
    Cherry = 2,
    DieMonster = 4
} 
