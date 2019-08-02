using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public delegate void Action(GameObject obj, float value, float time = 1, bool moveType = true);
    public bool _isDone = false;
    private float _t = 0;

    public float _waitRange; // 기다리는 시간
    public float _actRange; // 얼마나 작동 할건지
    public float _distance;

    private void Start() 
    {
        StartCoroutine(Wait(_waitRange, Movement.MoveHorizontal));
    }
    IEnumerator Wait(float range, Action action)
    {
        // 시간이 다되지 않거나 플레이어 일이 다 마치지 않았다면
        // 계속 기다린다.
        while(range > this._t || !_isDone)
        {
            this._t  += Time.deltaTime;
            yield return null;
        }
        
        if(action != null && _isDone)
        {
            this._t = 0;
            StartCoroutine(Act(_actRange, action));
        }
    }

    IEnumerator Act(float range, Action action)
    {
        while(range > this._t)
        {
            this._t += Time.deltaTime;
            action(this.gameObject, _distance, range);
            yield return null;
        }

        if(_isDone)
            Clear();
    }

    public void Clear()
    {
        this._isDone = false;
        this._t = 0;
    }
    public void Done()
    {
        this._isDone = true;
    }
}
