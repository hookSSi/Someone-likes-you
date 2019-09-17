// #define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief 열고 닫을 수 있는 문
 *  @detail 설정(_isInteractable)에 따라 상호작용할 수도 있고 아닐 수도 있다.@n
 *  Open과 Close로 문을 열고 닫을 수 있으며 상호작용을 위한 OnNotify가 있다.
 */
public class InteractableDoor : InteractableObject
{
    [SerializeField] private bool _isOpen = false;
    /// 상호작용으로 열리는 문인지 아닌지를 결정하는 변수
    [Header("상호작용으로 열 수 있는가?")]
    [SerializeField] private bool _isInteractable = false;

    #if DEBUG
    [Header("[Debug] 강제 상호작용")]
    /// 디버그를 위한 강제 Interact 발생용 변수
    public bool _testTrigger = false;
    #endif
    
    /// OnInteract 델리게이트 초기화용
    void Awake()
    {
        OnInteract += new OnInteractEvent(
            (GameObject door)=>
            {
                if (!_isOpen)
                    Open(door);
                else
                    Close(door);
            });
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
    }

    /// 문을 여는 함수
    public void Open(GameObject door)
    {
        _isOpen = true;
        Debug.Log("문이 열렸다.");
    }

    /// 문을 닫는 함수
    public void Close(GameObject door)
    {
        _isOpen = false;
        Debug.Log("문이 닫혔다.");
    }

    /// 상호작용(상호작용 가능한 문(_isInteractable)일 경우에 발동)
    public override void OnNotify()
    {
        if (!_isInteractable)
            return;
        
        //Activate();
        base.OnNotify();
    }
    
    /*
    /// 자동으로 상태를 고려하여 문을 열거나 닫는 함수(혹시나를 위해 만든 함수, Switch와 쓰지 마시오.)
    public void Activate()
    {
        base.OnNotify();
    }
    */
}
