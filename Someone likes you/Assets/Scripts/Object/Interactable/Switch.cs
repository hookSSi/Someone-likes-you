#define DEBUG

using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

/**
 *  @brief 상호작용할 수 있는 스위치
 *  @detail
 *  어떤 스위치냐에 따라 실행하는 함수가 달라지기 때문에,@n
 *  인스펙터에서 실행할 함수를 정할 수 있도록 UnityEvent를 사용함.@n
 *  ReusableMode : 스위치를 재사용할 수 있는지를 정하는 모드@n
 *  - REUSABLE 스위치를 계속 사용할 수 있음@n
 *  - SINGLE_USE 조작은 계속할 수 있지만 작동은 한 번만.@n
 *  - USE_AND_BREAK 딱 한 번 키거나, 끌 수 있다. 이벤트 트리거용.@n
 *  SwitchType : 키고 끄는 방식을 선택할 수 있다.@n
 *  - SWITCH : 일반적인 스위치처럼 키고 끌 수 있다.@n
 *  - BUTTON : 마인크래프트 버튼처럼, 활성화하면 잠시 후 Off로 돌아간다.
 */
public class Switch : InteractableObject
{
    [Header("스프라이트")]
    /// 스프라이트를 플레이 도중 교체하기 위해 저장받는 스프라이트 렌더러
    [SerializeField] private SpriteRenderer render;

    /// 스위치가 켜졌을 때의 스프라이트
    public Sprite _spriteActive;
    /// 스위치가 꺼졌을 때의 스프라이트
    public Sprite _spriteDeactive;


    /**
     *  @brief 스위치를 재사용할 수 있는지를 정하는 모드
     *  @detail
     *  REUSABLE : 재사용 가능한 모드@n
     *  SINGLE_USE :@n조작(온오프)은 계속 가능하지만 작동은 한 번만 가능하다.@n
     *  USE_AND_BREAK : 한 번 조작하면 다시 조작할 수 없다.
     */
    public enum ReusableMode
    {
        REUSABLE, SINGLE_USE, USE_AND_BREAK
    }
    
    /**
     *  @brief 키고 끄는 방식을 선택할 수 있다.
     *  @detail
     *  - SWITCH : 일반적인 스위치처럼 키고 끌 수 있다.@n
     *  - BUTTON : 마인크래프트 버튼처럼, 활성화하면 잠시 후 Off로 돌아간다.
     */
    public enum SwitchType
    {
        SWITCH, BUTTON
    }
    [Header("스위치 모드")]
    [Header("재사용 가능 관련")]
    /// 재사용 모드(기본 REUSABLE)
    public ReusableMode _mode = ReusableMode.REUSABLE;

    [Header("스위치 타입")]
    /// 스위치 타입(기본 SWITCH)
    public SwitchType _type = SwitchType.SWITCH;

    /// 버튼일 경우 켜고 꺼지는 데 걸리는 딜레이 시간
    [SerializeField] private float _buttonDelay = 1.0f;
    private float _currentTime;

    /// 스위치가 켜졌을 때 일어날 일들
    [Header("스위치가 켜질 때 할 것들")]
    public UnityEvent _OnEvent;

    /// 스위치가 꺼질 때 일어날 일들
    [Header("스위치가 꺼질 때 할 것들")]
    public UnityEvent _OffEvent;


    /// 스위치의 상태(On, Off)
    [Header("스위치의 상태(인게임에선 강제변경불가)")]
    [SerializeField] protected bool _isActive;

    /// 스위치를 사용할 수 있는지를 결정하는 변수. ReusableMode에 따라 자동으로 변경되니 건드리지 마시오.
    protected bool _isUsable = true;


    #if DEBUG
    [Header("[Debug] 강제 상호작용")]
    /// 디버그를 위한 강제 Interact 발생용 변수
    public bool _testTrigger = false;
    #endif


    /// 변수 초기화
    void Awake()
    {
        render = gameObject.GetComponentInChildren<SpriteRenderer>();
        render.sprite = _isActive ? _spriteActive : _spriteDeactive;
    }

    /// 테스트 및 디버그용
    void Update()
    {
        #if DEBUG
        if (_testTrigger)
        {
            _testTrigger = false;
            OnNotify();
        }
        #endif

        if (_type == SwitchType.BUTTON && _isActive)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > _buttonDelay)
            {
                SwitchAct();
                _currentTime -= _buttonDelay;
            }
        }
    }

    /// 상호작용시 스위치를 조작할 수 있게 한다(Interact 구현 후에나 버그가 없는지 확인할 수 있겠다...)
    public override void OnNotify()
    {
        if (!_isUsable)
        {
            switch(_mode)
            {
                case ReusableMode.REUSABLE:
                    _isUsable = true;
                    break;
                case ReusableMode.SINGLE_USE:
                    Debug.Log("사용할 수 없는 장치인 듯하다.");
                    break;
                case ReusableMode.USE_AND_BREAK:
                    Debug.Log("사용할 수 없는 장치인 듯하다.");
                    return;
            }
        }

        // Button 타입은 킬 수만 있다.
        if (_type == SwitchType.BUTTON && _isActive)
            return;

        SwitchAct();

        switch(_mode)
        {
            case ReusableMode.REUSABLE:
                    break;
                case ReusableMode.SINGLE_USE:
                case ReusableMode.USE_AND_BREAK:
                    _isUsable = false;
                    return;
        }
    }
    
    /// 스위치 작동
    private void SwitchAct()
    {
        _isActive = !_isActive ? true : false;
        if (_isActive)
        {
            // Debug.Log("켜졌어!");
            if (_isUsable)
                _OnEvent.Invoke();
            SwitchOn();
        }
        else
        {
            // Debug.Log("꺼졌어!");
            if (_isUsable)
                _OffEvent.Invoke();
            SwitchOff();
        }
    }

    /// 스위치가 켜질 때 스위치의 스프라이트(또는 애니메이션)나 사운드 따위를 재생하는 함수
    protected void SwitchOn()
    {
        render.sprite = _spriteActive;
    }
    /// 스위치가 꺼질 때 스위치의 스프라이트(또는 애니메이션)나 사운드 따위를 재생하는 함수
    protected void SwitchOff()
    {
        render.sprite = _spriteDeactive;
    }
}
