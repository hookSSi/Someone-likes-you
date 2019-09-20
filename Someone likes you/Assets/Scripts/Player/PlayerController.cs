using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief
 *  플레이어 담당 일진 클래스
 *  @author 문성후
 *  @date   2019.08.16
 *  @see 참고: https://github.com/Brackeys/2D-Character-Controller/blob/master/CharacterController2D.cs
 */

public class PlayerController : MonoBehaviour
{
    /**
    *   @brief 
    *   플레이어는 Observer이자 Subject
    *   부를 이벤트 형식을 delegate로 정의
    *   @see https://boycoding.tistory.com/107
    *   @see https://hyunity3d.tistory.com/528
    */

    public delegate void NotifyEvent(GameObject entity);
    public static event NotifyEvent Notify;

    /// 플레이어 움직임을 제어하는 클래스
    private PlayerMovement _movement;
    /// 플레이어의 상태
    private PlayerState _state;

    /// 플레이어의 Animator
    [SerializeField] private Animator _animator;
    /// 플레이어의 Sprite
    [SerializeField] private GameObject _sprite;

    /// 땅인지 체크하는 포지션
    [SerializeField] private Transform _groundCheck;
    /// 위에 천장이 닿이는 지 체크하는 포지션(천장이 너무 낮으면 자동으로 앉도록)
	[SerializeField] private Transform _ceilingCheck;
    /// 벽을 체크하는 포지션
    [SerializeField] private Transform _wallCheck;

    [Space(10)]
    [Header("플레이어 조작커맨드 목록")]
    /// 키 입력 한번만 처리
    [SerializeField] private List<Command> _commandsGetKeyDown;
    /// 키 입력 누르고 있을 경우 처리
    [SerializeField] private List<Command> _commandsGetKey;
    [Header("플레이어에게 땅인 레이어")]
    [SerializeField] private LayerMask _whatIsGround;
    [Header("플레이어에게 벽인 레이어")]
    [SerializeField] private LayerMask _whatIsWall;

    [Space(10)]
    [Header("플레이어 정보")]
    /// 플레이어의 굶주림 정도 (0~1) = (먹어야함 ~ 배가 꽉차 있음)
    [Range(0, 1)] [SerializeField]public float _hungriness;
    /// 플레이어의 이동 방향
    [SerializeField] private float _horizontalMove;
    /// 플레이어의 걷는 속도
    [SerializeField] private float _walkingSpeed; 
    /// 플레이어의 점프 크기
    [SerializeField] private float _jumpForce;
    /// 플레이어 일반 중력 저항
    [SerializeField] private float _normalFallMultipler;
    /// 플레이어가 점프키를 길게 누르는 동안 중력 저항
    [SerializeField] private float _lowFallMultipler;
    /// 플레이어의 벽타기중 하강 속도
    [SerializeField] private float _wallSlidingSpeed;

    /// 천장
    const float _ceilingRadius = .2f;
    /// 땅
    const float _groundedRadius = .18f;
    /// 벽
    const float _wallDistance = .3f;
    /**
     * 플레이어 클래스 초기화
     * @brief
     * 플레이어 클래스를 초기화하는 함수
     */
    private void Awake() 
    {
        // 필수적인 스크립트 자동추가
        if(this._state == null)
        {
            this._state    = this.gameObject.AddComponent<PlayerState>();
            this._state._animator = this._animator;
        }
        if(this._movement == null)
        {
            this._movement = this.gameObject.AddComponent<PlayerMovement>();
            this._movement.Init(this.GetComponent<Rigidbody2D>(), this._state);
        }

        /// @brief
        /// 이동, 마우스 위치 등 Axis를 제외한 키만 취급
        _commandsGetKeyDown.Add(ScriptableObject.CreateInstance<Command>().Init(KeyCode.Mouse0, Attack));
        _commandsGetKeyDown.Add(ScriptableObject.CreateInstance<Command>().Init(KeyCode.Space, Jump)); // 단점프
        _commandsGetKeyDown.Add(ScriptableObject.CreateInstance<Command>().Init(KeyCode.E, Interact));

        _commandsGetKey.Add(ScriptableObject.CreateInstance<Command>().Init(KeyCode.Space, HoldJumpKey)); // 장점프
    }
    private void FixedUpdate() 
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * Time.deltaTime;

        if(!_movement._isGround)
        {
            // 벽 타지 않을때
            _movement.Down(_normalFallMultipler, _lowFallMultipler);
            
            if(_movement.WallCheck(_wallCheck.position, _horizontalMove, _wallDistance, _whatIsWall))
            {
                Debug.Log("벽 타는 중");
                // 벽 탈때
                _movement.WallSliding(_wallSlidingSpeed * Time.deltaTime);
            }
        }

        if(_movement.GroundCheck(_groundCheck.position, _groundedRadius, _whatIsGround))
            Debug.Log("착지!");

        Move(_horizontalMove);
    }
    private void Update()
    {
        HandleInput();
    }
    /**
     * 플레이어의 입력 처리
     * @brief
     * 플레이어의 입력을 처리하는 함수로 자유롭게 키를 변경할 수 있도록
     * 중간 클래스를 만들어서 사용한다.
     */
    public void HandleInput()
    {
        foreach (Command command in _commandsGetKeyDown)
        {
            command.CheckGetKeyDown();
        }
        foreach (Command command in _commandsGetKey)
        {
            command.CheckGetKey();
        }
    }
    /** 플레이어 이동
     * @brief
     * 입력 받은 방향에 따라 플레이어 이동을 담당하는 함수
     * @param move 이동 방향 Vector3(-1 ~ 1)
     * @return 가는 방향 vector 리턴
     */
    public Vector3 Move(float move)
    {
        Vector3 dir = Vector3.right * move * _walkingSpeed;
        this._movement.Move(dir);
        Flip(_movement._prevDir);

        return dir;
    }
    /// 플레이어 반전
    public void Flip(float dir)
    {
        Vector3 dirToVector = new Vector3(transform.localScale.x * ((dir >= 0) ? 1:-1), transform.localScale.y, transform.localScale.z);

        _sprite.transform.localScale                  = dirToVector; // 스프라이트 좌우 교체
        _ceilingCheck.parent.transform.localScale     = dirToVector; // 천장 체커 좌우 교체
    }
    /// 플레이어 공격
    public void Attack()
    {
        Debug.Log("공격!");
    }
    /// 플레이어 점프
    public void Jump()
    {
        this._movement.Jump(_jumpForce);
        Debug.Log("점프!");
    }
    public void HoldJumpKey()
    {
        _movement.HoldJumpKey();
    }
    /// 플레이어 상호작용
    public void Interact() => Debug.Log("상호작용!");
}
