using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch1_1 : MonoBehaviour, IInteractable
{
    public bool isOn = false;

    public Sprite SpriteOn;
    public Sprite SpriteOff;
    private SpriteRenderer renderer;

    private bool first = true;

    public GameObject Object;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();

        if (isOn)
        {
            Object.SetActive(true);
            renderer.sprite = SpriteOn;
        }
        else
        {
            Object.SetActive(false);
            renderer.sprite = SpriteOff;
        }
    }

    public void Interact()
    {
        GetComponent<AudioSource>().Play();

        Switching();

        if (first)
        {
            Debug.Log("첫번째 스위치");

            Conversation.GetInstance().startConversation("Assets/Resources/DialogueText/1_1switch.txt");

            first = false;
        }
    }

    public void Switching()
    {
        if (isOn)
        {
            isOn = false;
            renderer.sprite = SpriteOff;
            Object.SetActive(false);
        }
        else
        {
            isOn = true;
            renderer.sprite = SpriteOn;
            Object.SetActive(true);
        }
    }
}