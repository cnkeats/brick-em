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

    public Level currentLevel;
    public GameObject currentLevelObject;

    private PlayerController player;

    private string[] levelList = { "Level_0", "Level_2" };
    private int currentLevelIndex = -1;

    public void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        if (currentLevelObject != null)
        {
            currentLevelIndex = Array.FindIndex(levelList, n => n.Equals(currentLevelObject.name));
        }
    }

    public void LoadLevelFromGenerator(LevelGenerator levelGenerator)
    {
        this.levelGenerator = levelGenerator;
        //this.levelGenerator.bounds = GameObject.Find("GameArea").GetComponent<EdgeCollider2D>().bounds;
        levelGenerator.Generate();
        //this.levelGenerator.GetMinos();
    }

    public void ReloadLevel()
    {
        string name = "";

        foreach (Transform transform in GameObject.Find("GameArea").transform)
        {
            if (transform.gameObject.name.StartsWith("Level_"))
            {
                name = transform.gameObject.name;
            }
            Destroy(transform.gameObject);
        }

        GameObject.Find("Player").GetComponent<PlayerController>().Setup();
        LoadLevel(name);
    }

    public void LoadLevel(string levelName)
    {
        GameObject staticContent = Instantiate(Resources.Load("Prefabs/StaticContent")) as GameObject;
        staticContent.transform.Find("Shield").gameObject.SetActive(false);
        GameObject dynamicContent = Instantiate(Resources.Load("Prefabs/DynamicContent")) as GameObject;
        GameObject level = Instantiate(Resources.Load(string.Format("Levels/{0}", levelName))) as GameObject;

        staticContent.gameObject.name = staticContent.gameObject.name.Replace("(Clone)", "");
        dynamicContent.gameObject.name = dynamicContent.gameObject.name.Replace("(Clone)", "");
        level.gameObject.name = level.gameObject.name.Replace("(Clone)", "");

        staticContent.transform.parent = GameObject.Find("GameArea").transform;
        dynamicContent.transform.parent = GameObject.Find("GameArea").transform;
        level.transform.parent = GameObject.Find("GameArea").transform;
    }

    [ContextMenu("Load next level")]
    public void LoadNextLevel()
    {
        currentLevelIndex += 1;
        string nextLevelName = levelList[currentLevelIndex % levelList.Length];

        Debug.Log(string.Format("Loading index {0} - {1}", currentLevelIndex, nextLevelName));

        Type levelType = Type.GetType(nextLevelName);
        Level level = Activator.CreateInstance(levelType) as Level;

        if (level == null)
        {
            Debug.Log(string.Format("Failed to load level {0}!", nextLevelName));
            return;
        }

        Transform parent = currentLevelObject.transform.parent;

        if (Application.isEditor)
        {
            DestroyImmediate(currentLevelObject);

            foreach (Ball ball in FindObjectsOfType<Ball>())
            {
                DestroyImmediate(ball.gameObject);
            }
        }
        else
        {
            Destroy(currentLevelObject);

            foreach (Ball ball in FindObjectsOfType<Ball>())
            {
                Destroy(ball.gameObject);
            }
        }


        currentLevel = level;

        // Create level objects
        currentLevelObject = Instantiate(level.levelContent);
        currentLevelObject.transform.parent = parent;
        currentLevelObject.name = level.levelMetadata.name;
        currentLevelObject.tag = "Level";

        // Update player data
        player.LevelAdvance();

    }
}