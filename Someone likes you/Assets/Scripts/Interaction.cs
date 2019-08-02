﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 캐릭터에 부착될 컴포넌트
// 상호작용 : Interactable 태그 오브젝트만

public class Interaction : MonoBehaviour
{
    public Collider2D[] objCollider = new Collider2D[10]; // 상호작용 가능한 오브젝트를 등록하는 배열(자동등록)
    public Collider2D objNearest = null;
    public Image indicator;
    double distanceObjNearestSqr = 0;

    public Camera camera;

    private void Start()
    {


        //Debug.Log("테스트");
    }
    private void Update()
    {
        checkNearestObject();

        // objNearest.gameObject에 Shader 적용(방법을 모르겠음)

        DrawInteractable();

        if (Input.GetKeyDown(KeyCode.E))
        {
            //checkNearestObject();
            // 가장 가까운 상호작용 오브젝트와 상호작용
            if (objNearest != null)
            {
                IInteractable objInteractable = objNearest.GetComponent<IInteractable>();
                objInteractable.Interact();
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            throwStone();
        }
    }

    //상호작용(E)관련
    private void checkNearestObject() // 가장 가까운 상호작용 오브젝트 찾기(거리 짧은 것을 objNearest로 함)
    {
        objNearest = null;
        for (int i = 0; i < objCollider.Length; i++)
        {
            if (objCollider[i] == null)
                continue;
            if (objNearest == null)
            {
                objNearest = objCollider[i];
                distanceObjNearestSqr = ((Vector2)(objNearest.transform.position - this.transform.position)).sqrMagnitude;
                continue;
            }

            double distanceObjSqr = ((Vector2)(objCollider[i].transform.position - this.transform.position)).sqrMagnitude;
            if (distanceObjNearestSqr > distanceObjSqr)
            {
                objNearest = objCollider[i];
                distanceObjNearestSqr = distanceObjSqr;
            }
        }
    }

    private void DrawInteractable()
    {
        
    }

    // 상호작용 가능한 오브젝트의 콜라이더를 등록한다
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Interactable")
        {
            for (int i = 0; i < objCollider.Length; i++)
            {
                if (objCollider[i] != null)
                    continue;
                objCollider[i] = other;
                break;
            }
        }
    }

    // 상호작용 가능한 오브젝트의 콜라이더를 삭제한다
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Interactable")
        {
            for (int i = 0; i < objCollider.Length; i++)
            {
                if (objCollider[i] != other)
                    continue;
                objCollider[i] = null;
                break;
            }
        }
    }


    //돌 던지기관련
    private void throwStone()
    {
        Item stoneItem = ItemDatabase.GetInstance().items.Find(x => x.itemName.Equals("돌"));

        if (stoneItem == null)
        {
            return;
        }

        ItemDatabase.GetInstance().items.Remove(stoneItem);

        Debug.Log("돌 던짐");

        Vector3 mousePos = camera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        mousePos.z = 0;

        GameObject temp = GameObject.Instantiate(Resources.Load("Prefabs/Stone") as GameObject, transform.position, Quaternion.identity);

        Vector3 dir = mousePos - transform.position;

        dir.z = 0;

        temp.GetComponent<Rigidbody2D>().AddForce(dir * 200);
    }

}
