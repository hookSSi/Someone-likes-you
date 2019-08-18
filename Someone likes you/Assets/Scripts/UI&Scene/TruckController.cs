using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    public delegate void Action(GameObject obj, float value, float time = 1, bool moveType = true);
    public bool _isDone = false;
    private float _t = 0;

    public float _waitRange; // 기다리는 시간
    public float _distance;
    private Movement _movement;

    private void Awake() 
    {
        _movement = this.gameObject.AddComponent<Movement>();
    }

    public void Move()
    {
        //_movement.MoveHorizontal(this.gameObject, _distance, _waitRange);
    }
    public void Clear()
    {
        this._isDone = false;
        this._t = 0;
    }
    public void Done()
    {
        Debug.Log("트럭 출발!");
        this._isDone = true;
    }
}
