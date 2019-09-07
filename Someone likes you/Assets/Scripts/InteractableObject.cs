using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief
 *  상호작용 가능한 오브젝트를 위한 추상클래스
 *  상호작용하는 오브젝트들은 모두 이 클래스를 상속받을 것!
 *  @author 문성후
 *  @date   2019.09.03
 */
public abstract class InteractableObject : MonoBehaviour
{
    public delegate void OnInteractEvent(GameObject obj);
    /// 상호작용시 발생할 이벤트
    public event OnInteractEvent OnInteract;
    /// 상호작용시 부르는 함수
    public virtual void OnNotify(){}
}
