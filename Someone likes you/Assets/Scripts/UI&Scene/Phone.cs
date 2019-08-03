using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public float _loadDistance;
    public float _foldDistance;
    public bool _isLoad;
    public bool _isFold;
    private float _t = 0;
    
    public void Fold(float range)
    {
        Debug.Log("휴대폰 닫기");
        StartCoroutine(Co_FoldPhone(range));
    }

    public void Load(float range)
    {
        Debug.Log("휴대폰 열기");
        StartCoroutine(CO_LoadPhone(range));
    }

    public IEnumerator Co_FoldPhone(float range)
    {
        while(_t > range)
        {
            this._t += Time.deltaTime;
            Movement.MoveVertical(this.gameObject, _foldDistance, range);
            yield return null;
        }

        _t = 0;
    }
    public IEnumerator CO_LoadPhone(float range)
    {
        while(_t > range)
        {
            this._t += Time.deltaTime;
            Movement.MoveVertical(this.gameObject, _loadDistance, range);
            yield return null;
        }

        _t = 0;
    }
}
