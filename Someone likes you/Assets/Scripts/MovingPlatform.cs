using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Ground 태그 오브젝트와 충돌 시 방향 변환, PlatformPath 태그 오브젝트의 Trigger에 진입 시 방향 전환
    public float speed = 2f;
    public float idleTime = 2.4f;

    public Vector3 dir = Vector3.right; // dir를 Up-Down 방식 또는 Left-Right로 바꾸면 어디에든 적용할 수 있다.
    public Rigidbody2D rigid;
    public State state;
    
    public enum State
    {
        Deactivated, Idle, Activated
    }

    private void Awake()
    {
        state = State.Activated;
        rigid = GetComponent<Rigidbody2D>();
        if(!rigid)
        {
            rigid = gameObject.AddComponent<Rigidbody2D>();
        }
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case State.Activated:
                Move();
                break;

            case State.Idle:
                break;

            case State.Deactivated:
                break;
        }
    }

    private void Move()
    {
        transform.position += dir * speed * Time.fixedDeltaTime; // fixed? original? 뭘 써야 할까?
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlatformPath" || collision.tag == "Ground")
        {
            ChangeDirection();
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // Attach
            collision.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player" && collision.transform.parent == transform)
        {
            collision.transform.parent = null;
            if (state == State.Activated)
               collision.GetComponent<Rigidbody2D>().velocity += (Vector2)dir * speed;
        }
    }

    private void ChangeDirection()
    {
        dir *= -1;
        StartCoroutine("WaitIdleTime");
    }

    private IEnumerator WaitIdleTime()
    {
        if (state != State.Activated) yield break;
        state = State.Idle;
        // Wait Idle Time();
        yield return new WaitForSeconds(idleTime);
        if(state == State.Idle)
        {
            state = State.Activated;
        }
    }
}
