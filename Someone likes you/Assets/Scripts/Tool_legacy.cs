using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolEnum { HAND, HAMMER }

[System.Serializable]
public class Tool
{
    public string name;
    public Sprite toolImage;
    public ToolEnum toolEnum;


    public Tool(string name, Sprite toolImage, ToolEnum toolEnum)
    {
        this.name = name;
        this.toolImage = toolImage;
        this.toolEnum = toolEnum;
    }
}
