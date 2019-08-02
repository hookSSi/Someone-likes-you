using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigid;

    public float moveSpeed = 2.67f;
    public float jumpPower = 5.1f;

    private bool isJumping = false;
    public bool isGround = false;

    public Tool currentTool;

    public enum State
    {
        Idle,
        Moving,
        Jump,
        Hang,
        Attack
    }

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
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
