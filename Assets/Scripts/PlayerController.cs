using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public BallLauncher launcher;
    public static List<GameObject> shotQueue = new List<GameObject>();

    [Space(20)]
    public int shotLimit = -1;
    public int shotDisplayLimit = 7;
    public static int usedShots = 0;

    private static int level = 2;
    private static int score = 0;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        launcher = FindObjectOfType<BallLauncher>();

        for (int i = 0; i < 5; i++)
        {
            AddDefaultShotToQueue();
        }
    }

    public void Update()
    {

    }

    public static int AddToScore(int points)
    {
        score += points;
        UIManager.UpdateScoreDisplay(score);

        return score;
    }

    public int GetScore()
    {
        return score;
    }

    public GameObject PopShotQueue()
    {
        if (shotQueue.Count == 0)
        {
            return null;
        }

        GameObject nextshot = shotQueue[0];
        shotQueue.RemoveAt(0);
        usedShots++;

        UpdateNextShotImage();

        return nextshot;
    }

    private void AddDefaultShotToQueue()
    {
        AddShotToQueue("Ball");
    }

    [ContextMenu("Get Star Shot")]
    private void AddStarShotToQueue()
    {
        AddShotToQueue("StarShot");
    }

    public void AddShotToQueue(string shotName)
    {
        GameObject shotToAdd = Resources.Load(string.Format("Prefabs/{0}", shotName)) as GameObject;

        if (shotToAdd != null)
        {
            if (!shotName.Equals("Ball"))
            {
                //GameObject nextDefaultShot = shotQueue.Where(s => s.name.Equals("Ball")).FirstOrDefault();
                //shotQueue.Remove(nextDefaultShot);
            }

            shotQueue.Insert(0, shotToAdd);
        }

        UpdateNextShotImage();
    }

    public void UpdateNextShotImage()
    {
        if (shotQueue.Count > 0)
        {
            GameObject.Find("NextShotImage").GetComponent<Image>().sprite = shotQueue[0].GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            GameObject.Find("NextShotImage").GetComponent<Image>().sprite = (Resources.Load("Prefabs/Ball") as GameObject).GetComponent<SpriteRenderer>().sprite;
        }
    }
}
