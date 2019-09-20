using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBubble : MonoBehaviour
{
     public delegate void TextShowMode(TMP_Text textComponent);
    public TMP_Text _textMesh;
    public bool _isRunning = false;

    public void Awake() 
    {
        if(_textMesh == null)
            _textMesh = this.GetComponent<TMP_Text>();
    }

    public void ShowTextByPage(float waitRange = 0, TextShowMode mode = null)
    {
        if(mode == null)
            StartCoroutine(Co_ShowTextByPage(this._textMesh, waitRange, RevealCharacters));
        else
            StartCoroutine(Co_ShowTextByPage(this._textMesh, waitRange, mode));
    }
    public void RevealCharacters(TMP_Text textComponent)
    {
        StartCoroutine(Co_RevealCharacters(textComponent));
    }
    public void RevealCharacters(string text)
    {
        _textMesh.text = text;
        RevealCharacters(_textMesh);
    }
    public void FadeinShow(float rangeTime)
    {
        StartCoroutine(Co_FadeInShow(this._textMesh, rangeTime));
    }
    public void SetSoftness(float softness)
    {
        _textMesh.font.material.SetFloat("_OutlineSoftness", softness);
    }

    // 페이지 순서대로 보여줌
    IEnumerator Co_ShowTextByPage(TMP_Text textComponent, float waitRange = 0, TextShowMode mode = null)
    {
        this._isRunning = true;

        textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = textComponent.textInfo;

        int totalPage = textInfo.pageInfo.Length;
        int curPage = 1;

        while(true)
        {

            if(curPage > totalPage)
            {
                this._isRunning = false;
                yield break;
            }
            textComponent.pageToDisplay = curPage;
            if(mode != null)
                mode(textComponent);
            yield return new WaitForSeconds(waitRange);
            curPage += 1;  
            yield return null;
        }        
    }

    // 문자열을 점차적으로 보여줌
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

    IEnumerator Co_FadeInShow(TMP_Text textComponent, float rangeTime = 0)
    {
        textComponent.ForceMeshUpdate();

        Material mat = textComponent.font.material;
        float curSoftness = 0;
        float softness = 1;
        float t = 0;

        while(true)
        {
            if(softness <= curSoftness || t > rangeTime)
            {
                softness = curSoftness;
                textComponent.font.material.SetFloat("_OutlineSoftness", softness);
                yield break;
            }

            textComponent.font.material.SetFloat("_OutlineSoftness", softness);

            t += Time.deltaTime;
            softness -= Time.deltaTime / rangeTime;

            yield return null;
        }
    }
}
