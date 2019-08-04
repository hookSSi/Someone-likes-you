using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    public bool isOn = false;

    public Sprite SpriteOn;
    public Sprite SpriteOff;
    private SpriteRenderer renderer;
    
    public GameObject Object;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        
        if (isOn)
            renderer.sprite = SpriteOn;
        else
            renderer.sprite = SpriteOff;
    }

    public void Interact()
    {
        Switching();
    }

    public void Switching()
    {
        if (isOn)
        {
            isOn = false;
            renderer.sprite = SpriteOff;
            Object.SetActive(false);
        }
        else
        {
            isOn = true;
            renderer.sprite = SpriteOn;
            Object.SetActive(true);
        }
    }
}
