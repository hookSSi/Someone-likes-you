using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTools : MonoBehaviour, IInteractable
{
    public string toolName;
    public Tool.ToolType toolType;

    public void Interact()
    {
        ItemDatabase.GetInstance().AddTools(toolName, toolType);
        gameObject.SetActive(false);
    }
}