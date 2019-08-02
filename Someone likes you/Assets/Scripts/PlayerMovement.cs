using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigid;
    Collider2D playerCollider;

    public float moveSpeed = 2.67f;
    public float jumpPower = 5.1f;
    public float lengthHangable = 0.4f; // 머리 크기인 20 픽셀 * 2(올라탈 수 있는 범위)(1 픽셀 : 0.01)

    private bool isJumping = false;
    public bool isGround = false;

    public Tool currentTool;

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
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && isGround == true)
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
            ItemDatabase.GetInstance().currentTool++;
            //currentTool = ItemDatabase.GetInstance().CurrentTool();
        }

        if (Input.GetAxisRaw("ScrollWheel") > 0)
        {
            Debug.Log("삐빅 위 휠");
        }

    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        Vector3 moveVelocity = new Vector3();

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;

            transform.localScale = new Vector3(-1f, 1f, 1f); // 강제로 스프라이트를 뒤집는 방법
        }
        else if (Input.GetAxisRaw("Horizontal")>0)
        {
            moveVelocity = Vector3.right;

            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        transform.position += moveVelocity * Time.deltaTime * moveSpeed;
    }

    private void Jump()
    {
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

    private void Hang()
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && collision.transform.position.y < transform.position.y)
        {
            if (!isGround)
            {
                float colHeight = ((BoxCollider2D)collision.collider).size.y;
                float deltaHeight = collision.transform.position.y + colHeight / 2f - (playerCollider.offset.y + playerCollider.transform.position.y);
                float playerHeight = ((BoxCollider2D)playerCollider).size.y;
                if (deltaHeight <= playerHeight / 2f && deltaHeight >= playerHeight / 2f - lengthHangable) // 올라탈 수 있는 가능 범위
                {
                    Debug.Log("올라갈 수 있을 것 같다.");
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
