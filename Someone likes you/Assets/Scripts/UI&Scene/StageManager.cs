using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public UI _hungernessUI;
    public UI _phoneUI;
    public UI _equipmentUI;

    // Start is called before the first frame update
    void Start()
    {
        _hungernessUI._text = "배고픔 수치";
        _phoneUI._text      = "휴대폰";
        _equipmentUI._text  = "장비";

        _hungernessUI._handler += MakeInvisible;
        _phoneUI._handler      += MakeInvisible;
        _equipmentUI._handler  += MakeInvisible;
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
