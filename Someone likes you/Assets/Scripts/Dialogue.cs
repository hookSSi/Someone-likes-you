using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Dialogue : MonoBehaviour
{
    public enum DialogueStatus {END, TYPING, WAIT}

    public DialogueStatus status = DialogueStatus.END;

    public GameObject wordBubble;

    public GameObject dialogueText;

    void Start()
    {
        StartCoroutine(dialogue("테스트 테스트 \n Test Test \n 테테테테테스트트트트트"));
    }

    void Update()
    {
       
    }
    private IEnumerator dialogue(string text)
    {
        status = DialogueStatus.TYPING;

        StartCoroutine(keyDownCheck());

        TextMesh dialogue = dialogueText.GetComponent<TextMesh>();

        dialogue.text = "";

        if (!wordBubble.activeSelf)
        {
            wordBubble.SetActive(true);
        }

        foreach (char letter in text.ToCharArray())
        {
            dialogue.text += letter;

            yield return new WaitForSeconds(0.05f);

            if (status == DialogueStatus.WAIT)
            {
                dialogue.text = text;

                break;
            }
        }

        status = DialogueStatus.WAIT;

        while (status == DialogueStatus.WAIT)
        {
            yield return null;
        }

        wordBubble.SetActive(false);

        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator keyDownCheck()
    {
        while (status == DialogueStatus.TYPING)
        {
            if (Input.anyKeyDown)
            {
                status = DialogueStatus.WAIT;

                break;
            }

            yield return null;
        }

        Debug.Log("TYPING -> WAIT");

        while (Input.anyKeyDown)
        {
            yield return null;
        }

        while (status == DialogueStatus.WAIT)
        {
            if (Input.anyKeyDown)
            {
                status = DialogueStatus.END;

                break;
            }

            yield return null;
        }

        Debug.Log("WAIT -> END");

        yield return null;
    }
}