﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief
 *  공통적인 움직임을 정의할 함수를 담는 클래스
 *  @author 문성후
 *  @date   2019.08.16
 */

public class Movement : MonoBehaviour
{
    protected Rigidbody2D _rigid;
    protected Vector3 _velocity = Vector3.zero;
    public float _movementSmoothing = .01f;
    /// Movement 초기화 함수
    public virtual void Init(Rigidbody2D rigid)
    {
        this._rigid = rigid;
    }
    /**
     *  @brief
     *  Transform 이동 함수
     *  @param obj 이동할 게임 오브젝트
     *  @param dest 목표 위치
     *  @param speed 속도
     *  @param mode  이동 모드 (true = Velocity Move, false = Position Move)
     *  @param isDebug  디버그 사용여부 (true = 디버그 사용, false = 디버그 사용안함)
     */
    public virtual Vector3 Move(Transform obj, Vector3 dest, float speed, bool mode = false, bool isDebug = false)
    {
        Vector3 origin = obj.position;

        if(mode)
        {
            obj.position += dest.normalized * speed * Time.deltaTime;   
        }
        else
            obj.position = dest;

        if(isDebug)
        {
            if(mode)
                Debug.Log(string.Format("{0}에서 {1}으로 {2}의 속도로 이동", origin, dest.normalized, speed));
            else
                Debug.Log(string.Format("{0}에서 {1}로 이동", origin, dest));
        }

        return origin;
    }
    /**
     *  @brief
     *  RigidBody 이동 함수
     *  @param dir 방향
     *  @return origin 원래 위치
     */
    public virtual Vector3 Move(Vector3 dir)
    {
        Vector3 origin = _rigid.position;

        Vector2 targetVelocity = new Vector2(dir.x, _rigid.velocity.y + dir.y);
        _rigid.velocity = Vector3.SmoothDamp(_rigid.velocity, targetVelocity, ref _velocity, _movementSmoothing);

        return origin;
    }
    /**
     *  @brief
     *  RigidBody 힘을 주는 함수
     *  @param dir 힘의 방향
     *  @param amount 힘의 크기
     *  @param mode 힘을 주는 방식
     *  @return origin 원래 위치
     */
    public virtual Vector3 AddForce(Vector3 dir, float amount, ForceMode2D mode)
    {
        Vector3 origin = _rigid.position;

        _rigid.AddForce(dir.normalized * amount, mode);

        return origin;
    }
   /**
     *  @brief
     *  점프하는 함수
     *  @param amount 힘의 크기
     *  @param obj 점프하는 게임 오브젝트
     *  @todo 장점프를 구현해야함 (getkey입력시)
     */
    public virtual void Jump(float amount, GameObject obj = null)
    {
        if(_rigid)
        {
            _rigid.AddForce(Vector2.up * amount);
        }
        else
        {
            obj.transform.position += Vector3.up * amount;
        }
    }
}