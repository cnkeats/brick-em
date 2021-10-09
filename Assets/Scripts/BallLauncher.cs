using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    InputManager inputManager;

    private Vector2 startPosition;
    private Vector2 endPosition;

    private float startTime;
    private float endTime;

    private float width;

    public GameObject ball;

    public int spawnCount = 6;

    public List<LauncherSpawnPoint> spawnPoints = new List<LauncherSpawnPoint>();

    void Awake()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        width = gameObject.GetComponent<BoxCollider2D>().bounds.size.x;

        foreach (LauncherSpawnPoint spawnPoint in gameObject.GetComponentsInChildren<LauncherSpawnPoint>())
        {
            spawnPoints.Add(spawnPoint);
        }

        for (int i = spawnPoints.Count; i < spawnCount; i++)
        {
            GameObject gameObject = Instantiate(Resources.Load("Prefabs/LauncherSpawnPoint")) as GameObject;
            gameObject.name = gameObject.name.Replace("(Clone)", "");
            gameObject.transform.parent = transform;
        }
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += ActiveTouch;
        inputManager.OnActiveTouch += ActiveTouch;
        inputManager.OnEndTouch += EndTouch;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= ActiveTouch;
        inputManager.OnActiveTouch -= ActiveTouch;
        inputManager.OnEndTouch -= EndTouch;
    }

    private void ActiveTouch(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;

        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

        if (ball == null && collider.bounds.Contains(position))
        {
            ball = Instantiate(Resources.Load("Prefabs/Ball")) as GameObject;
            ball.name = ball.name.Replace("(Clone)", "");
            ball.transform.parent = GameObject.Find("DynamicContent").transform;

        }
        else if (ball != null && collider.bounds.Contains(position))
        {

        }

        if (ball != null && ball.GetComponent<Ball>().ballState == Ball.BallState.LAUNCHING)
        {
            float leftEdge = gameObject.transform.position.x - width / 2;
            float rightEdge = gameObject.transform.position.x + width / 2;

            float spawnWidth = width / (spawnCount + 1);

            float lerp = Mathf.InverseLerp(leftEdge, rightEdge, position.x);
            float temp = Mathf.Round(lerp * (spawnCount - 1)) + 1;

            for (int i = 1; i <= spawnCount; i++)
            {
                //Utils.MarkPoint(new Vector2(leftEdge + (i * spawnWidth), ball.transform.parent.position.y));
                //Debug.DrawLine(Vector3.zero, new Vector3(leftEdge + (i * spawnWidth), gameObject.transform.position.y, 0), Color.white, 1f);
            }

            //Debug.DrawLine(Vector3.zero, new Vector3(leftEdge + (temp * spawnWidth), gameObject.transform.position.y, 0), Color.red, 0.01f);
            //Debug.Log(Utils.RoundToIncrement(position.x, spawnWidth));

            ball.transform.position = new Vector3(leftEdge + (temp * spawnWidth), gameObject.transform.position.y);
        }
    }

    private void EndTouch(Vector2 position, float time)
    {
        if (ball != null && ball.GetComponent<Ball>().ballState == Ball.BallState.LAUNCHING)
        {
            ball.GetComponent<Ball>().Launch();
            //ball = null;
        }
    }
}
