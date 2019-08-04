using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigid;
    Collider2D playerCollider;

    public float moveSpeed = 4.7f;
    public float jumpPower = 0.9f;
    public float climbSpeed = 6f;
    public float climbJumpPower = 0.4f;

    public float minLengthHangable = 0.6f; // 가장 밑에서부터 측정, PlayerCollider에서 Climb할 수 있는 하한선(플레이어 Collider에서의 비율로 측정)
    public float maxLengthHangable = 1.2f; // 가장 밑에서부터 측정, PlayerCollider에서 Climb할 수 있는 상한선(플레이어 Collider에서의 비율로 측정

    private bool isJumping = false;
    public bool isGround = false;
    private bool isClimbing = false;
    private bool isJumpCancelable = false;

    public Tool currentTool;

    public Animator _animator;

    public enum State
    {
        Idle,
        Moving,
        Jump,
        Hang,
        Attack,
        Throw,
        UnderAttack
    }

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();

        // PlayerCollider 가져오기(isTrigger == false)
        Collider2D[] tempColList = GetComponents<Collider2D>();
        for (int i = 0; i < tempColList.Length; i++)
        {
            if (tempColList[i].isTrigger == false)
            {
                playerCollider = tempColList[i];
                break;
            }
        }

        if(_animator == null)
        {
            _animator = this.gameObject.GetComponent<Animator>();
        } 
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
        if (!Input.GetButton("Jump"))
        {
            JumpCut();
        }

        // 무기 스왑
        // 작성 중
        if (Input.GetAxisRaw("ScrollWheel") < 0)
        {
            //ItemDatabase.GetInstance().currentTool++;
            //currentTool = ItemDatabase.GetInstance().CurrentTool();
        }

        if (Input.GetAxisRaw("ScrollWheel") > 0)
        {
            Debug.Log("삐빅 위 휠");
        }

        if (!isClimbing)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                Move(Vector3.left /* * Input.GetAxis("Horizontal")*(-1)*/);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                Move(Vector3.right /* * Input.GetAxis("Horizontal")*/);
            }
            Jump();
            _animator.SetFloat("x_speed", Mathf.Abs(rigid.velocity.x));
            _animator.SetFloat("y_speed", rigid.velocity.y);
        }
        _animator.SetBool("isGround", isGround);
    }

    private void Move(Vector3 dir)
    {
        Vector3 vel = dir * moveSpeed;
        rigid.velocity = vel + Vector3.up * rigid.velocity.y;
        if(vel.x != 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * vel.normalized.x, transform.localScale.y, transform.localScale.z); // 스프라이트 좌우 교체
    }

    private void Jump()
    {
        if (!isJumping || !isGround) // 점프 커맨드 입력
            return;

        _animator.SetTrigger("isJumping");
        rigid.velocity = Vector2.zero;

        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

        isJumpCancelable = true;
        isJumping = false;
        isGround = false;
    }

    private void Jump(float _jumpPower) // 강제 점프
    {
        rigid.velocity = Vector2.zero;

        rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

        isJumping = false;
        isGround = false;
    }

    private void JumpCut()
    {
        if (!isJumpCancelable)
            return;

        float velocityY = -0.1f;

        if (rigid.velocity.y > velocityY)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, velocityY);
        }
    }

    IEnumerator Climb(float deltaHeight, Collision2D collision) // 파쿠르 강제 올라가기
    {
        // Debug.Log("올라간다!!");
        Vector3 startPosition = transform.position;
        Vector3 dir;
        float time = 0;
        float _moveSpeed;
        float distance = deltaHeight + playerCollider.bounds.size.y / 2;
        float timeExpected = distance / climbSpeed;

        isClimbing = true;
        _animator.SetBool("isClimbing", isClimbing);
        rigid.simulated = false;
        rigid.velocity = Vector2.zero;

        while (true) // y 좌표 이동
        {
            transform.parent = collision.transform;
            yield return new WaitForFixedUpdate();
            transform.position += Vector3.up * (climbSpeed * Time.fixedDeltaTime);
            time += Time.fixedDeltaTime;

            if (time >= timeExpected) // 어느 정도의 시간이 흐르면
            {
                break;
            }
        }

        time = 0;
        distance = playerCollider.bounds.size.x; // 플레이어 사이즈 만큼 x축으로 이동

        yield return new WaitForFixedUpdate();
        rigid.simulated = true;
        isJumpCancelable = false;
        Jump(climbJumpPower);
        //Debug.Log("현재 속도: " + rigid.velocity.y);
        yield return new WaitForFixedUpdate();

        timeExpected = - rigid.velocity.y * 2 / (rigid.gravityScale * Physics2D.gravity.y);
        _moveSpeed = distance / timeExpected;

        if(collision.transform.position.x > transform.position.x) dir = Vector3.right;
        else dir = Vector3.left;

        while (true) // x 좌표 이동
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
                break;

            transform.parent = collision.transform;
            yield return new WaitForFixedUpdate();
            
            transform.position += dir * _moveSpeed * Time.fixedDeltaTime;

            time += Time.fixedDeltaTime;

            if (time >= timeExpected)
                break;
        }
        
        isClimbing = false;
        // rigid.simulated = true;
        yield return new WaitUntil(()=>(isGround));
        _animator.SetBool("isClimbing", isClimbing);
        transform.parent = null;
        isJumpCancelable = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (!isGround && !isClimbing)
            {
                // 벽 타고 오르기
                float colHeight = collision.collider.bounds.size.y;
                float deltaHeight = collision.transform.position.y + colHeight / 2f - transform.position.y;
                float playerHeight = playerCollider.bounds.size.y;
                if (deltaHeight >= (minLengthHangable - 0.5f) * playerHeight && deltaHeight <= (maxLengthHangable - 0.5f) * playerHeight) // 올라탈 수 있는 가능 범위
                {
                    StartCoroutine(Climb(deltaHeight, collision));
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        float y = collision.transform.position.y + collision.bounds.size.y / 2f;
        float playerY = transform.position.y + playerCollider.offset.y * transform.localScale.y - playerCollider.bounds.size.y / 2f;
        if (collision.tag == "Ground" && playerY >= y - 0.05f)
        {
            isGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        float y = collision.transform.position.y + collision.bounds.size.y / 2f;
        float playerY = transform.position.y + playerCollider.offset.y * transform.localScale.y - playerCollider.bounds.size.y / 2f;
        if (collision.tag == "Ground" && playerY >= y - 0.05f)
        {
            isGround = false;
        }
    }

}
