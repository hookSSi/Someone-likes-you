using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitch : MonoBehaviour, IInteractable
{
    public Sprite SpriteDeactivative;
    public Sprite SpriteActivative;
    private SpriteRenderer spriteRenderer;
    public Door door;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpriteDeactivative;
    }

    public void Interact()
    {
        if (spriteRenderer.sprite == SpriteDeactivative)
        {
            spriteRenderer.sprite = SpriteActivative;
            door.Open();
        }
        else
        {
            spriteRenderer.sprite = SpriteDeactivative;
            door.Close();
        }
    }
}
