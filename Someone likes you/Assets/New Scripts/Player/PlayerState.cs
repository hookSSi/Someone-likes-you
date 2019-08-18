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
    [EnumFlags]
    public P_State _playerState;
    public enum P_State
    {
        CLIMBING = 1 << 0,
        THROWING = 1 << 1
    }

    public override void HandleAnim()
    {
        base.HandleAnim();
    }
}
