using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelGenerator
{
    abstract public string name { get; }

    public GameObject level;

    public GameObject levelArea;

    public GameObject paddle;

    public GameObject ball;

    public GameObject levelContent;

    public abstract List<GameObject> GetMinos();

    public Bounds bounds;

    public GameObject Generate()
    {
        GameObject gameArea = GameObject.Find("GameArea");

        if (gameArea != null)
        {
            GameObject.DestroyImmediate(gameArea);
        }

        if (gameArea == null)
        {
            Debug.Log("No GameArea - Making a new one!");
            gameArea = GameObject.Instantiate(Resources.Load("GameAreaTemplate")) as GameObject;
            gameArea.name = gameArea.name.Replace("Template(Clone)", "");
        }

        level = GameObject.Instantiate(Resources.Load("EmptyLevel")) as GameObject;
        level.name = level.name.Replace("(Clone)", "");
        level.transform.parent = gameArea.transform;

        return gameArea;
    }
}
