using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTools : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("망치 나가신다!");
        gameObject.SetActive(false);
    }
}
