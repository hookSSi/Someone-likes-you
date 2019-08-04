using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitch : MonoBehaviour, IInteractable
{
    public Sprite SpriteOff;
    public Sprite SpriteOn;
    private SpriteRenderer renderer;
    
    public GameObject Object;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = SpriteOff;
    }

    public void Interact()
    {
        if (renderer.sprite == SpriteOff)
        {
            renderer.sprite = SpriteOn;
            Object.SetActive(true);
        }
        else
        {
            renderer.sprite = SpriteOff;
            Object.SetActive(false);
        }
    }
}
