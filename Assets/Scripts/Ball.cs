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
    public float speedLimit = 100f;

    [Space(20)]
    [Range(1, 10)]
    public float debugScale = 1;
    public float debugPersistence = 2f;
    public int maxBounces = 0;

    private BallGizmoDisplayData ballData;

    [ExecuteInEditMode]
    void Awake()
    {
        //radius = gameObject.GetComponent<CircleCollider2D>().radius;
        //speed = velocity.magnitude;

        ballData = new BallGizmoDisplayData();
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(1.3f, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("HIT");


        //if (collision.gameObject
        //{
        if (collision.gameObject.CompareTag("Mino"))
        {
            collision.gameObject.GetComponent<Mino>().Hit();
        }
        //}
    }

    private void Update()
    {
        transform.up = gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        rigidbody.velocity = Mathf.Clamp(rigidbody.velocity.magnitude, -speedLimit, speedLimit) * rigidbody.velocity.normalized;
        speed = rigidbody.velocity.magnitude;

        //rigidbody.velocity = new Vector3(.2f, 1f) * Time.fixedDeltaTime;
        //rigidbody.MovePosition(transform.position + (Vector3)rigidbody.velocity * Time.fixedDeltaTime);
        

        /*
        // Speed limit
        velocity = velocity.magnitude > speedLimit ? Vector3.ClampMagnitude(velocity, speedLimit) : velocity;
        speed = velocity.magnitude;

        float distanceToMove = velocity.magnitude * Time.fixedDeltaTime;

        List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();
        RaycastHit2D currentHit;
        Vector3 startPoint = gameObject.transform.position;

        List<Collider2D> hitColliders = new List<Collider2D>();

        MarkPoint(transform.position, Color.black, 0.005f, true);

        int bounceLimit = 10;
        for (int bounces = 0; bounces < bounceLimit; bounces++)
        {
            // If we have bounces remaining, do a circle cast from the start point towards our current heading for the distance we need to move
            currentHit = Physics2D.CircleCast(startPoint, radius * gameObject.transform.localScale.x, velocity, distanceToMove, LayerMask.GetMask("Ball") ^ 0xFFFF);
            if (currentHit.collider != null)
            {
                // If we are intersecting the collider, we need to eject ourselves
                if (currentHit.distance < Mathf.Epsilon)
                {
                    Debug.Log("TOO CLOSE");
                    Debug.DrawRay(currentHit.point, currentHit.normal / 8, Color.cyan);
                    Debug.Break();
                }

                hitColliders.Add(currentHit.collider);
                raycastHits.Add(currentHit);

                // Bump our speed
                velocity += acceleration * Time.fixedDeltaTime * velocity.normalized;

                // Reflect heading about the normal
                velocity = Vector3.Reflect(velocity, currentHit.normal);

                // Subtract the distance of the hit from the distance to move
                distanceToMove -= currentHit.distance;

                Vector3 v = Vector3.zero;

                // Velocity influence of whatever we hit

                // Set the new start point to the centroid of the hit, plus a little bit to move it out of the normal
                startPoint = currentHit.centroid + currentHit.normal * 0.002f;

                // Mark the target as hit
                if (currentHit.collider.gameObject.GetComponent<Mino>())
                {
                    currentHit.collider.gameObject.GetComponent<Mino>().Hit();
                }
            }
            else
            {
                // If we don't hit anything, we are done and have our final position
                break;
            }
        }

        // If we hit the bounce limit, something has gone wrong. We're going to pop instead of risk being launched out of bounds


        ballData.previousPosition = gameObject.transform.position;
        ballData.startPoint = startPoint;
        ballData.raycastHits = raycastHits;

        Vector3 endPoint = startPoint + (velocity.normalized * distanceToMove);

        gameObject.transform.position = endPoint;
        gameObject.transform.rotation.SetLookRotation(velocity);
        */
    }

    private void OnDrawGizmos()
    {
        Vector2 velocityPosition = new Vector2(-5, 4);
        Handles.Label(velocityPosition + Vector2.left * 2, "Velocity");
        //Handles.Label(velocityPosition + Vector2.left * 2 + Vector2.down, string.Format("Speed: {0}", velocity.magnitude));
        //Debug.DrawRay(velocityPosition, velocity.normalized, Color.yellow);

        if (ballData.previousPosition != null)
        {
            //Debug.DrawLine((Vector3)ballData.previousPosition, transform.position, Color.yellow, debugPersistence);
        }
        
        MarkPoint(transform.position);
        if (ballData.raycastHits != null)
        {
            Vector2 hitsPosition = new Vector2(-6, 2);

            if (ballData.raycastHits.Count > 0)
            {
                foreach (RaycastHit2D hit in ballData.raycastHits)
                {
                    MarkPoint(hit.point, persistent: true);
                    Debug.DrawRay(hit.point, hit.normal/4, Color.cyan, debugPersistence);
                }
            }
            Handles.Label(hitsPosition, string.Format("Bounces: {0}", ballData.raycastHits.Count));
            maxBounces = ballData.raycastHits.Count > maxBounces ? ballData.raycastHits.Count : maxBounces;

            Vector3 endPoint = Vector3.zero;
            Vector3 currentPoint = ballData.startPoint;

            for (int i = 0; i < ballData.raycastHits.Count; i++)
            {
                if (ballData.raycastHits[i].collider != null)
                {
                    Debug.DrawLine(currentPoint, ballData.raycastHits[i].centroid);
                }
            }
        }
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