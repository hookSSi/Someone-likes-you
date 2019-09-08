using UnityEngine;
using UnityEditor;

public static class EditorPlatformList
{
    public static void Show (SerializedProperty list, EditorListOption options = EditorListOption.Defalut)
    {
        if (!list.isArray)
        {
            EditorGUILayout.HelpBox(list.name + "는 배열이나 리스트가 아니므로 Inspector에 표시할 수 없습니다. EditorGUILayout.PropertyField 함수의 사용을 추천합니다!", MessageType.Error);
            return;
        }

        bool
            showListLabel = (options & EditorListOption.ListLabel) != 0,
            showListSize = (options & EditorListOption.ListSize) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel++;
        }
        if (!showListLabel || list.isExpanded)
        {
            SerializedProperty size = list.FindPropertyRelative("Array.size");
            if (showListSize)
            {
                EditorGUILayout.PropertyField(size);
            }
            if (size.hasMultipleDifferentValues)
            {
                EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
            }
            else
            {
                ShowElements(list, options);
            }
        }
        if (showListLabel)
        {
        EditorGUI.indentLevel--;
        }
    }

    private static GUIContent
        MoveButtonContent = new GUIContent("\u21b4", "move down"),
        duplicateButtonContent = new GUIContent("+", "duplicate"),
        deleteButtonContent = new GUIContent("-", "delete"),
        AddButtonContent = new GUIContent("+", "add element");        
    private static void ShowElements (SerializedProperty list, EditorListOption options)
    {
        bool
            showElementLabels = (options & EditorListOption.ElementLabels) != 0,
            showButtons = (options & EditorListOption.Buttons) != 0;

        for (int i = 0; i < list.arraySize; i++)
        {
            if (showButtons)
            {
                EditorGUILayout.BeginHorizontal();
            }
            if (showElementLabels)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            else 
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
            }
            if (showButtons)
            {
                ShowButtons(list, i);
                EditorGUILayout.EndHorizontal();
            }
        }
        if (showButtons && list.arraySize == 0 && GUILayout.Button(AddButtonContent, EditorStyles.miniButton))
        {
            list.arraySize += 1;
        }
    }
    
    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(MoveButtonContent, EditorStyles.miniButtonLeft))
        {
            list.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid))
        {
            list.InsertArrayElementAtIndex(index);
        }
        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight))
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }
    }
}
  
