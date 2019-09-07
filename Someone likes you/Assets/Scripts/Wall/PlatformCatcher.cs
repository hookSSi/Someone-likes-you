using UnityEngine;
using System.Collections.Generic;

/**
 *  @brief 움직이는 플랫폼이 위에 있는 물체와 함께 움직이게 하는 클래스
 *  @author 한민서
 *  @date 2019.09.01
 */
class PlatformCatcher : MonoBehaviour
{
    [Header("플랫폼을 무시할 레이어")]
    [SerializeField] private LayerMask[] _Filter;
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D temp;
        Collider2D coll = collision.collider;
        // 실제로 플레이어는 바닥에 약간 파묻혀 있으므로 이를 보정하기 위한 변수 delta를 둔다
        float delta = 0.05f;
        float collY = coll.bounds.min.y;

        // 플랫폼 위에 물체가 있지 않다면 return한다
        if (collY < _collider.bounds.max.y - delta)
            return;
        
        // Debug.Log("들어온다.");
        // 필터에 해당하는 오브젝트를 거른다.
        if (Filter(coll))
            return;

        // 플레이어의 경우 자식 Collider가 걸려서 부모를 강제 변경(...) 당할 수 있으므로 부모 오브젝트에 Collider가 존재하는지 탐색한다.
        while((temp = coll.transform.parent.GetComponent<Collider2D>()) != null)
        {
            coll = temp;
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

    /// _Filter에 해당하는 콜라이더를 무시한다
    private bool Filter(Collider2D coll)
    {
        foreach(var layer in _Filter)
        {
            if (coll.gameObject.layer == layer)
            {
                return true;
            }
        }
        return false;
    }
}