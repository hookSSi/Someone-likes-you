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
     *  @param mode  이동 모드 (true = Velocity Move, false = Position Move)
     *  @param debug  디버그 사용여부 (true = 디버그 사용, false = 디버그 사용안함)
     */
    public virtual Vector3 Move(Transform obj, Vector3 dest, float speed, bool mode = false, bool isDebug = false)
    {
        Vector3 origin = obj.position;

        if(mode)
        {
            obj.position += dest.normalized * speed * Time.deltaTime;   
        }
        else
            obj.position = dest;

        if(isDebug)
        {
            if(mode)
                Debug.Log(string.Format("{0}에서 {1}으로 {2}의 속도로 이동", origin, dest.normalized, speed));
            else
                Debug.Log(string.Format("{0}에서 {1}로 이동", origin, dest));
        }

        return origin;
    }
    public virtual Vector3 AddForce(Rigidbody2D rigid, Vector3 dir, float amount, ForceMode2D mode)
    {
        Vector3 origin = rigid.position;

        rigid.AddForce(dir.normalized * amount, mode);

        return origin;
    }
    public virtual void Jump(GameObject obj, float amout)
    {
        Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

        if(rigid)
        {
            rigid.AddForce(Vector3.up, ForceMode2D.Impulse);
        }
        else
        {
            obj.transform.position += Vector3.up * amout;
        }
    }
}