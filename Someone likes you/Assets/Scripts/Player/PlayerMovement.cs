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
    public bool _isWall = false;
    public bool _isCeiling = false;
    public bool _isClimbing = false;

    protected float _deceleration = 0.5f;

    public virtual void Init(Rigidbody2D rigid, PlayerState state)
    {
        this._state = state;
        base.Init(rigid);
    }

    /**
     *  @brief
     *  플레이어 땅 체크 함수
     *  @param groundCheckPos 땅을 체크해주는 빈객체의 위치
     *  @param groundedRadius 해당 위치를 중심으로 반경 체크
     *  @param groundLayers 땅으로 분류할 레이어 마스크
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
                {
                    _state.NotifyState(PlayerState.OnGround.LANDING, PlayerState.OffGround.NONE);
                    return true;
                }
            }
        }

        if(!_isGround)
            _state.NotifyState(PlayerState.OnGround.NONE, PlayerState.OffGround.FALLING);

        return false;
    }
    public bool CeilingCheck(Vector2 ceilingCheckPos, float ceilingRadius, LayerMask ceilingLayers)
    {
        return false;
    }
    /**
     *  @brief 벽 타기를 위한 벽 체크 함수
     *  @param 벽 체크 객체 위치
     *  @param 벽 체크 범위
     *  @param 벽으로 분류할 레이어 마스크
     *  @return
     */
    public bool WallCheck(Vector2 wallCheckPos, float dir, float wallDistance,LayerMask wallLayers)
    {
        bool wasWalled = _isWall;
        _isWall = false;

        if(Mathf.Abs(dir) > 0)
        {
            Vector2 dest = new Vector2(wallCheckPos.x + (dir > 0 ? wallDistance : -wallDistance), wallCheckPos.y);

            DebugRaycast(wallCheckPos, dest, Color.blue);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(dest, wallDistance, wallLayers);
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].gameObject != gameObject)
                {
                    _isWall = true;
                }
            }
        }

        return _isWall;
    }

    /**
     *  @brief
     *  이동에 플레이어가 어느 방향을 향하는 지 판단하는 코드 추가
     *  dir.x 가 0이 아니라면 _prevDir 업데이트
     */
    public override Vector3 Move(Vector3 dir)
    {
        if(dir.x < 0.01)
            _state.Move(0);
        if(dir.x != 0)
            _prevDir = dir.x > 0  ? 1 : -1;
        
        // 애니메이션에게 속도 업데이트
        _state.Move(Mathf.Abs(_rigid.velocity.x));
        
        return base.Move(dir);
    }
    /**
     *  @brief
     *  플레이어가 땅위에 있을 때만 점프할 수 있도록 조치 
     */
    public override void Jump(float amount, GameObject obj = null)
    {
        if(_isGround)
        {
            _state.NotifyState(PlayerState.OnGround.NONE, PlayerState.OffGround.JUMPING);
            base.Jump(amount, obj);
        }
    }
    public virtual void Down(float amount)
    {
        _rigid.velocity = new Vector2(_rigid.velocity.x, _rigid.velocity.y - amount);
        _state.NotifyState(PlayerState.OnGround.NONE, PlayerState.OffGround.FALLING, PlayerState.OnClimbing.NONE, PlayerState.PlayerAction.NONE);
    }

    public virtual void WallSliding(float amount)
    {
        if(_rigid.velocity.y < 0)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, -amount);
            _state.NotifyState(PlayerState.OnGround.NONE, PlayerState.OffGround.NONE, PlayerState.OnClimbing.NONE, PlayerState.PlayerAction.SLIDING);
        }
    }

    private void DebugCircle(Vector3 pos, float radius, Color color)
    {
        Debug.DrawLine(pos, new Vector3(pos.x + radius, pos.y, 0), color);
        Debug.DrawLine(pos, new Vector3(pos.x - radius, pos.y, 0), color);
        Debug.DrawLine(pos, new Vector3(pos.x, pos.y + radius, 0), color);
        Debug.DrawLine(pos, new Vector3(pos.x, pos.y - radius, 0), color);
    }

    private void DebugRaycast(Vector3 origin, Vector3 dest, Color color)
    {
        Debug.DrawLine(origin, dest, color, 3f);
    }
}
