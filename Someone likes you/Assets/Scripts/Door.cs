using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Sprite spriteClosed;
    public Sprite spriteOpened;

    private SpriteRenderer spriteRenderer;
    // private bool isOpen;
    public bool isOpenByKey;

    private void Start()
    {
        // isOpen = false;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteClosed;
    }

    public void Open()
    {
        spriteRenderer.sprite = spriteOpened;
        // isOpen = true;
        gameObject.SetActive(false);
    }
    public void Close()
    {
        spriteRenderer.sprite = spriteClosed;
        // isOpen = false;
        gameObject.SetActive(true);
    }

    public void Interact()
    {
        if(isOpenByKey == true)
        {
            // 오브젝트 찾기
            List<Item> items = ItemDatabase.GetInstance().items;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemType == Item.ItemType.Key)
                {
                    items.RemoveAt(i);
                    Open();
                    return;
                }
            }
        }
        else
            Debug.Log("열 수 없는 문이다.");
    }
}
