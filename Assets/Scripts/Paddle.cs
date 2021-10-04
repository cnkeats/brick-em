using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    public float keySpeed = 4.0f;

    public float leftBoundary = -5f;
    public float rightboundary = 5f;

    [SerializeField]
    private float leftEdge;
    [SerializeField]
    private float rightEdge;

    private InputManager inputManager;

    private Camera cameraMain;

    public Vector3 touchedPosition;

    public float speedLimit = 1.0f;

    public Vector3 velocity = Vector3.zero;

    [SerializeField]
    private bool activeTouch = false;

    private void Awake()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        cameraMain = Camera.main;
    }

    private void OnEnable()
    {
       inputManager.OnStartTouch += StartTouch;
        inputManager.OnActiveTouch += ProcessTouch;
    }

    private void OnDisable()
    {
        inputManager.OnEndTouch -= EndTouch;
        inputManager.OnActiveTouch -= ProcessTouch;
    }

    public void StartTouch(Vector2 position, float time)
    {
        activeTouch = true;
    }

    public void EndTouch(Vector2 position, float time)
    {
        activeTouch = false;
    }

    public void ProcessTouch(Vector2 screenPosition, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, cameraMain.nearClipPlane);
        Vector3 worldCoordinates = cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;

        touchedPosition = worldCoordinates;
    }

    void FixedUpdate()
    {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        Bounds bounds = collider.bounds;

        if (activeTouch)
        {
            Vector3 targetPosition = new Vector3(touchedPosition.x, transform.position.y, transform.position.z);
            velocity = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speedLimit / 5f) - gameObject.transform.position;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speedLimit / 5f);
        }

        leftEdge = -bounds.extents.x + gameObject.transform.position.x;
        rightEdge = bounds.extents.x + gameObject.transform.position.x;

        if (leftEdge - leftBoundary < 0)
        {
            gameObject.transform.Translate(-(leftEdge - leftBoundary), 0, 0);
        }
        else if (rightEdge - rightboundary > 0)
        {
            gameObject.transform.Translate(-(rightEdge - rightboundary), 0, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(new Vector2(leftBoundary, gameObject.transform.position.y+1), new Vector2(rightboundary, gameObject.transform.position.y+1),Color.magenta);

        Debug.DrawLine(
            new Vector2(leftEdge, gameObject.transform.position.y),
            new Vector2(rightEdge, gameObject.transform.position.y),
            Color.red);

        Debug.DrawRay(new Vector2(-5, 2), velocity, Color.yellow);
    }
}
