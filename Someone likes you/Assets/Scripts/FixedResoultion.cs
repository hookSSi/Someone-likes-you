using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *      @brief 카메라를 16:9 종회비로 맞추는 클래스
 *      @detail 다른 종회비는 귀찮아서 구현하지 않을 예정
 */
public class FixedResoultion : MonoBehaviour
{
    /**
     *      @brief ResolutionFix 실행
     *      @detail GameObject의 컴포넌트가 아니면 실행되지 않음
     *      @return void
     */
    void Start()
    {
        ResolutionFix();
    }

    /**
     *      @brief 카메라를 16:9 종회비로 맞추는 함수
     *      @return void
     */
    private void ResolutionFix()
    {
        // 가로 세로 비율
        float targetWidthAspect = 16.0f;
        float targetHeightAspect = 9.0f;
    
        Camera.main.aspect = targetWidthAspect / targetHeightAspect;
    
        float widthRatio = (float)Screen.width / targetWidthAspect;
        float heightRatio = (float)Screen.height / targetHeightAspect;
    
        float heightadd = ((widthRatio / (heightRatio / 100)) - 100) / 200;
        float widthadd = ((heightRatio / (widthRatio / 100)) - 100) / 200;
    
        // 시작지점을 0으로 만들어준다.
        if (heightRatio > widthRatio)
            widthRatio = 0.0f;
        else
            heightRatio = 0.0f;
    
        Camera.main.rect = new Rect(
            Camera.main.rect.x + Mathf.Abs(widthadd),
            Camera.main.rect.x + Mathf.Abs(heightadd),
            Camera.main.rect.width + (widthadd * 2),
            Camera.main.rect.height + (heightadd * 2));
    }
}
