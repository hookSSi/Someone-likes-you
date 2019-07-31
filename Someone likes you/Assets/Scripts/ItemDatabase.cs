using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    // 아이템들
    private static ItemDatabase instance;
    public List<Item> items = new List<Item>();

    // 도구 리스트 구현 예정
    
    public static ItemDatabase GetInstance() // 싱글톤 패턴, 데이터베이스가 많아질 수 있으니 삭제/수정 가능성 있음.
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<ItemDatabase>();
            if (instance == null)
            {

                instance = new GameObject().AddComponent<ItemDatabase>();
                instance.gameObject.name = "Database";
            }
        }
        return instance;
    }

    public void Add(string itemName, string itemDescription, int gainHungry, Item.ItemType itemType)
    {
        Debug.Log(itemName + "를 획득했다!");
        items.Add(new Item(itemName, itemDescription, gainHungry, itemType, Resources.Load<Sprite>("ItemImages/" + itemName)));
    }
    
    private void Start()
    {
        
        Add("열쇠", "문을 열 수 있다. 맛은... 있을리가.", 0, Item.ItemType.Key);
        Add("초코바", "맛있다. 군인의 영원한 친구.", 50, Item.ItemType.Food);
        //*/
    }
    
}
