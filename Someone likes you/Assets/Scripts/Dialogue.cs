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

        TMP_Text dialogue = gameObject.GetComponent<RectTransform>().Find("Text").GetComponent<TMP_Text>();

        dialogue.text = "";

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
