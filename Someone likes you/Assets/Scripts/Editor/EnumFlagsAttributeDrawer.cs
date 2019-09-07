using UnityEngine;
using UnityEditor;

/**
 *  @author 문성후
 *  @date   2019.08.18
 *  @see https://wergia.tistory.com/104
 */
[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        _property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
    }
}