using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformInspector : Editor
{
    public override void OnInspectorGUI()
    {
        MovingPlatform _script = (MovingPlatform)target;
        PlatformCatcher _platform = _script._platform;

        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_platform"));
        EditorPlatformList.Show(serializedObject.FindProperty("_checkPointList"), EditorListOption.All);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_moveMode"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_velocity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_waitTime"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_timeForStop"));
        serializedObject.ApplyModifiedProperties();

        // MovingPlatform의 _platform이 비어있을 경우 경고문을 띄운다.
        if (_platform == null)
        {
            EditorGUILayout.HelpBox("해당 오브젝트의 자식으로 Platform 오브젝트가 존재하지 않거나, " +
            "Platform 오브젝트에서 PlatformCatcher 스크립트를 찾을 수 없습니다.", MessageType.Error);
        }
        // MovingPlatform의 Layer가 Ground가 권고문을 띄운다.
        if (_script.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            EditorGUILayout.HelpBox("Moving Platform의 LayerMask가 Ground가 아닙니다.", MessageType.Warning);
        }
    }
}