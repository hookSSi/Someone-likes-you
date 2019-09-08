using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief 뺀돌뺀돌 혼자서도 잘 움직이는 벽
 *  @author 한민서
 *  @date   2019.08.28
 *  @see https://youtu.be/ek0JZrL6BU4 1분 6초 참고
 */

public class MovingPlatform : MonoBehaviour
{
    /// 플랫폼 오브젝트
    [Header("플랫폼")]
    public PlatformCatcher _platform;
    
    /// 도착지의 위치, 빈 게임 오브젝트를 목적지로 삼을 것임!
    [Header("목적지")]
    public List<Transform> _checkPointList;
    /// 현재 목표로 하고 있는 posList의 인덱스
    protected int _index = 0;
    /// 인덱스(_index)가 어떻게 변할지를 결정, -1은 역순, 1은 순방향
    private int _indexDir = 1;
    
    /**
     *  @brief 벽의 이동 방법을 결정하는 모드
     *  @details
     *  벽이 지점 사이를 어떻게 왕복할 것인지를 결정합니다. 약간 '미니 메트로' 같은 느낌? @n
     *  LOOP : 순회노선처럼, 지점 사이를 순회. @n
     *  BACK_FORTH : LOOP처럼 모든 지점을 지나치되, 양방향으로 처음과 중간지점, 끝을 거쳐 왕복한다. @n
     *  ONE_WAY : 편도선, 한 번씩 지나친 후 움직임을 멈춘다. MoveStart로 다시 움직인다.
     *  @see https://youtu.be/ek0JZrL6BU4 1분 6초 참고
     */
    public enum MoveMode
    {
        LOOP, BACK_FORTH, ONE_WAY
    }

    /// 기본 MoveMode는 Loop(순회하기)
    [Header("이동 모드(순회, 양방향, 편도)")]
    public MoveMode _moveMode = MoveMode.LOOP;

    /// 벽이 움직이는 최대 속력
    [Header("속도")]
    public float _velocity = 6f;

    /// 벽이 얼마나 멈췄다가 출발할지를 결정하는 변수
    [Header("출발하기 전에 얼마나 쉬었다가 갈까?")]
    public float _waitTime = 1f;
    /// 현재 경과 시간(건드리지 말것!)
    private float _currentTime = 0f;

    /// 벽이 부드럽게 정지하는 데 걸리는 시간
    [Header("제로백 시간")]
    [SerializeField] private float _timeForStop = 0.7f;

    /// 벽의 이동 상태를 나타낸다.
    enum WallState
    {
        Idle, Move
    };
    
    private WallState _state = WallState.Move;

    /// 벽이 움직임이 활성화되었는가?
    private bool _isOn = true;
    /// Act 함수가 FixedUpdate에서 실행되는데, Act의 Coroutine이 한 번만 실행되게 하는 트리거 변수
    private bool _trigger = true;

    private void Awake()
    {
        if (!_platform)
            _platform = GetComponentInChildren<PlatformCatcher>();
    }

    private void FixedUpdate()
    {
        if (_isOn)
            Act();
        else
            _trigger = true;
    }
    /// 이 함수를 통해 외부에서 해당 오브젝트를 활성화(이동 시작)한다.
    public void MoveStart()
    {
        _trigger = true;
        _isOn = true;
    }
    /// 이 함수를 통해 외부에서 해당 오브젝트를 비활성화(이동 중지)한다.
    public void MoveStop()
    {
        /// 플랫폼이 비활성화되었다가 다시 활성화되면, 이전에 이동하던 곳으로 이동한다.
        _isOn = false;
    }

    /// 상태에 따라서 벽이 움직이게 하는 함수
    private void Act()
    {
        switch(_state)
        {
            case WallState.Idle:
                _currentTime += Time.deltaTime;
                if (_currentTime >= _waitTime)
                {
                    NextTo();
                    _index += _indexDir;
                    _trigger = true;
                    _state = WallState.Move;
                    _currentTime = 0f;
                }
                break;
            case WallState.Move:
                if (_trigger)
                {
                    StartCoroutine("MoveTo");
                    _trigger = false;
                }
                break;
        }
    }
    
    /// 다음 오브젝트로 넘어간다
    private void NextTo()
    {
        // 정방향 순회시 발동
        if (_indexDir == 1 && _index >= _checkPointList.Count - 1)
        {
            switch(_moveMode)
            {
                case MoveMode.LOOP:
                    _index = -1;                                
                    break;
                case MoveMode.BACK_FORTH:
                    _indexDir *= -1;
                    break;
                case MoveMode.ONE_WAY:
                    _indexDir *= -1;
                    MoveStop();
                    break;
            }
        }
        // 역방향 순회시 발동
        else if (_indexDir == -1 && _index <= 0)
        {
            switch(_moveMode)
            {
                case MoveMode.LOOP:
                    _index = _checkPointList.Count;                                
                    break;
                case MoveMode.BACK_FORTH:
                    _indexDir *= -1;
                    break;
                case MoveMode.ONE_WAY:
                    _indexDir *= -1;
                    MoveStop();
                    break;
            }
        }
    }
    
    /**
     *  @brief 지정된 위치로 벽이 이동한다!
     *  @details 지정된 위치를 향해 일정한 속도로 이동하지만, 부드러운 정지를 위해 시작 위치와 목적지 근방에서
     *  가속을 이용합니다. @n
     *  적당히 시간 계산해서 그 스케줄 따라 움직입니다.
     */
    IEnumerator MoveTo()
    {
        if (_state == WallState.Idle)
            yield break;
        
        Transform _curOrigin = _platform.transform;
        Transform _curDest = _checkPointList[_index];

        Vector3 _dir = (_curDest.position - _curOrigin.position).normalized;

        float _curVelocity = 0f;
        float _dist = (_curDest.position - _curOrigin.position).magnitude;
        /// 예상시간(_estTime) : 목적지까지 도달하는 데 걸리는 시간. 목적지가 강제로 옮겨지면 변할 수도 있다.
        float _estTime = (_dist < _velocity * _timeForStop) ?
                               2 * Mathf.Sqrt(_dist * _timeForStop / _velocity):
                               _timeForStop + _dist / _velocity;
        // Debug.Log("예상 시간 : " + _estTime);
        _currentTime = 0f;
        
        /// 예상 시간(_estTime) 동안 while문으로 계속 이동한다.
        while(_currentTime < _estTime)
        {
            // 시간에 따라 속도를 설정한다.
            if (!_isOn || (_currentTime > _estTime / 2 && _currentTime > _estTime - _timeForStop))
            {
                _curVelocity -= _velocity / _timeForStop * Time.deltaTime;
                if (_curVelocity < 0.01f)
                    _curVelocity = 0f;
            }
            else if (_currentTime < _estTime / 2 && _curVelocity < _velocity)
            {
                _curVelocity += _velocity / _timeForStop * Time.deltaTime;
            }
            else
                _curVelocity = _velocity;
            
            _platform.transform.position += _dir * _curVelocity * Time.deltaTime;
            
            /// 목적지로부터 너무 크게 빗겨가지 않게, 적당히 가까워지면 정지한다(목적지를 강제로 이동시킬 경우 문제가 생길 수 있음)
            if ((_curDest.position - _platform.transform.position).sqrMagnitude <= 0.0003)
                break;
            
            _currentTime += Time.deltaTime;

            if (!_isOn && _curVelocity == 0f)
            {
                _currentTime = 0f;
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
        
        /// pos 위치가 이동했을 땐 한 번 더 이동해서 보정한다.
        if ((_curDest.position - _platform.transform.position).sqrMagnitude > 0.0003)
        {
            StartCoroutine("MoveTo");
            yield break;
        }
        _platform.transform.position = _curDest.position;

        _currentTime = 0f;

        _state = WallState.Idle;
    }
}
