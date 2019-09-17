using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief
 *  플레이어 HandInput()을 위한 중간 매개층 Command
 *  이걸 따로 만듬으로써 키변경의 유연함을 가질 수 있게됨
 *  @details
 *  어차피 게임에서 쓰일 일 없으니까 ScriptableObject를 상속받아
 *  코드에서 객체를 유지할 수 있게 만듬
 *  @see https://blog.naver.com/hammerimpact/220770261760
 *  @author 문성후
 *  @date   2019.08.17
 */
public class Command : ScriptableObject
{
    public delegate void KeyDownEvent();
    /// 키입력시 발생할 이벤트
    public event KeyDownEvent _event;
    /// 어떤 키로 설정된 Command인지
    public string _name;
    private KeyCode _key {get; set;}
    /// 생성자
    public Command Init(KeyCode key, KeyDownEvent e, string name = "무제")
    {
        this._key = key;
        this._event += e;
        this._name = name;

        return this;
    }
    /// 복사생성자
    public Command Init(Command other)
    {
        this._key = other._key;
        this._event = other._event;

        return this;
    }
    /// Set Event
    public void SetEvent(KeyDownEvent e){_event = e;}
    /**
     * @brief
     * 키 입력 체크하는 함수
     */
    public bool CheckGetKeyDown()
    {   
        if(Input.GetKeyDown(_key))
        {
            _event();
            return true;
        }
        return false;
    }

    public bool CheckGetKey()
    {
        if(Input.GetKey(_key))
        {
            _event();
            return true;
        }
        return false;
    }
}
