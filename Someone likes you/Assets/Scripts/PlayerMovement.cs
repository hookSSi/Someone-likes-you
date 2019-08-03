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
    public float climbJumpPower = 1.8f;

    public float minLengthHangable = 0.4f; // collider 가장 위에서부터 아래로 길이(비율) - 머리 크기인 20 픽셀 * 2(올라탈 수 있는 범위)(1 픽셀 : 0.01)
    public float maxLengthHangable = 0.2f; // collider 가장 위에서부터 위로의 길이(비율)

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
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // 강제로 스프라이트를 뒤집는 방법
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                Move(Vector3.right);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            Jump();
            _animator.SetFloat("speed", Mathf.Abs(rigid.velocity.x));
        }
    }

    private void Move(Vector3 dir) // Move 함수가 position을 이동시키는 것 때문에, 관통 현상이 일어나는 것 같음. AddForce나 Velocity를 이용할 순 없을까?
    {
        Vector3 vel = dir * moveSpeed;
        rigid.velocity = vel + Vector3.up * rigid.velocity.y;
    }

    private void Jump()
    {
        // 현재 버그 발견. 일정 이상의 속도일 경우 각을 잘 맞추면(블럭과 블럭 사이?) 바닥에 닿을 때 바닥을 그대로 뚫고 감
        // 블럭 사이로 관통해버린다...
        if (!isJumping || !isGround) // 점프 커맨드 입력
            return;

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

    IEnumerator Climb(float deltaHeight, bool isRight) // 파쿠르 강제 올라가기
    {
        // Scale을 강제로 조절한 플랫폼은 파쿠르가 이상하게 작동하는 버그가 있어서 이를 수정했습니다.
        // - 오브젝트의 Scale을 강제로 조절해도, Collider의 수치 상의 offset과 size는 변하지 않기 때문!
        // - size에 scale을 강제로 곱하여 해결하였으며, 때문에 Collider의 size 또는 offset에 관련된 수에 localScale.x 또는 localScale.y가 곱해졌습니다.
        Vector3 startPosition = transform.position;
        Vector3 dir;
        float time = 0;
        float _moveSpeed;
        float distance = deltaHeight + ((BoxCollider2D)playerCollider).size.y * transform.localScale.y / 2; // 올라가야 하는 거리
        float timeExpected = distance / climbSpeed;

        if(isRight) dir = Vector3.right;
        else dir = Vector3.left;
        isClimbing = true;
        rigid.simulated = false;
        rigid.velocity = Vector2.zero;

        while (true) // y 좌표 이동
        {
            yield return new WaitForFixedUpdate();
            transform.position += Vector3.up * (climbSpeed * Time.fixedDeltaTime);
            time += Time.fixedDeltaTime;

            if (time >= timeExpected) // 어느 정도의 시간이 흐르면
            {
                break;
            }
        }

        time = 0;
        distance = ((BoxCollider2D)playerCollider).size.x * transform.localScale.x; // 플레이어 사이즈 만큼 x축으로 이동


        yield return new WaitForFixedUpdate();
        rigid.simulated = true;
        isJumpCancelable = false;
        Jump(climbJumpPower);
        //Debug.Log("현재 속도: " + rigid.velocity.y);
        yield return new WaitForFixedUpdate();

        timeExpected = - rigid.velocity.y * 2 / (rigid.gravityScale * Physics2D.gravity.y);
        _moveSpeed = distance / timeExpected;

        while (true) // x 좌표 이동
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
                break;

            yield return new WaitForFixedUpdate();
            
            transform.position += Vector3.right * _moveSpeed * Time.fixedDeltaTime;

            time += Time.fixedDeltaTime;

            if (time >= timeExpected)
                break;
        }
        
        isClimbing = false;
        yield return new WaitUntil(()=>(isGround));
        isJumpCancelable = true;
    }

    private void OnCollisionStay2D(Collision2D collision) // 벽 타고 오르기
    {
        // localScale.y가 마이너스가 되면 대참사가 벌어질 수도 있으니 주의....
        if (collision.collider.tag == "Ground"/*&& collision.transform.position.y < transform.position.y*/)
        {
            if (!isGround)
            {
                Vector3 colScale = collision.transform.localScale;
                float colHeight = ((BoxCollider2D)collision.collider).size.y * colScale.y;
                float deltaHeight = collision.transform.position.y + colHeight / 2f - (playerCollider.offset.y * transform.localScale.y + transform.position.y);
                float playerHeight = ((BoxCollider2D)playerCollider).size.y * transform.localScale.y;
                if (deltaHeight <= playerHeight / 2f + maxLengthHangable * playerHeight && deltaHeight >= playerHeight / 2f - minLengthHangable * playerHeight) // 올라탈 수 있는 가능 범위
                {
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
