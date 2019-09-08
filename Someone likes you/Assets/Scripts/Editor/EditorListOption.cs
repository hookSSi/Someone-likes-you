using UnityEngine;
using System;

[Flags]
public enum EditorListOption
{
    None = 0,
    ListSize = 1 << 0,
    ListLabel = 1 << 2,
    ElementLabels = 1 << 3,
    Buttons = 1 << 4,
    Defalut = ListSize | ListLabel | ElementLabels,
    NoElementLabels = ListSize | ListLabel,
    All = Defalut | Buttons
}
