using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image _sprite; // 페이드 인, 아웃에 사용할 UI Sprite
    
    private void Update() {
        if(Input.GetKeyUp(KeyCode.S)) FadeIn(3);
        if(Input.GetKeyUp(KeyCode.A)) FadeOut(3);
    }

    public void FadeIn(float fadeTime, System.Action nextEvent = null)
    {
        StartCoroutine(CoFadeIn(fadeTime, nextEvent));
    }
    public void FadeOut(float fadeTime, System.Action nextEvent = null)
    {
        StartCoroutine(CoFadeOut(fadeTime, nextEvent));
    }

    IEnumerator CoFadeIn(float fadeTime, System.Action nextEvent = null)
    {
        Color tempColor = _sprite.color;
        while(tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeTime;
            _sprite.color = tempColor;

            yield return null;
        }

        if(nextEvent != null) nextEvent();
    }

    IEnumerator CoFadeOut(float fadeTime, System.Action nextEvent = null)
    {
        Color tempColor = _sprite.color;
        while(tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeTime;
            _sprite.color = tempColor;

            yield return null;
        }

        if(nextEvent != null) nextEvent();
    }

}
