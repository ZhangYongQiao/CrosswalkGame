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

    private LoadPlayerAndPanel _loadPlayerAndPanel;     //获取LoadPlayerAndPanel脚本中的成员

    public AnimationClip _clip;                 //受伤动画片段
    private float _clipLenght;                  

    private void Awake()
    {
        _clipLenght = _clip.length;             //获得受伤动画时长

        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        coll  = GetComponent<Collider2D>();

        _childAnim = trans.Find("playerModule").GetComponent<Animator>();
        _loadPlayerAndPanel = GameObject.Find("Canvas").transform.GetComponent<LoadPlayerAndPanel>();
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

        //防止如果角色和怪物一直接触出现一直播放动画的状态
        if(_childAnim.GetBool("Hurt"))
        {   
            if(_clipLenght<0)
            {
                _childAnim.SetBool("Hurt", false);
                _clipLenght = _clip.length;
            }
            else
                _clipLenght -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        //触发界面跳转
        if (collision.CompareTag("ToNextScene"))
        {
            float soundValueTmp = GameObject.Find("Audio Source").GetComponent<AudioSource>().volume;
            PlayerPrefs.SetFloat("soundValue", soundValueTmp);

            int numTmp = int.Parse(PlayerDataRunTime.Instance._curScene);
            MessageData data = new MessageData(numTmp+1);
            MessageCenter.Instance.Send(MessageName.SCENE_JUMP, data);
        }

        //受伤 触发动画、减血、无敌2s
        if (collision.CompareTag("Monster") && _canHurt)
        {   
            if(_childAnim)
            {
                _childAnim.SetBool("Hurt",true);
                _canHurt = !_canHurt;

                //减血
                if(PlayerDataRunTime.Instance._curBlood > 0)
                {
                    GameObject go = _loadPlayerAndPanel._bloods.Pop();
                    PlayerDataRunTime.Instance._curBlood -= 1;
                    if (PlayerDataRunTime.Instance._curBlood == 0)
                    {
                        Destroy(go);
                        //TODO
                        //弹出菜单选项
                        GameObject.Find("Canvas").transform.Find("DeathLoadPanel(Clone)").gameObject.SetActive(true);

                        Time.timeScale = 0;

                        Debug.Log("死了");
                        return;
                    }
                    //清除血量图标
                    Destroy(go);
                }
            }
        }

        //吃樱桃
        if(collision.CompareTag("IncreaseBlood"))
        {   
            Animator animTmp = collision.gameObject.GetComponent<Animator>();
            if(animTmp)
                animTmp.SetTrigger("Eat");
            animTmp.transform.GetComponent<BoxCollider2D>().enabled = false;                    //防止多次接触
            Destroy(collision.gameObject, 0.3f);

            //满血加分、缺血加血
            if (PlayerDataRunTime.Instance._curBlood == 5)
            {   
                //发送消息
                PlayerDataRunTime.Instance._curScore +=2;
                MessageData data = new MessageData(PlayerDataRunTime.Instance._curScore);
                MessageCenter.Instance.Send(MessageName.SCORE_CHANGE, data);
            }
            else
            {
                PlayerDataRunTime.Instance._curBlood += 1;
                MessageData data = new MessageData(PlayerDataRunTime.Instance._curBlood);
                MessageCenter.Instance.Send(MessageName.ADD_BLOOD, data);
            }
        }

        //得宝石
        if(collision.CompareTag("Gem"))
        {
            Animator animTmp = collision.gameObject.GetComponent<Animator>();
            if (animTmp)
                animTmp.SetTrigger("Get");
            animTmp.transform.GetComponent<BoxCollider2D>().enabled = false;                        //防止多次接触
            Destroy(collision.gameObject, 0.3f);

            //发送消息
            PlayerDataRunTime.Instance._curScore += 1;
            MessageData data = new MessageData(PlayerDataRunTime.Instance._curScore);
            MessageCenter.Instance.Send(MessageName.SCORE_CHANGE, data);
        }

        //掉落至下界 直接死亡
        if (collision.CompareTag("LowerBound"))
        {
            GameObject.Find("Canvas").transform.Find("DeathLoadPanel(Clone)").gameObject.SetActive(true);

            Time.timeScale = 0;

            Debug.Log("死了");
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Monster"))
    //    {
    //        if (_childAnim)
    //        {
    //            _childAnim.SetBool("Hurt", false);
    //        }
    //    }
    //}
}
