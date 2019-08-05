using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private enum DialogueStatus { NULL, END, TYPING, WAIT }

    private DialogueStatus status = DialogueStatus.NULL;

    public void Dialog(string text)
    {
        StartCoroutine(dialogue(text));
    }

    public IEnumerator dialogue(string text)
    {
        status = DialogueStatus.TYPING;

        StartCoroutine(keyDownCheck());

        TMP_Text dialogue = gameObject.transform.GetChild(0).Find("Text").GetComponent<TMP_Text>();

        dialogue.text = text;

        dialogue.ForceMeshUpdate();

        TMP_TextInfo textInfo = dialogue.textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object

        

        Debug.Log(totalVisibleCharacters);

        int visibleCount = 0;

        while(true)
        {
            Debug.Log(visibleCount);
            
            if (visibleCount > totalVisibleCharacters)
            {
                break;
            }
            
            dialogue.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
            visibleCount += 1;
            yield return null;
        }

        Debug.Log("end");

        status = DialogueStatus.WAIT;

        while (status == DialogueStatus.WAIT)
        {
            yield return null;
        }

        yield return null;
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

        yield return null;
    }

}
