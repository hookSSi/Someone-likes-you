using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/*
사용법
1. 이 스크립트를 가지는 게임 오브젝트를 만든다.
2. 사용할 대화가 표시될 Canvas와 Main Camera를 붙인다.
3. startConversation("대화가 있는 txt 파일의 경로")를 호출한다.
대화 txt 파일 작성법
대사를 말할 오브젝트명 대사
즉
오브젝트명'\t'대사
예시) Assets\Resources\DialogueText\TextDT.txt
주의!
1. 오브젝트명이 대사 txt 파일에 없으면 오류남!
2. UTF-8로 인코딩 할 것!
*/

public class Conversation : MonoBehaviour
{
    private static Conversation instance;

    public Canvas canvas;

    public Camera camera;

    /*
    public string text;

    private void Awake() {
        startConversation(text);
    }
    */

    public static Conversation GetInstance() // 싱글톤 패턴
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<Conversation>();
            if (instance == null)
            {

                instance = new GameObject().AddComponent<Conversation>();
                instance.gameObject.name = "ConversationManager";
            }
        }
        return instance;
    }

    public void startConversation(string Path)
    {
        StartCoroutine(conversation(loadConversation(Path)));
    }

    private Vector2 worldToCanvas(Vector3 input)
    {
        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

        Vector2 ViewportPosition = camera.WorldToViewportPoint(input);

        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        return WorldObject_ScreenPosition;
    }

    private IEnumerator conversation(List<character_line> lines)
    {
        foreach (character_line i in lines)
        {
            Vector2 canvasPos = worldToCanvas(i.character.transform.position);

            GameObject temp = GameObject.Instantiate(Resources.Load("Prefabs/WordBubble") as GameObject, transform.position, Quaternion.identity);

            //temp.GetComponent<RectTransform>().SetParent(canvas.transform);

            temp.transform.SetParent(canvas.transform);

            temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            temp.GetComponent<RectTransform>().anchoredPosition = canvasPos;

            temp.transform.localPosition = new Vector3(canvasPos.x, canvasPos.y + 120, 0);

            yield return StartCoroutine(temp.GetComponent<Dialogue>().dialogue(i.line));

            Destroy(temp);
        }

        yield return null;
    }

    private List<character_line> loadConversation(string path)
    {
        List<character_line> output = new List<character_line>();

        string[] lines = File.ReadAllLines(path);

        foreach (string i in lines)
        {
            string[] result = i.Split(new char[] { '\t' });

            output.Add(new character_line(GameObject.Find(result[0]), result[1]));
        }

        return output;
    }
}

public class character_line
{
    public GameObject character;
    public string line;

    public character_line(GameObject character, string line)
    {
        this.character = character;
        this.line = line;
    }
}