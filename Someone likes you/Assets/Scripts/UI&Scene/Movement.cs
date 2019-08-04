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
        Vector3 origin = obj.transform.position;
        Vector3 dest = new Vector3(origin.x, origin.y + value, origin.z);

        StartCoroutine(Co_Move(obj, dest, time, moveType));
    }
    public void MoveHorizontal(GameObject obj, float value, float time = 1, bool moveType = true)
    {
        Vector3 origin = obj.transform.position;
        Vector3 dest = new Vector3(origin.x + value, origin.y, origin.z);

        StartCoroutine(Co_Move(obj, dest, time, moveType));
    }

    public IEnumerator Co_Move(GameObject obj, Vector3 dest, float time = 1, bool moveType = true)
    {
        float t = 0;
        Vector3 origin = obj.transform.position;

        if(moveType)
        {
            while(t < time)
            {
                t += Time.deltaTime;
                obj.transform.position = Vector3.Lerp(origin, dest, t / time);
                yield return null;
            }
        }
        else
        {
            obj.transform.position = dest;
            yield break;
        }
    }

}
