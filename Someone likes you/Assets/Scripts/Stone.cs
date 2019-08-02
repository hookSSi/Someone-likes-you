using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StoneTrigger")
        {
            collision.gameObject.GetComponent<StoneTrigger>().act();
        }
    }
}
