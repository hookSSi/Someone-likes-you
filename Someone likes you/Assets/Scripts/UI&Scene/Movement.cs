using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Movemenet */
/* 물체의 자연스러운 움직을 위한 스크립트 */
/* ex) 휴대폰, 트럭 */

public class Movement : MonoBehaviour
{
    public void MoveVertical(GameObject obj, float value, float time = 1, bool moveType = true)
    {
        Vector3 origin = obj.gameObject.transform.position;
        Vector3 dest = new Vector3(origin.x + value, origin.y, origin.z);

        MoveToDest(obj, dest, time, moveType);
    }
    public void MoveHorizontal(GameObject obj, float value, float time = 1, bool moveType = true)
    {
        Vector3 origin = obj.gameObject.transform.position;
        Vector3 dest = new Vector3(origin.x, origin.y + value, origin.z);

        MoveToDest(obj, dest, time, moveType);
    }

    public void MoveToDest(GameObject obj ,Vector3 dest, float time = 1, bool moveType = true)
    {
        if(moveType) // 시간에 따라 천천히 이동
        {
            Vector3 origin = obj.transform.position;

            obj.transform.position = Vector3.Lerp(origin, dest, Time.deltaTime / time);
        }
        else // 바로 이동
        {
            obj.transform.position = dest;
        }
    }
}
