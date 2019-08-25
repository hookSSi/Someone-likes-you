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
    /// 플레이어만을 위한 애니메이션 상태 변수
    [EnumFlags]
    public P_State _playerState;
    public enum P_State
    {
        CLIMBING = 1 << 0,
        THROWING = 1 << 1,
        LANDING  = 1 << 2
    }

    public override void NotifyState(OnGround onGroundState, OffGround offGroundState)
    {
        base.NotifyState(onGroundState, offGroundState);
    }

    public override void HandleAnim()
    {
        base.HandleAnim();
    }
}
