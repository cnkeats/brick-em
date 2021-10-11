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
}