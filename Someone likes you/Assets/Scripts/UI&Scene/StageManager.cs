using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    // 페이드 인, 아웃 컨트롤러
    public FadeController _fadeController;
    // 휴대폰 움직임을 제어하는 컨트롤러
    public PhoneController _phoneController;
    // 트럭의 움직임을 제어하는 컨트롤러
    public TruckController _truckController;
    // 플레이어의 움직임을 제어하는 컨트롤러
    public PlayerMovement _playerMovement;
    private TutorialManager _tutorialController;
    public GameObject[] _components;
    public UITextScript[] _UITexts;


    private float _curTime = 0;
    private float _prevTime = 0;
    private bool _isDone = false;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "SomeoneLikesYou")
        {
            StartCoroutine(SomeoneLikesYou());
        }
        if(SceneManager.GetActiveScene().name == "Intro")
        {
            StartCoroutine(Intro());
        }
    }

    public IEnumerator SomeoneLikesYou()
    {
        /* 0: Producer 오브젝트 */
        /* 1: Subject  오브젝트  */

        GameObject producer = _components[0];
        UITextScript producer_script = _UITexts[0];

        GameObject subject = _components[1];
        UITextScript subject_script = _UITexts[1];

        producer.SetActive(true);
        producer_script.Awake();
        
        producer_script.ShowTextByPage(2f);
        while(producer_script._isRunning)
        {
            yield return null;
        }
        if(!producer_script._isRunning)
        {
            yield return new WaitForSeconds(2f);
            producer.SetActive(false);
            subject.SetActive(true);
            subject_script.Awake();
            subject_script.SetSoftness(1f);
        }
        subject_script.FadeinShow(5f);
        yield return new WaitForSeconds(7f);
        this.NextScene("Intro");
    }
    public IEnumerator Intro()
    {
        // 어두웠던 화면이 페이드 아웃으로 밝아짐
        _fadeController.FadeIn(0.0001f);
        yield return new WaitForSeconds(2f);
        _fadeController.FadeOut(2f);

        // 움직임 튜토리얼
        GameObject tutorialObj = _components[0];
        tutorialObj.SetActive(true);
        _tutorialController = tutorialObj.GetComponent<TutorialManager>();
        
        yield return new WaitForSeconds(2f);
        _tutorialController.Clear(); // 초기화
        // Right Move tutorial
       
        _tutorialController.ShowRightMoveUI();
        while(_tutorialController._isRunning){ yield return null;}
        // Left Move tutorial
        _tutorialController.ShowLeftMoveUI();
        while(_tutorialController._isRunning){ yield return null;}
        // Interect tutorial
        _tutorialController.ShowInterectUI();
        while(_tutorialController._isRunning){ yield return null;}

        Debug.Log("튜토리얼 완료");
        this.NextScene("1-1");
        // 잠시후 전화가 옴
        //yield return new WaitForSeconds(3f);
        //_phoneController.Load(3); yield return new WaitForSeconds(3f);
        //_phoneController.Called("힐다");
        //     yield return new WaitForSeconds(5f);
        // _phoneController.Fold(1); yield return new WaitForSeconds(3f);
        // _phoneController.Load(3); yield return new WaitForSeconds(3f);
        // _phoneController.Messaged("브라이언","주저리 주저리 동창회에 올거야? 난 말이지");
        //     yield return new WaitForSeconds(5f);
        // _phoneController.Fold(1);
        // // 이동 튜토리얼 시작
        // // 이동 튜토리얼이 끝났다 싶으면 트럭이 플레이어 쪽으로 옴
        // yield return new WaitForSeconds(5f);
        // _truckController.Move();
        // yield return new WaitForSeconds(5f);
        // // 플레이어가 트럭에 타면 트럭이 출발
        // _truckController.Done();
        yield return null;
    }
    public void NextScene(string sceneName)
    {
        _fadeController.FadeIn(0.0001f);
        SceneManager.LoadScene(sceneName);
    }

    private void DisableObjects(UI sender, GameObject[] obj)
    {
        foreach (GameObject item in obj)
        {
            item.SetActive(false);
        }
    }
    private void EnableObjects(UI sender, GameObject[] obj)
    {
        foreach (GameObject item in obj)
        {
            item.SetActive(true);
        }
    }
}
