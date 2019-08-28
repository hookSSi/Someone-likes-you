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

    [Space(10)]
    [Header("플레이어 조작커맨드 목록")]
    [SerializeField] private List<Command> _commands;
    [Header("플레이어에게 땅인 레이어들")]
    [SerializeField] private LayerMask _whatIsGround;

    [Space(10)]
    [Header("플레이어 정보")]
    /// 플레이어의 굶주림 정도 (0~1) = (먹어야함 ~ 배가 꽉차 있음)
    [Range(0, 1)] [SerializeField]public float _hungriness; 
    /// 플레이어의 걷는 속도
    [SerializeField] private float _walkingSpeed; 
    /// 플레이어의 점프 크기
    [SerializeField] private float _jumpForce; 

    const float _cellingRadius = .2f;
    const float _groundedRadius = .2f;
    /**
     * 플레이어 클래스 초기화
     * @brief
     * 플레이어 클래스를 초기화하는 함수
     */
    private void Awake() 
    {
        // 필수적인 스크립트 자동추가
        if(this._movement == null)
        {
            this._movement = this.gameObject.AddComponent<PlayerMovement>();
            this._movement.Init(this.GetComponent<Rigidbody2D>());
        }
        if(this._state == null)
        {
            this._state    = this.gameObject.AddComponent<PlayerState>();
            this._state._animator = this._animator;
        }

        /// @brief
        /// 이동, 마우스 위치 등 Axis를 제외한 키만 취급
        _commands.Add(ScriptableObject.CreateInstance<Command>().Init(KeyCode.Mouse0, Attack));
        _commands.Add(ScriptableObject.CreateInstance<Command>().Init(KeyCode.Space, Jump));
        _commands.Add(ScriptableObject.CreateInstance<Command>().Init(KeyCode.E, Interect));
    }
    private void FixedUpdate() 
    {
        if(_movement.GroundCheck(_groundCheck.position, _groundedRadius, _whatIsGround))
            _state.NotifyState(PlayerState.OnGround.LANDING, PlayerState.OffGround.NONE);

        if(!_movement._isGround)
            _state.NotifyState(PlayerState.OnGround.NONE, PlayerState.OffGround.FALLING);
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
        Move(Input.GetAxis("Horizontal"));

        foreach (Command command in _commands)
        {
            command.CheckKey();
        }
    }
    /** 플레이어 이동
     * @brief
     * 입력 받은 방향에 따라 플레이어 이동을 담당하는 함수
     * @param dir 이동 방향 Vector3(-1 ~ 1)
     */
    public void Move(float move)
    {
        if(Mathf.Abs(move) < 0.0001)
        {
            _state.NotifyState(PlayerState.OnGround.IDLE, PlayerState.OffGround.NONE);
        }
        else
        {
            if(_movement._isGround)
                _state.NotifyState(PlayerState.OnGround.WALKING, PlayerState.OffGround.NONE);
            else
                _state.NotifyState(PlayerState.OnGround.NONE, PlayerState.OffGround.FALLING);
        }
        
        Vector3 dir = Vector3.right * move * _walkingSpeed;
        this._movement.Move(dir);

        _sprite.transform.localScale = new Vector3(transform.localScale.x * ((_movement._prevDir >= 0) ? 1:-1), transform.localScale.y, transform.localScale.z); // 스프라이트 좌우 교체
    }
    /// 플레이어 공격
    public void Attack()
    {
        Debug.Log("공격!");
    }
    /// 플레이어 점프
    public void Jump()
    {
        this._movement.Jump(Vector2.up, _jumpForce);
        _state.NotifyState(PlayerState.OnGround.NONE, PlayerState.OffGround.JUMPING);
        Debug.Log("점프!");
    }
    /// 플레이어 상호작용
    public void Interect()
    {
        Debug.Log("상호작용!");
    }
}
