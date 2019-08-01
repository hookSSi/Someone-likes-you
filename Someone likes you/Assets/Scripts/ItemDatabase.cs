//#define TEST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    // 아이템들
    private static ItemDatabase instance;
    public List<Item> items = new List<Item>(); // 인벤토리
    public List<Tool> tools = new List<Tool>(); // 도구 리스트
    public int currentTool = 0;


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

    }
}
