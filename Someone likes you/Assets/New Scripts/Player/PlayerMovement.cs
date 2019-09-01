using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief
 *  플레이어 움직임 담당 클래스
 *  @author 문성후
 *  @date   2019.08.27
 */
public class PlayerMovement : Movement
{
    protected PlayerState _state;
    public float _prevDir = 0;
    public bool _isGround = true;

    public virtual void Init(Rigidbody2D rigid, PlayerState state)
    {
        this._state = state;
        base.Init(rigid);
    }

    /**
     *  @brief
     *  플레이어 땅 체크 함수
     *  @param groundCheckPos 땅을 체크해주는 빈객체의 위치 Vector2
     *  @param groundRadius 해당 위치를 중심으로 반경 float
     *  @param groundLayers 땅으로 분류할 레이어 마스크 LayerMask
     *  @return LANDING 했는지 판단하는 bool 값 리턴
     */
    public bool GroundCheck(Vector2 groundCheckPos, float groundedRadius, LayerMask groundLayers)
    {
        bool wasGrounded = _isGround;
        _isGround = false;

        DebugCircle(groundCheckPos, groundedRadius, Color.red);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, groundedRadius, groundLayers);
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject != gameObject)
            {
                _isGround = true;
                 _state.NotifyState(PlayerState.OnGround.IDLE, PlayerState.OffGround.NONE);
                if(!wasGrounded)
                    return true;
            }
        }

        _state.NotifyState(PlayerState.OnGround.NONE, PlayerState.OffGround.FALLING);
        return false;
    }
    /**
     *  @brief
     *  이동에 플레이어가 어느 방향을 향하는 지 판단하는 코드 추가
     */
    public override Vector3 Move(Vector3 dir)
    {
        if(dir.x < 0.01)
            _state.Move(0);
        if(dir.x != 0)
            _prevDir = dir.x > 0  ? 1 : -1;
        _state.Move(Mathf.Abs(dir.x));
        
        return base.Move(dir);
    }
    /**
     *  @brief
     *  플레이어가 땅위에 있을 때만 점프할 수 있도록 조치 
     */
    public override void Jump(Vector2 dir, float amout, GameObject obj = null)
    {
        if(_isGround)
        {
            base.Jump(dir, amout, obj);
        }
    }

    private void DebugCircle(Vector3 pos, float radius, Color color)
    {
        Debug.DrawLine(pos, new Vector3(pos.x + radius, pos.y, 0), color, 0.1f);
        Debug.DrawLine(pos, new Vector3(pos.x - radius, pos.y, 0), color, 0.1f);
        Debug.DrawLine(pos, new Vector3(pos.x, pos.y + radius, 0), color, 0.1f);
        Debug.DrawLine(pos, new Vector3(pos.x, pos.y - radius, 0), color, 0.1f);
    }
}
