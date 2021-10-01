using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelGenerator levelGenerator;

    public void LoadLevelFromGenerator(LevelGenerator levelGenerator)
    {
        this.levelGenerator = levelGenerator;
        //this.levelGenerator.bounds = GameObject.Find("GameArea").GetComponent<EdgeCollider2D>().bounds;
        levelGenerator.Generate();
        //this.levelGenerator.GetMinos();
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
                GameObject level = GameObject.Find("GameArea").GetComponentsInChildren<GameObject>().Where(child => child.tag == "Level").First();

                if (level != null)
                {
                    PrefabUtility.SaveAsPrefabAssetAndConnect(level, path, InteractionMode.UserAction);
                }
                else
                {
                    Debug.Log("Unable to find GameArea - prefab NOT saved.");
                }
            }
        }
        else if (GUILayout.Button("Load From Prefab", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
        {
            string prefabname = Path.GetFileNameWithoutExtension(EditorUtility.OpenFilePanel("Select a Level", "C:\\Users\\Calvin\\Documents\\prog\\brick-em\\Assets\\Resources\\Levels", "prefab"));

            GameObject gameArea = GameObject.Find("GameArea");
            if (gameArea != null)
            {
                GameObject.DestroyImmediate(gameArea);
            }
            gameArea = GameObject.Instantiate(Resources.Load("GameAreaTemplate")) as GameObject;
            gameArea.name = gameArea.name.Replace("Template(Clone)", "");

            GameObject prefabLevel = (GameObject)Instantiate(Resources.Load(string.Format("Levels/{0}", prefabname)));

            if (prefabLevel != null)
            {
                prefabLevel.name = prefabLevel.name.Replace("(Clone)", "");
                prefabLevel.transform.parent = gameArea.transform;
            }
            else
            {
                Debug.Log(string.Format("Failed to load level {0}", prefabname));
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