using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGainable : MonoBehaviour, IInteractable
{
    public string itemName;
    public string itemDescription;
    public int gainHungry;
    public Item.ItemType itemType;


    public void Interact()
    {
        // Debug.Log(name + "다!");

        ItemDatabase.GetInstance().Add(itemName, itemDescription, gainHungry, itemType);

        gameObject.SetActive(false);
    }
}
