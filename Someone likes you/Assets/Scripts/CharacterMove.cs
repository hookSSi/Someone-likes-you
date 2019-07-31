using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float m_MoveSpeed;

    private void Update()
    {
        // 캐릭터 움직임
        // 테스트를 위해 가장 기초적인 기능(좌우 이동)을 구현했습니다.
        // 가능하면 캐릭터 움직임은 새로 코드를 만들어주세요.
        Vector2 position = transform.position;
        position.x += m_MoveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");

        transform.position = position;
    }
}
