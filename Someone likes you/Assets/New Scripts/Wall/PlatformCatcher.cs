using UnityEngine;
using System.Collections.Generic;

/**
 *  @brief 움직이는 플랫폼이 위에 있는 물체와 함께 움직이게 하는 클래스
 *  @author 한민서
 *  @date 2019.09.01
 */
class PlatformCatcher : MonoBehaviour
{
    [SerializeField] private LayerMask[] _ArrayNotCatch;
    private Collider2D _collider;
    private Rigidbody2D _rigid;
    private PlatformEffector2D _platformEffector;

    /// 컴포넌트 초기화
    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        if (!_collider)
        {
            _collider = (Collider2D)gameObject.AddComponent<BoxCollider2D>();
        }
        _collider.usedByEffector = true;

        _rigid = GetComponent<Rigidbody2D>();
        if (!_rigid)
        {
            _rigid = gameObject.AddComponent<Rigidbody2D>();
        }
        _rigid.isKinematic = true;

        _platformEffector = GetComponent<PlatformEffector2D>();
        if (!_platformEffector)
        {
            _platformEffector = gameObject.AddComponent<PlatformEffector2D>();
        }
        _platformEffector.useColliderMask = true;
        _platformEffector.useOneWay = true;
    }

    /// 플랫폼 위에 있는 물체의 부모를 플랫폼으로 설정한다
    void OnCollisionEnter2D(Collision2D coll)
    {
        // 플랫폼 위에 물체가 있지 않다면 return한다
        float collY = coll.collider.bounds.min.y;
        if (collY < _collider.bounds.max.y - 0.05f)
            return;
        
        // Debug.Log("들어온다.");
        foreach(var layer in _ArrayNotCatch)
        {
            if (coll.gameObject.layer == layer)
            {
                return;
            }
        }

        coll.transform.SetParent(this.transform);
    }

    /// 플랫폼 위에 있는 물체의 부모를 해제한다
    void OnCollisionExit2D(Collision2D coll)
    {
        // Debug.Log("나간다.");
        if (coll.transform.parent == this.transform)
        {
            coll.transform.SetParent(null);
        }
    }
}