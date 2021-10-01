using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelGenerator levelGenerator;

    public void LoadLevelFromGenerator(LevelGenerator levelGenerator)
    {
        this.levelGenerator = levelGenerator;
        this.levelGenerator.bounds = GameObject.Find("GameArea").GetComponent<EdgeCollider2D>().bounds;
        this.levelGenerator.GetMinos();
    }
}

[CustomEditor(typeof(LevelManager))]
class LevelManagerEditor : Editor
{

    public static LevelGenerator loadedLevelGenerator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelManager levelManager = (LevelManager)target;

        EditorGUILayout.BeginHorizontal();

        LevelGenerator newLevelGenerator = DropAreaGUI();
        loadedLevelGenerator = newLevelGenerator ?? loadedLevelGenerator;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(loadedLevelGenerator?.name, GUILayout.Width(100), GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Load From Generator", GUILayout.Width(100), GUILayout.ExpandWidth(true)) && loadedLevelGenerator != null)
        {
            levelManager.LoadLevelFromGenerator(loadedLevelGenerator);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save As Prefab", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Level", "levelname", "prefab", "Enter a filename", "Assets/Ressources/Levels/");

            if (path.Length >= 0)
            {
                GameObject levelContent = GameObject.Find("LevelContent");
                PrefabUtility.SaveAsPrefabAssetAndConnect(levelContent, path, InteractionMode.UserAction);
            }
        }
        else if (GUILayout.Button("Load From File", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
        {
            string prefabname = Path.GetFileNameWithoutExtension(EditorUtility.OpenFilePanel("Select a Level", "C:\\Users\\Calvin\\Documents\\prog\\brick-em\\Assets\\Resources\\Levels", "prefab"));

            GameObject gameObject = (GameObject)Instantiate(Resources.Load(string.Format("Levels/{0}", prefabname)));

            if (gameObject != null)
            {
                DestroyImmediate(GameObject.Find("LevelContent"));
                gameObject.transform.parent = GameObject.Find("GameArea").transform;
                gameObject.name = "LevelContent";
            }

        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    public LevelGenerator DropAreaGUI()
    {
        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(drop_area, "Drop Generator File Here");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(evt.mousePosition))
                    return null;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    MonoScript generatorFile = DragAndDrop.objectReferences[0] as MonoScript;
                    //Debug.Log(string.Format("file: {0}", generatorFile));

                    Type generatorType = generatorFile.GetClass();
                    //Debug.Log(string.Format("type: {0}", generatorType));

                    LevelGenerator levelGenerator = (LevelGenerator)Activator.CreateInstance(generatorType);

                    return levelGenerator;
                }
                break;
        }
        return null;
    }
}