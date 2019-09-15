using UnityEngine;
using System.Collections.Generic;

/**
 *  @brief 움직이는 플랫폼이 위에 있는 물체와 함께 움직이게 하는 클래스
 *  @author 한민서
 *  @date 2019.09.01
 */
public class PlatformCatcher : MonoBehaviour
{
    [Header("플레이어 레이어")]
    [SerializeField] private LayerMask _playerLayer = 1 << 9;
    [Header("오브젝트 레이어")]
    [SerializeField] private LayerMask _objLayer = 1 << 0;
    [Header("플랫폼을 무시할 레이어")]
    [SerializeField] private LayerMask[] _Filter;
    [SerializeField] public Dictionary<Transform, Transform> _parentDict;
    private Collider2D _collider;
    private Rigidbody2D _rigid;
    private PlatformEffector2D _platformEffector;

    /// 기본 설정 초기화
    void Awake()
    {
        _parentDict = new Dictionary<Transform, Transform>();

        // 컴포넌트 초기화
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
        Collider2D coll = collision.collider;
        Transform target;
        // 실제로 플레이어는 바닥에 약간 파묻혀 있으므로 이를 보정하기 위한 변수 delta를 둔다
        float delta = 0.05f;
        float collY = coll.bounds.min.y;

        // 플랫폼 위에 물체가 있지 않다면 거른다
        if (collY < _collider.bounds.max.y - delta)
            return;
        
        // 필터에 해당하는 오브젝트를 거른다.
        if (Filter(coll))
            return;

        // 자식 오브젝트 혼자 납치하지 않게 하는 함수
        target = GetRoot(coll.transform);
        // 부모 오브젝트가 존재하면 이를 리스트에 올리고, 플랫폼을 부모로 설정한다
        if (target.parent != null)
        {
            _parentDict.Add(target, target.parent);
        }
        target.SetParent(this.transform);
    }

    /// 플랫폼 위에 있는 물체의 부모를 해제한다
    void OnCollisionExit2D(Collision2D collision)
    {
        Collider2D coll = collision.collider;
        Transform target = GetRoot(coll.transform);

        if (Filter(coll))
        {
            return;
        }

        // 부모 오브젝트를 원래대로 되돌린다.
        target.SetParent(null);
        if (_parentDict.ContainsKey(target))
        {
            target.SetParent(_parentDict[target]);
            _parentDict.Remove(target);
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

    /**
    *   @brief 오브젝트의 루트를 찾는다.
    *   @detail Player이거나 Object에 해당한다면 OK! Platform 오브젝트나 그 위까지는 찾지 않는다.
    */
    private Transform GetRoot(Transform obj)
    {
        Transform root = obj;
        while(root.parent)
        {
            if (root.parent == this.transform || IsPlayer(obj) || IsObject(obj))
            {
                break;
            }
            root = root.parent;
        }
        return root;
    }

    /// 해당 오브젝트가 플레이어인지 검사한다.
    private bool IsPlayer(Transform target)
    {
        if (_playerLayer != 0 &&
        _playerLayer != LayerMask.NameToLayer("Default") &&
        target.gameObject.layer == _playerLayer)
        {
            Debug.Log("Player 찾았다!");
            return true;
        }
        return false;
    }

    /// 해당 오브젝트가 움직일 수 있는 오브젝트인지 검사한다.
    private bool IsObject(Transform target)
    {
        if (_objLayer != 0 &&
        _objLayer != LayerMask.NameToLayer("Default") &&
        target.gameObject.layer == _objLayer)
        {
            return true;
        }
        return false;
    }
}