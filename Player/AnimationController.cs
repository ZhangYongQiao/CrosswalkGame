using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    private Animator anim;
    //private readonly Transform trans;

    //父类组件
    private PlayerController parentCtrl;

    private float _moveX;
    //private bool _isGrounded;
    //[SerializeField]
    private float _speedY;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        parentCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {   
        _moveX = parentCtrl.moveX;
        _speedY = parentCtrl.speedY;
        //_isGrounded = parentCtrl.isGrounded;

        anim.SetBool("isGrounded", parentCtrl.isGrounded);

        if (_moveX != 0 && parentCtrl.isGrounded)
        {
            anim.SetBool("Run", true);
            anim.SetBool("jumpDown", false);
            anim.SetBool("jumpUp", false);
        }
        if (_moveX == 0)
        {
            anim.SetBool("jumpDown", false);
            anim.SetBool("jumpUp", false);
            anim.SetBool("Run", false);
        }
            

        if(_speedY > 0.05)
        {
            anim.SetBool("jumpDown", false);
            anim.SetBool("jumpUp", true);
        }
        if(_speedY < -0.05)
        {
            anim.SetBool("jumpUp", false);
            anim.SetBool("jumpDown", true);
        }
    }

}
