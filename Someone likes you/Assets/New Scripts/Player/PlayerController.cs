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
        
    }
    /// 플레이어 입력 처리
    public void HandleInput(){}
    /** 플레이어 이동
     * @brief
     * 입력 받은 방향에 따라 플레이어 이동을 담당하는 함수
     * @param dir 이동 방향 Vector3(-1 or 1)
     */
    public void Move(Vector3 dir){}
    /// 플레이어 공격
    public void Attack(){}
    /// 플레이어 점프
    public void Jump(){}
    /// 플레이어 상호작용
    public void Interct(){}
    
}
