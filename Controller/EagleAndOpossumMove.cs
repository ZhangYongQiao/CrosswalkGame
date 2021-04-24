using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EagleAndOpossumMove : MonoBehaviour
{
    private Transform _monsterTrans;                                        //初始化组件
    private SpriteRenderer _monsterSpr;

    private bool _isRight;
    private float _recordTime = 0f;                                         //计时

    public float _moveSpeed;                                                //移动速度
    private float _oneDirectionMoveTime;                                    //单方向移动时间(秒)


    private readonly float _distance = 5 * 0.02f * 50 * 2;                  //固定单程距离（标准5m/s,2s）

    private void Awake()
    {
        _monsterTrans = GetComponent<Transform>();
        _monsterSpr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //可以通过改变速度调整难度，但行走路程不变
        _oneDirectionMoveTime = _distance / (_moveSpeed * 0.02f * 50);

        _recordTime += Time.deltaTime;

        if (_recordTime >= _oneDirectionMoveTime && _isRight)
        {
            _isRight = false;
            _recordTime = 0f;
        }
        else if (_recordTime >= _oneDirectionMoveTime && !_isRight)
        {
            _isRight = true;
            _recordTime = 0f;
        }

        if (_isRight)
        {
            _monsterTrans.Translate(_moveSpeed * Time.deltaTime, 0, 0);
            _monsterSpr.flipX = true;
        }
        else
        {
            _monsterTrans.Translate(-_moveSpeed * Time.deltaTime, 0, 0);
            _monsterSpr.flipX = false;
        }
    }
    
}
