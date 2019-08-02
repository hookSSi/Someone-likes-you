//#define TEST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDatabase : MonoBehaviour
{
    // 아이템들
    private static ItemDatabase instance;
    public List<Item> items = new List<Item>(); // 인벤토리
    public List<Tool> tools = new List<Tool>(); // 도구 리스트
    public int currentTool = 0;

    public GameObject inventory1; // 도구와 먹을 것
    public GameObject inventory2; // 그 외

    public static ItemDatabase GetInstance() // 싱글톤 패턴
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

    public void AddItem(string itemName, string itemDescription, int gainHungry, Item.ItemType itemType)
    {
        Debug.Log(itemName + "를 획득했다!(아이템)");
        items.Add(new Item(itemName, itemDescription, gainHungry, itemType, Resources.Load<Sprite>("ItemImages/" + itemName)));

        drawInventory();
    }

    public void AddTool(string toolName, Tool.ToolType toolType)
    {
        Debug.Log(toolName + "를 주웠다!(도구)");
        tools.Add(new Tool(toolName, toolType, Resources.Load<Sprite>("ItemImages/" + toolName)));
    }
    
    /*
    public Tool CurrentTool()
    {
        if (tools.Count == 1) return tools[0];
    }
    */

    private void Start()
    {
#if TEST
        AddItem("열쇠", "문을 열 수 있다. 맛은... 있을리가.", 0, Item.ItemType.Key);
        AddItem("초코바", "맛있다. 군인의 영원한 친구.", 50, Item.ItemType.Food);
#endif
        AddTool("맨손", Tool.ToolType.Melee);
    }

    //인벤토리 시각화

    void drawInventory()
    {
        int canNum = 0;
        int chocoNum = 0;



        //inventory2 모든 자식 제거
        Transform[] childList = inventory2.transform.GetComponentsInChildren<RectTransform>();

        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                if (childList[i] != transform && !childList[i].name.Contains("Inventory"))
                {                       
                    Destroy(childList[i].gameObject);
                }
            }
        }
        //

        float X = 200f;

        float Y = 75f;

        GameObject temp;

        Sprite tempSprite;

        foreach (Item i in items)
        {
            if (i.itemType == Item.ItemType.Food)
            {
                if (i.itemName.Equals("초코바"))
                {
                    chocoNum++;
                }
                else if (i.itemName.Equals("음료수"))
                {
                    canNum++;
                }

                continue;
            }

            temp = GameObject.Instantiate(Resources.Load("Prefabs/Item") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);

            temp.transform.SetParent( inventory1.transform);

            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(X, Y);

            temp.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);

            //tempSprite = Resources.Load<Sprite>("InventoryIconRaw/Can");

            tempSprite = i.itemImage;

            temp.GetComponent<Image>().overrideSprite = tempSprite;

            X += 50f;
        }

        inventory1.transform.Find("Can").transform.Find("CanNum").GetComponent<Text>().text = canNum.ToString();
        inventory1.transform.Find("ChocoBar").transform.Find("ChocoNum").GetComponent<Text>().text = chocoNum.ToString();
    }
}
