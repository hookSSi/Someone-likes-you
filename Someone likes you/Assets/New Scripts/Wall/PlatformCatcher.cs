using UnityEngine;
using System.Collections.Generic;

class PlatformCatcher : MonoBehaviour
{
    [SerializeField] private LayerMask[] _ArrayNotCatch;
    private Collider2D _collider;
    private Rigidbody2D _rigid;
    private PlatformEffector2D _platformEffector;

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

    // GroundCheck를 통해서 Object를 체크할 수도 있지 않을까....라고 생각했지만 그래버리면
    // 다른 오브젝트는 제대로 체크가 안 되겠지...
    void OnCollisionEnter2D(Collision2D coll)
    {
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
        
        SpriteRenderer sprite = coll.gameObject.GetComponentInChildren<SpriteRenderer>();
        Vector3 spriteScale = sprite.transform.localScale;

        coll.transform.SetParent(this.transform);

        // 요주의 코드. 스프라이트 크기가 스파게티가 된다면 아마 이 녀석 때문일겁니다.
        sprite.transform.localScale = spriteScale;
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        // Debug.Log("나간다.");
        if (coll.transform.parent == this.transform)
        {
            SpriteRenderer sprite = coll.gameObject.GetComponentInChildren<SpriteRenderer>();
            Vector3 spriteScale = sprite.transform.localScale;
        
            coll.transform.SetParent(null);
            sprite.transform.localScale = spriteScale;
        }
    }
}