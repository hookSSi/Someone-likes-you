using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// @TODO JUMPING, FALLING 처리
public class PlayerMovement : Movement
{
    public float _prevDir = 0;
    public bool _isGround = true;

    /// 플레이어 땅 체크
    public bool GroundCheck(Vector2 groundCheckPos, float groundedRadius, LayerMask groundLayers)
    {
        bool wasGrounded = _isGround;
        _isGround = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, groundedRadius, groundLayers);
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject != gameObject)
            {
                _isGround = true;
                if(!wasGrounded)
                    return true;
            }
        }
        return false;
    }

    public override Vector3 Move(Transform obj, Vector3 dest, float speed, bool mode = false, bool isDebug = false)
    {
        this._prevDir = dest.x >= 0 ? 1 : -1;
        return base.Move(obj, dest, speed, mode, isDebug);
    }

    public override void Jump(Vector2 dir, float amout, GameObject obj = null)
    {
        if(_isGround)
        {
            base.Jump(dir, amout, obj);
        }
    }
}
