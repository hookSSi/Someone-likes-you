using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief
 *  공통적인 움직임을 정의할 함수를 담는 클래스
 *  @author 문성후
 *  @date   2019.08.16
 */

public class Movement : MonoBehaviour
{
    /**
     *  @brief
     *  이동 관련 함수
     *  @param obj 이동할 게임 오브젝트
     *  @param dest 목표 위치
     *  @param speed 속도
     *  @param mode  이동 모드 true = Velocity Move, false = Position Move
     *  @param debug  디버그 사용여부 true = 디버그 사용, false = 디버그 사용안함
     */
    public Vector3 Move(GameObject obj, Vector3 dest, float speed, bool mode = false, bool isDebug = false)
    {
        Vector3 origin = obj.transform.position;

        if(mode)
        {
            obj.GetComponent<Rigidbody2D>().velocity = dest.normalized * speed;
        }
        else
            obj.transform.position = dest;

        if(isDebug)
        {
            
        }

        return origin;
    }
    public Vector3 AddForce(GameObject obj, Vector3 dir, float amount)
    {
        Vector3 origin = Vector3.zero;

        return origin;
    }
    public void Jump(GameObject obj, float amout)
    {

    }
}