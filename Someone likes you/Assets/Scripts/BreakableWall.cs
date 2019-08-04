using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (ItemDatabase.GetInstance().currentTool.toolEnum == ToolEnum.HAMMER)
        {
            ItemDatabase.GetInstance().AddItem("돌","",0,Item.ItemType.Consumable, "pebble2");
        }
        else
        {

        }
    }
}
