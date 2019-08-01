using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hungerness : MonoBehaviour
{
    public int _max = 10; // MAX 값
    public int _limit = 20; // 이 값에 도달하면 음식을 먹기 시작
    public float _hungerness = 100.0f; // 계산된 hungerness 값 (100기준 0으로 도달하면 사망)
    private float _timeLaps = 0.0f;

    private void FixedUpdate() 
    {
        this.Thrist();
    }

    // 일정 시간마다 hungerness 수치 줄이기
    public void Thrist()
    {
        if(_hungerness > 0)
        {
            if(_hungerness < _limit)
                this.Eat();

          _hungerness -= (Time.fixedDeltaTime / _max) * 100;
            _timeLaps += Time.fixedDeltaTime;
        }
        else
        {
            Debug.Log("사망");
            Destroy(this);
        }
    }

    public void Eat()
    {
        Debug.Log("음식 먹기");
    }
}
