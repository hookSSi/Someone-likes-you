using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject, 3.0f);

        if (collision.tag == "StoneTrigger")
        {
            collision.gameObject.GetComponent<StoneTrigger>().act();

            Destroy(gameObject);
        }
        else if(collision.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
