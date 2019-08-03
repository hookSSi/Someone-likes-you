using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigid;
    Collider2D playerCollider;

    public float moveSpeed = 2.67f;
    public float jumpPower = 5.1f;
    public float climbSpeed = 2f;
    public float minLengthHangable = 0.4f; // collider 가장 위에서부터 아래로 길이 - 머리 크기인 20 픽셀 * 2(올라탈 수 있는 범위)(1 픽셀 : 0.01)
    public float maxLengthHangable = 0.2f; // collider 가장 위에서부터 위로의 길이

    bool isClimbing = false;

    private bool isJumping = false;
    public bool isGround = false;

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
        if (Input.GetButtonDown("Jump") && isGround == true)
        {
            isJumping = true;
            _animator.SetBool("isJumping", isJumping);
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
                Move(Vector3.left);
                transform.localScale = new Vector3(-1f, 1f, 1f); // 강제로 스프라이트를 뒤집는 방법
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                Move(Vector3.right);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            Jump();
            _animator.SetFloat("speed", Mathf.Abs(rigid.velocity.x));
        }
    }

    private void Move(Vector3 dir) // Move 함수가 position을 이동시키는 것 때문에, 관통 현상이 일어나는 것 같음. AddForce나 Velocity를 이용할 순 없을까?
    {
        Vector3 vel = dir * moveSpeed;
        rigid.velocity = vel;
    }

    private void Jump()
    {
        // 현재 버그 발견. 일정 이상의 속도일 경우 각을 잘 맞추면(블럭과 블럭 사이?) 바닥에 닿을 때 바닥을 그대로 뚫고 감
        // 블럭 사이로 관통해버린다...
        if (!isJumping || !isGround) // 점프 커맨드 입력
            return;

        rigid.velocity = Vector2.zero;

        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

        isJumping = false;
        isGround = false;
    }

    private void JumpCut()
    {
        float velocityY = -0.1f;

        if (rigid.velocity.y > velocityY)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, velocityY);
        }
    }

    IEnumerator Climb(float deltaHeight, bool isRight) // 파쿠르 강제 올라가기
    {
        // 현재 문제점 : Scale을 강제로 조절한 플랫폼은 파쿠르가 이상하게 작동하는 버그가 있습니다.
        // - 오브젝트의 Scale을 강제로 조절해도, Collider의 수치 상의 offset과 size는 변하지 않기 때문!
        // - 해결법 1. size에 scale을 강제로 곱하여 해결한다.
        // (굉장히 단순하지만 효과적인 방법, 하지만 상당히 귀찮고 코드를 알아보기 힘들 것 같습니다.)(아직 미적용)

        isClimbing = true;
        Debug.Log("삐빅 올라갑니다");
        rigid.simulated = false;
        rigid.velocity = Vector2.zero;
        float time = 0;

        float length = deltaHeight + ((BoxCollider2D)playerCollider).size.y / 2;
        Vector3 startPosition = transform.position;

        float timeExpected = length / (climbSpeed);
        // Debug.Log("예상 시간은 " + timeExpected + "초");

        while (true) // y 좌표 이동
        {
            transform.position += Vector3.up * (climbSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();

            time += Time.fixedDeltaTime;

            if (time > timeExpected) // 어느 정도의 시간이 흐르면
            {
                transform.position = startPosition + Vector3.up * length;
                break;
            }
        }

        time = 0;
        length = ((BoxCollider2D)playerCollider).size.x;
        timeExpected = length / climbSpeed;
        if (!isRight) length *= -1;

        while (true) // x 좌표 이동
        {
            if (isRight)
                transform.position += Vector3.right * climbSpeed * Time.fixedDeltaTime;
            else
                transform.position += Vector3.left * climbSpeed * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();

            time += Time.fixedDeltaTime;

            if (time > timeExpected)
            {
                transform.position = new Vector3(startPosition.x + length, transform.position.y, transform.position.z);
                break;
            }
        }

        rigid.velocity = Vector3.zero;
        rigid.simulated = true;
        isClimbing = false;
    }

    private void OnCollisionStay2D(Collision2D collision) // 벽 타고 오르기
    {
        if (collision.collider.tag == "Ground" && collision.transform.position.y < transform.position.y) // 넓게 범위 설정
        {
            if (!isGround)
            {
                float colHeight = ((BoxCollider2D)collision.collider).size.y;
                float deltaHeight = collision.transform.position.y + colHeight / 2f - (playerCollider.offset.y + playerCollider.transform.position.y);
                float playerHeight = ((BoxCollider2D)playerCollider).size.y;
                if (deltaHeight <= playerHeight / 2f + maxLengthHangable && deltaHeight >= playerHeight / 2f - minLengthHangable) // 올라탈 수 있는 가능 범위
                {
                    Debug.Log("올라갈 수 있을 것 같다.");

                    StartCoroutine(Climb(deltaHeight, collision.transform.position.x > transform.position.x));
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
            isGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
            isGround = false;
    }

}
