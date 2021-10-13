using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
class LevelManagerEditor : Editor
{

    public static LevelGenerator loadedLevelGenerator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}