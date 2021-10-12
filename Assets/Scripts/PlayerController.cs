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

    private static List<GameObject> shotQueueDisplay = new List<GameObject>();

    private static float edgeOffset = .2f;
    private static float queueOffset = .3f;

    private void Awake()
    {
        launcher = GameObject.Find("BallLauncher").GetComponent<BallLauncher>();

        shotQueue.Add(Resources.Load("Prefabs/Ball") as GameObject);
        shotQueue.Add(Resources.Load("Prefabs/Ball") as GameObject);
        shotQueue.Add(Resources.Load("Prefabs/StarShot") as GameObject);
        shotQueue.Add(Resources.Load("Prefabs/StarShot") as GameObject);
        shotQueue.Add(Resources.Load("Prefabs/Ball") as GameObject);

        Debug.Log(shotQueue.Count);

        CreateShotQueueDisplay();
    }

    public void Update()
    {
    }

    public static GameObject PopShotQueue()
    {
        GameObject nextshot = shotQueue[0];
        shotQueue.RemoveAt(0);

        RemoveFirstFromShotQueueDisplay();

        return nextshot;
    }

    private static void RemoveFirstFromShotQueueDisplay()
    {
        Destroy(shotQueueDisplay[0]);
        shotQueueDisplay.RemoveAt(0);

        foreach (GameObject shotQueueSprite in shotQueueDisplay)
        {
            shotQueueSprite.transform.Translate(Vector2.left * queueOffset);
        }
    }

    private void CreateShotQueueDisplay()
    {
        for (int i = 0; i < shotQueue.Count; i++)
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
}
