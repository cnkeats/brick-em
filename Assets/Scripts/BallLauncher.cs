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

    void Awake()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        width = gameObject.GetComponent<BoxCollider2D>().bounds.size.x;
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
            ball.transform.position = transform.position;
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
