using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public bool killAction = false;

    public GameObject wordBubble;

    public GameObject dialogText;

    void Start()
    {
        StartCoroutine(NonStopTalk("테스트 테스트 \n Test Test"));
    }

    void Update()
    {
        
    }

    private IEnumerator NonStopTalk(string text)
    {
        TextMesh dialogue = dialogText.GetComponent<TextMesh>();

        dialogue.text = "";
        if (!wordBubble.activeSelf)
        {
            wordBubble.SetActive(true);
        }

        foreach (char letter in text.ToCharArray())
        {
            dialogue.text += letter;

            yield return new WaitForSeconds(0.05f);

            if (killAction)
            {
                dialogue.text = text;

                break;
            }
        }

        killAction = false;

        yield return new WaitForSeconds(0.2f);
    }
}
