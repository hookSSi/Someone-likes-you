using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhoneController : MonoBehaviour
{
    private float _t = 0;
    private Movement _movement;

    public float _loadDistance;
    public float _foldDistance;

    public GameObject _messageUI;
    public TMP_Text _messageText;
    public TMP_Text _messageSender;
    public GameObject _callUI;
    public TMP_Text _callSender;

    private void Awake() 
    {
        _movement = this.gameObject.AddComponent<Movement>();
        _messageText.maxVisibleCharacters = 0;
    }
    public void Called(string sender)
    {
        Debug.Log("전화 옴");
        _callSender.text = sender;

        _callUI.SetActive(true);
        _messageUI.SetActive(false);
    }
    public void Messaged(string sender, string text)
    {
        Debug.Log("메시지 옴");
        _messageText.text = text;
        _messageSender.text = sender;

        _messageUI.SetActive(true);
        _callUI.SetActive(false);

        StartCoroutine(Co_RevealCharacters(_messageText));
    }
    public void Fold(float range)
    {
        _messageUI.SetActive(false);
        _callUI.SetActive(false);

        Debug.Log("휴대폰 닫기");
        //_movement.MoveVertical(this.gameObject, _foldDistance, range);
    }

    public void Load(float range)
    {
        Debug.Log("휴대폰 열기");
        //_movement.MoveVertical(this.gameObject, _loadDistance, range);
    }

    IEnumerator Co_RevealCharacters(TMP_Text textComponent)
    {
        textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = textComponent.textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

        while (true)
        {
            if (visibleCount > totalVisibleCharacters)
            {
                yield break;
            }

            textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            visibleCount += 1;

            yield return null;
        }
    }
}
