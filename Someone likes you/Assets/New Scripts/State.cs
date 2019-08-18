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
        WALKING      = 1 << 0,
        IDLE         = 1 << 1,
        ATTACK       = 1 << 2,
        INTERACTING1 = 1 << 3
    }
    
    public enum OffGround
    {
       JUMPING  = 1 << 0,
       ATTACK   = 1 << 1,
       FALLING  = 1 << 2
    }
    /// @todo 받는 데이터 형식, 버그는 없는지? (사실 버그 있을거 같음) 체크할 것
    public virtual void Update() 
    {
        HandleAnim();
    }
    /**
     * @brief
     * 객체에게 데이터를 받는 함수
     * 단, 데이터는 애니메이션을 위한 값
     */
    public virtual void NotifyData(float dir)
    {
        if(Mathf.Abs(dir) > 0.01)
        {
            _onGroundState = OnGround.WALKING;
        }
        else
        {
            _onGroundState = OnGround.IDLE;
        }
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
            if(_onGroundState != 0)
            {
                OnGroundAnim();
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
        }
    }
    /// 땅위아님에서 애니메이션 처리
    public virtual void OffGroundAnim(){}
}
