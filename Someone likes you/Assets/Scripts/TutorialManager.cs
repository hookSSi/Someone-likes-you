using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject _rightUIPos;
    public GameObject _rightMoveUI;

    public GameObject _leftUIPos;
    public GameObject _leftMoveUI;

    public GameObject _upUIPos;
    public GameObject _interectUI;

    public bool _isMoveRight;
    public bool _isMoveLeft;
    public bool _isJump;
    public bool _isInterect;
    public bool _isRunning = false; // 튜토리얼 중인지 체크하는 bool 변수

    private void Start() 
    {
        Clear();
    }

    public void Clear()
    {
        _isMoveRight = false;
        _isMoveLeft  = false;
        _isJump      = false;
        _isInterect  = false;
        _isRunning   = false;
    }
    
    public void ShowRightMoveUI()
    {
        _rightMoveUI.SetActive(true);
        _isRunning = true;
    }
    private bool RightMoveCheck()
    {
        if(Input.GetKeyDown(KeyCode.D) && _rightMoveUI.activeInHierarchy)
        {
            _rightMoveUI.SetActive(false);
            _isRunning = false;
            return true;
        }
        else
        {
            FixPosition(_rightMoveUI, _rightUIPos.transform.position);
        }
        return false;
    }

    public void ShowLeftMoveUI()
    {
        _leftMoveUI.SetActive(true);
        _isRunning = true;
    }
    private bool LeftMoveCheck()
    {
        if(Input.GetKeyDown(KeyCode.A) && _leftMoveUI.activeInHierarchy)
        {
            _leftMoveUI.SetActive(false);
            _isRunning = false;
            return true;
        }
        else
        {
            FixPosition(_leftMoveUI, _leftUIPos.transform.position);
        }
        return false;
    }

    public void ShowInterectUI()
    {
        _interectUI.SetActive(true);
        _isRunning = true;
    }
    private bool InterectCheck()
    {
        if(Input.GetKeyDown(KeyCode.E) && _interectUI.activeInHierarchy)
        {
            _interectUI.SetActive(false);
            _isRunning = false;
            return true;
        }
        else
        {
            FixPosition(_interectUI, _upUIPos.transform.position);
        }
        return false;
    }

    public void Update()
    {
        if(_isRunning)
        {
            _isMoveRight = RightMoveCheck();
            _isMoveLeft  = LeftMoveCheck();
            _isInterect  = InterectCheck();
        }
    }

    public void FixPosition(GameObject obj, Vector3 dest)
    {
        obj.transform.position = dest;
    }
}
