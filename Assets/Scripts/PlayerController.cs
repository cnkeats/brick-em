using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BallLauncher launcher;
    public static List<GameObject> shotQueue = new List<GameObject>();

    [Space(20)]
    public int shotLimit = -1;
    public int shotDisplayLimit = 7;
    public static int usedShots = 0;

    private List<GameObject> shotQueueDisplay = new List<GameObject>();

    private static float edgeOffset = .2f;
    private static float queueOffset = .3f;

    private void Awake()
    {
        launcher = FindObjectOfType<BallLauncher>();

        for (int i = 0; i < 5; i++)
        {
            AddDefaultShotToQueue();
        }

        UpdateQueueDisplay();
    }

    public void Update()
    {
    }

    public GameObject PopShotQueue()
    {
        if (shotQueue.Count == 0)
        {
            return null;
        }

        GameObject nextshot = shotQueue[0];
        shotQueue.RemoveAt(0);
        RemoveFirstFromShotQueueDisplay();

        usedShots++;

        return nextshot;
    }

    private void RemoveFirstFromShotQueueDisplay()
    {
        Destroy(shotQueueDisplay[0]);
        shotQueueDisplay.RemoveAt(0);

        UpdateQueueDisplay();
    }

    private void UpdateQueueDisplay()
    {
        foreach (GameObject shotQueueSprite in shotQueueDisplay)
        {
            //shotQueueSprite.transform.Translate(Vector2.left * queueOffset);
            Destroy(shotQueueSprite);
        }

        int currentlyQueuedShots = shotQueue.Count;
        int limit = Math.Min(shotDisplayLimit, currentlyQueuedShots);

        for (int i = 0; i < limit; i++)
        {
            GameObject shotToDisplay = Instantiate(shotQueue[i]);
            shotToDisplay.name = shotToDisplay.name.Replace("(Clone)", "");
            shotToDisplay.transform.parent = transform;

            // Destroy all components except Transform and SpriteRenderer
            foreach (Component component in shotToDisplay.GetComponents<Component>().Where(c => c.GetType() != typeof(Transform) && c.GetType() != typeof(SpriteRenderer)))
            {
                Destroy(component);
            }

            float cameraHalfHeight = Camera.main.orthographicSize;
            float cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

            Vector2 bottomLeftCorner = new Vector2(-cameraHalfWidth, -cameraHalfHeight);
            Vector2 position = bottomLeftCorner + Vector2.up * edgeOffset + (Vector2.right * edgeOffset) + (Vector2.right * queueOffset * i);

            shotToDisplay.transform.position = position;

            shotQueueDisplay.Add(shotToDisplay);
        }
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
            shotQueue.Add(shotToAdd);
        }

        UpdateQueueDisplay();
    }
}
