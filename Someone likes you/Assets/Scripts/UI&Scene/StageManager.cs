using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public FadeController _fadeController;
    public PlayerMovement _playerMovement;
    public UI _hungernessUI;
    public UI _phoneUI;
    public UI _equipmentUI;


    private float _t = 0;
    private bool _isDone = false;

    // Start is called before the first frame update
    void Start()
    {
        _hungernessUI._text = "배고픔 수치";
        _phoneUI._text      = "휴대폰";
        _equipmentUI._text  = "장비";

        _hungernessUI._handler += MakeInvisible;
        _phoneUI._handler      += MakeInvisible;
        _equipmentUI._handler  += MakeInvisible;

        StartCoroutine(Intro());
    }

    public IEnumerator Intro()
    {
        _fadeController.FadeIn(0.0001f);
        yield return new WaitForSeconds(2f);
        _fadeController.FadeOut(2f);
        
    }

    private void MakeInvisible(UI sender, GameObject[] obj)
    {
        foreach (GameObject item in obj)
        {
            item.SetActive(false);
        }

        sender._handler -= MakeInvisible;
        sender._handler += MakeVisible;
    }
    private void MakeVisible(UI sender, GameObject[] obj)
    {
        foreach (GameObject item in obj)
        {
            item.SetActive(true);
        }

        sender._handler -= MakeVisible;
        sender._handler += MakeInvisible;
    }
}
