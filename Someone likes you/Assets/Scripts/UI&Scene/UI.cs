using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public string _text;
    public delegate void ControllVisible(UI sender, GameObject[] obj);
    public ControllVisible _handler;

    public GameObject[] _ui;

    // Start is called before the first frame update
    void Start()
    {
        if(_ui == null)
            _ui[0] = this.GetComponentInChildren(typeof(GameObject)).gameObject;
    }

    private void Update() 
    {
        if(Input.GetMouseButtonUp(0))
        {
            this.Interect();
        }
    }

    public void Interect()
    {
        this._handler(this, this._ui);
        Debug.Log(_text);
        
    }
}


