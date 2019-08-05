using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    void Start()
    {
        Conversation.GetInstance().startConversation("Assets/Resources/DialogueText/test.txt");
    }
}
