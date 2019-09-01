using System.Collections;
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
    public OnGround _onGroundState;
    /// 땅안위의 상태
    [EnumFlags]
    public OffGround _offGroundState;
    public enum OnGround
    {
        NONE         = 0,
        WALKING      = 1 << 0,
        IDLE         = 1 << 1,
        ATTACK       = 1 << 2,
        INTERACTING1 = 1 << 3,
        LANDING      = 1 << 4
    }
    public enum OffGround
    {
       NONE     = 0,
       JUMPING  = 1 << 0,
       ATTACK   = 1 << 1,
       FALLING  = 1 << 2
    }
    public virtual void Update() 
    {
        
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
        HandleAnim();
    }
    /**
     * @brief
     * 애니메이션을 처리하는 함수'
     * 땅위, 땅아님에 따라 처리
     */
    public virtual void HandleAnim()
    {
        if(_animator)
        {
            if(_onGroundState != OnGround.NONE)
            {
                OnGroundAnim();
            }
            if(_offGroundState != OffGround.NONE)
            {
                OffGroundAnim();
            }
        }
        else
            Debug.Log("애니메이션이 없어요!");
    }
    /// 땅위에서 애니메이션 처리
    public virtual void OnGroundAnim()
    {
        switch(_onGroundState)
        {
            case OnGround.WALKING:
                _animator.SetFloat("x_speed", 3);
                break;
            case OnGround.IDLE:
                _animator.SetFloat("x_speed", 0);
                break;
            case OnGround.LANDING:
                _animator.SetBool("isGround", true);
                break;
        }
    }
    /// 땅위아님에서 애니메이션 처리
    public virtual void OffGroundAnim()
    {
        switch(_offGroundState)
        {
            case OffGround.NONE:
                _animator.SetBool("isGround", true);
                break;
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
