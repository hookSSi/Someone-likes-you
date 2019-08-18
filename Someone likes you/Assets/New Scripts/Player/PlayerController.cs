using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief
 *  플레이어 담당 일진 클래스
 *  @author 문성후
 *  @date   2019.08.16
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
    public PlayerMovement _movement;
    /// 플레이어의 상태
    public PlayerState _state;
    /// 플레이어의 Animator
    public Animator _animator;
    public List<Command> _commands;
    /// 플레이어의 굶주림 정도 (0~1) = (먹어야함 ~ 배가 꽉차 있음)
    public float _hungriness; 
    /// 플레이어의 걷는 속도
    public float _walkingSpeed; 
    /// 플레이어의 점프 크기
    public float _jumpingPower; 
    /**
     * 플레이어 클래스 초기화
     * @brief
     * 플레이어 클래스를 초기화하는 함수
     */
    private void Start() 
    {
        // 필수적인 스크립트 자동추가
        if(this._movement == null)
            this._movement = this.gameObject.AddComponent<PlayerMovement>();
        if(this._state == null)
        {
            this._state    = this.gameObject.AddComponent<PlayerState>();
            this._state._animator = this._animator;
        }
            

        /// @brief
        /// 이동, 마우스 위치 등 Axis를 제외한 키만 취급
        _commands = new List<Command>();
        _commands.Add(new Command(KeyCode.Mouse0, Attack));
        _commands.Add(new Command(KeyCode.W, Jump));
        _commands.Add(new Command(KeyCode.E, Interect));
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
     * @param dir 이동 방향 Vector3(-1 or 1)
     */
    public void Move(float dir)
    {
        this._movement.Move(this.transform, Vector3.right * dir, _walkingSpeed, true);
        _state.NotifyData(dir);
    }
    /// 플레이어 공격
    public void Attack()
    {
        Debug.Log("공격!");
    }
    /// 플레이어 점프
    public void Jump()
    {
        Debug.Log("점프!");
    }
    /// 플레이어 상호작용
    public void Interect()
    {
        Debug.Log("상호작용!");
    }
    
}
