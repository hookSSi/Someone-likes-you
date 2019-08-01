using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string itemDescription;
    public int gainHungry;
    //public bool isOverlapped;
    public ItemType itemType;
    public Sprite itemImage;

    public enum ItemType
    {
        Key,
        Consumable,
        Food,
        StoryItem
    }

    public Item(string itemName, string itemDescription, int gainHungry, ItemType itemType, Sprite itemImage)
    {

        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.gainHungry = 0;
        if (itemType == ItemType.Food) // 음식 타입이 아닐 경우 gainHungry는 0으로 설정함.
            this.gainHungry = gainHungry;
        this.itemType = itemType;
        this.itemImage = itemImage;
    }
}