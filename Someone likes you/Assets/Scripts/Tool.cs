using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tool
{
    public string toolName;
    // public int toolDamage; 데미지 형식을 넣을 것인가?
    public ToolType toolType;
    public Sprite toolImage;

    public enum ToolType
    {
        Melee,
        Throwable,
        Event // 특정 이벤트에 이용되는 툴?? 예를 들어 마지막 보스를 죽이는 무기는 들고 있는 것으로 무언가 이벤트가 실행된다던가...
    }

    public Tool(string toolName, ToolType toolType, Sprite toolImage)
    {
        this.toolName = toolName;
        this.toolType = toolType;
        this.toolImage = toolImage;
    }
}
