using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTool : MonoBehaviour, IInteractable
{
    public string toolName;
    public Tool.ToolType toolType;

    public void Interact()
    {
        ItemDatabase.GetInstance().AddTool(toolName, toolType);
        gameObject.SetActive(false);
    }
}