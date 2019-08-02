using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTrigger : MonoBehaviour
{
    public GameObject door;

    public void act()
    {
        if (door.activeSelf == true)
        {
            door.SetActive(false);
        }
        else
        {
            door.SetActive(true);
        }
    }
}
