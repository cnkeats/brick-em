using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConsumableItem), true)]
class ConsumableItemEditor : Editor
{
    ConsumableItem consumableItem;

    public override void OnInspectorGUI()
    {
        consumableItem = target as ConsumableItem;

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        using (new EditorGUI.DisabledScope(!Application.isPlaying))
        {
            if (GUILayout.Button("Consume", GUILayout.Width(150), GUILayout.Height(25)))
            {
                consumableItem.Consume();
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(20);


        base.OnInspectorGUI();

    }
}