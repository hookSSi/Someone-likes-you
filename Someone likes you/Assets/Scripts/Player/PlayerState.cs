using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief
 *  플레이어 상태, 애니메이션 처리 클래스
 *  State 클래스를 상속받음!
 *  @author 문성후
 *  @date   2019.08.18
 */
public class PlayerState : State
{
    /// 플레이어 액션관련 애니메이션 상태 변수
    [EnumFlags]
    [SerializeField]protected PlayerAction _playerAction;
    /// 플레이어 벽타기 애니메이션 상태 변수
    [EnumFlags]
    [SerializeField]protected OnClimbing _onClimbing;

    public enum PlayerAction
    {
        NONE         = 0,
        ATTACK       = 1 << 1,
        THROWING     = 1 << 2,
        INTERACTING = 1 << 3,
        SLIDING      = 1 << 4
    }
    public enum OnClimbing
    {
        NONE     = 0,
        IDLE     = 1 << 0,
        CLIMBING = 1 << 1
    }

    public override void NotifyState(OnGround onGroundState, OffGround offGroundState)
    {
        base.NotifyState(onGroundState, offGroundState);
        _onClimbing = PlayerState.OnClimbing.NONE;
        _playerAction = PlayerState.PlayerAction.NONE;
    }
    /**
     *  @param onGroundState 땅위에서 애니메이션 상태
     *  @param offGroundState 공중에서 애니메이션 상태
     *  @param onClimbing 벽타기 중 애니메이션 상태
     *  @param playerAction 플레이어만이 행동 애니메이션 상태
     */
    public virtual void NotifyState(OnGround onGroundState, OffGround offGroundState, OnClimbing onClimbing, PlayerAction playerAction)
    {
        base.NotifyState(onGroundState, offGroundState);
        _onClimbing = onClimbing;
        _playerAction = playerAction;
    }
    protected override void OnGroundAnim()
    {
        switch(_onGroundState)
        {
            case OnGround.LANDING:
                _animator.SetBool("isGround", true);
                _animator.SetBool("isSliding", false);
                break;
        }
    }

    /// 벽타기 애니메이션 처리
    protected virtual void OnClimbingAnim()
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
    /// 플레이어 액션 애니메이션 처리
    protected virtual void PlayerActionAnim()
    {
          switch(_playerAction)
        {
            case PlayerAction.SLIDING:
                _animator.SetBool("isSliding", true);
                break;
        }
    }
    protected override void HandleAnim()
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
            if(_onClimbing != OnClimbing.NONE)
            {
                OnClimbingAnim();
            }
            if(_playerAction != PlayerAction.NONE)
            {
                PlayerActionAnim();
            }
        }
        else
            Debug.Log("애니메이션이 없어요!");
    }
}
