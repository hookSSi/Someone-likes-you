﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief
 *  애니메이션이 달린 객체의 상태 표시겸 애니메이션 처리 클래스
 *  @author 문성후
 *  @date   2019.08.18
 *  @see https://wergia.tistory.com/104
 */

public class State : MonoBehaviour
{
    /// 처리할 유니티의 Animator
    public Animator _animator{get; set;}
    /// 땅위의 상태
    [EnumFlags]
    [SerializeField]protected OnGround _onGroundState;
    /// 땅안위의 상태
    [EnumFlags]
    [SerializeField]protected OffGround _offGroundState;
    public enum OnGround
    {
        NONE         = 0,
        WALKING      = 1 << 0,
        IDLE         = 1 << 1,
        LANDING      = 1 << 2
    }
    public enum OffGround
    {
       NONE     = 0,
       JUMPING  = 1 << 0,
       FALLING  = 1 << 1
    }
    /**
     * @brief
     * 객체에게 데이터를 받는 함수
     * 단, 데이터는 애니메이션을 위한 값
     */
    public virtual void NotifyState(OnGround onGroundState, OffGround offGroundState)
    {
        _onGroundState  = onGroundState;
        _offGroundState = offGroundState;
        this.HandleAnim();
    }
    /**
     * @brief
     * 애니메이션을 처리하는 함수'
     * 땅위, 땅아님에 따라 처리
     */
    protected virtual void HandleAnim()
    {
        if(_animator)
        {
            OnGroundAnim();
            OffGroundAnim();
        }
        else
            Debug.Log("애니메이션이 없어요!");
    }
    /// 땅위에서 애니메이션 처리
    protected virtual void OnGroundAnim()
    {
        switch(_onGroundState)
        {
            case OnGround.LANDING:
                _animator.SetBool("isGround", true);
                break;
        }
    }
    /// 땅위아님에서 애니메이션 처리
    protected virtual void OffGroundAnim()
    {
        switch(_offGroundState)
        {
            case OffGround.JUMPING:
                _animator.SetTrigger("isJump");
                break;
            case OffGround.FALLING:
                _animator.SetBool("isGround", false);
                break;
        }
    }

    public void Move(float x_speed)
    {
        _animator.SetFloat("x_speed", x_speed);
    }
}
