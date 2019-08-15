using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTool : MonoBehaviour, IInteractable
{
    public string toolName;

    public Tool_legacy tool;

    public void Interact()
    {
        ItemDatabase.GetInstance().AddTool(tool);
        gameObject.SetActive(false);
    }
}