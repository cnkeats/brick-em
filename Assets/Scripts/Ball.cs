using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    //public float radius;

    //public Vector2 heading = new Vector2(0, -1).normalized;
    //public Vector2 newHeading;

    //Rigidbody2D rigidbody;

    [SerializeField]
    public float speed;
    public float acceleration = 0.0f;
    public float speedLimit = 10f;

    [Space(20)]
    [Range(1, 10)]
    public float debugScale = 1;
    public float debugPersistence = 2f;
    public int maxBounces = 0;

    public BallState ballState = BallState.LAUNCHING;

    private BallGizmoDisplayData ballData;

    public enum BallState
    {
        INACTIVE = 0,
        LAUNCHING = 1,
        ACTIVE = 2
    }

    [ExecuteInEditMode]
    void Awake()
    {
        ballData = new BallGizmoDisplayData();

        if (ballState == BallState.ACTIVE)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(1.3f, 5f);
            transform.up = gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
        }
    }

    public void Launch()
    {
        ballState = BallState.ACTIVE;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(1.3f, 5f);
        transform.up = gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mino"))
        {
            collision.gameObject.GetComponent<Mino>().Hit();
        }
        
        if (collision.gameObject.CompareTag("Shield"))
        {
            collision.gameObject.GetComponent<Shield>().Hit();
        }
    }

    private void Update()
    {
        transform.up = gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ballState == BallState.ACTIVE)
        {
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
            rigidbody.velocity = Mathf.Clamp(rigidbody.velocity.magnitude, -speedLimit, speedLimit) * rigidbody.velocity.normalized;
            speed = rigidbody.velocity.magnitude;
        }
    }

    private void DrawPrediction()
    {
        Vector2 velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        float radius = gameObject.GetComponent<CircleCollider2D>().radius * transform.localScale.x;

        if (velocity.magnitude != 0)
        {
            Vector2 startingPoint = transform.position;
            RaycastHit2D raycastHit =  Physics2D.CircleCast(startingPoint, radius, velocity, 10f, LayerMask.GetMask("Ball") ^ 0xFFFF);

            MarkPoint(startingPoint, Color.magenta);

            if (raycastHit.collider != null)
            {
                MarkPoint(raycastHit.centroid, Color.red);
                Debug.DrawRay(raycastHit.point, raycastHit.normal / 4, Color.cyan);
            }
            else
            {
                Debug.DrawRay(startingPoint, velocity, Color.blue);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 velocityPosition = new Vector2(-4, 4);
        Handles.Label(velocityPosition + Vector2.left * 2, "Velocity");

        Vector2 velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        Handles.Label(velocityPosition + Vector2.left * 2 + Vector2.down, string.Format("Speed: {0}", velocity.magnitude));
        Debug.DrawRay(velocityPosition, velocity.normalized/3, Color.yellow);

        if (ballData.previousPosition != null)
        {
            Debug.DrawLine((Vector3)ballData.previousPosition, transform.position, Color.yellow, debugPersistence);
        }

        ballData.previousPosition = gameObject.transform.position;

        DrawPrediction();
    }

    private void MarkPoint(Vector2 point, Color? color = null, float size = 0.1f, bool persistent = false)
    {
        Debug.DrawRay(point + new Vector2(-1, 1).normalized * size, 2 * size * new Vector2(1, -1).normalized, color ?? Color.white, persistent ? debugPersistence : 0);
        Debug.DrawRay(point + new Vector2(1, 1).normalized * size, 2 * size * new Vector2(-1, -1).normalized, color ?? Color.white, persistent ? debugPersistence : 0);
    }

    public struct BallGizmoDisplayData
    {
        public Vector3? previousPosition;
        public Vector3 startPoint;
        public List<RaycastHit2D> raycastHits;
    }
}