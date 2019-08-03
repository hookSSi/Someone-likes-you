using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public string _text;
    public delegate void ControllUI(UI sender, GameObject[] obj);
    public ControllUI _handler;

    public GameObject[] _ui;

    // Start is called before the first frame update
    void Start()
    {
        if(_ui == null)
            _ui[0] = this.GetComponentInChildren(typeof(GameObject)).gameObject;
    }
    public void Interect()
    {
        this._handler(this, this._ui);
        Debug.Log(_text);
    }
}


