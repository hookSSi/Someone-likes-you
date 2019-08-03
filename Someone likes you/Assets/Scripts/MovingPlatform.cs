using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float idleTime = 2.4f;
    public bool isActivated;
    public bool directionByCollision = false; // 충돌에 따라 방향이 바뀌는가?(true -> 목적지 존재, false -> 충돌할 때까지 전진)

    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 dir = Vector3.right; // dir를 Up-Down 방식 또는 Left-Right로 바꾸면 어디에든 적용할 수 있다.
    public Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        if(!rigid)
        {
            rigid = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    private void FixedUpdate()
    {
        if(isActivated)
        {
            Move();
        }
    }

    private void Move()
    {
        rigid.velocity = dir * speed * Time.fixedDeltaTime; // fixed? original? 뭘 써야 할까?
    }

    void OnCollisionEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player" && directionByCollision)
        {

        }
    }
}
