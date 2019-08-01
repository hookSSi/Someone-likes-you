using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 문제점 : 열쇠의 종류가 다양해질 경우, 하나의 키로 모든 종류의 문을 모두 열 수 있다는 것은 아주 큰 맹점이 된다.
// 문마다 짝을 이루는 열쇠의 이름을 정하고, 열쇠 이름을 함께 체크해보는 방법을 고려하는 중임.

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
