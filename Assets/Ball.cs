using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float radius;

    public float speed = 1.0f;
    public float acceleration = 0.0f;
    public Vector2 heading = new Vector2(0, -1).normalized;
    public Vector2 newHeading;

    [ExecuteInEditMode]
    // Start is called before the first frame update
    void Start()
    {
        radius = gameObject.GetComponent<CircleCollider2D>().radius;
        heading = gameObject.transform.up;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        heading = heading.normalized;

        Vector3 newPosition = gameObject.transform.position;

        Debug.DrawRay(new Vector2(-4, 4), heading, Color.yellow);
        Debug.DrawRay(new Vector2(-4, 3), newHeading, Color.yellow);

        float distanceToMove = speed * Time.fixedDeltaTime;
        float distanceToEdgeOfSelf = radius * gameObject.transform.localScale.x;
        float fullraycastDistance = distanceToEdgeOfSelf + distanceToMove;

        //Debug.Log("Distance to move: " + distanceToMove);

        // Mark object's center
        Debug.DrawRay(gameObject.transform.position + new Vector3(-0.05f, 0.05f), new Vector3(0.1f, -0.1f));
        Debug.DrawRay(gameObject.transform.position + new Vector3(0.05f, 0.05f), new Vector3(-0.1f, -0.1f));

        // Mark object's new center
        Debug.DrawRay(gameObject.transform.position + (Vector3)(distanceToMove * heading) + new Vector3(-0.01f, 0.01f), new Vector3(0.02f, -0.02f));
        Debug.DrawRay(gameObject.transform.position + (Vector3)(distanceToMove * heading) + new Vector3(0.01f, 0.01f), new Vector3(-0.02f, -0.02f));

        Color[] bounceColors = { Color.white, Color.green, Color.yellow, Color.magenta };
        int bounces = 0;

        RaycastHit2D raycastHit = Physics2D.Raycast(gameObject.transform.position, heading, fullraycastDistance, LayerMask.GetMask("Ball") ^ 0xFFFF);
        if (raycastHit.collider != null)
        {
            bounces++;
            Debug.DrawRay(gameObject.transform.position, heading * fullraycastDistance, Color.red);
            Debug.DrawRay((Vector3)raycastHit.point + new Vector3(-0.05f, 0.05f), new Vector3(0.1f, -0.1f));
            Debug.DrawRay((Vector3)raycastHit.point + new Vector3(0.05f, 0.05f), new Vector3(-0.1f, -0.1f));

            Debug.Log("TEST0");
            Debug.Log(distanceToMove);
            Debug.Log(fullraycastDistance);
            Debug.Log(raycastHit.distance);
            Debug.Log(distanceToMove - raycastHit.distance);
            float postBounceDistance = distanceToMove - raycastHit.distance;
            Debug.Log(postBounceDistance);
            //Debug.Log("Post-bounce distance: " + postBounceDistance);
            newHeading = heading - 2 * (Vector2.Dot(heading, raycastHit.normal) * raycastHit.normal);

            int raycastBounceLimit = 20;
            for (int i = bounces - 1; i < raycastBounceLimit; i++)
            {
                if (postBounceDistance > 0)
                {
                    Debug.Log("TEST1");
                    Vector2 nextRaycastStartPoint = raycastHit.point + (newHeading * 0.01f);

                    raycastHit = Physics2D.Raycast(nextRaycastStartPoint, newHeading, postBounceDistance, LayerMask.GetMask("Ball") ^ 0xFFFF);
                    // Draw full raycast
                    //Debug.DrawRay(nextRaycastStartPoint, newHeading * postBounceDistance, bounceColors[(i % bounceColors.Length)]);
                    if (raycastHit.collider != null)
                    {
                        bounces++;
                        Debug.DrawRay((Vector3)raycastHit.point + new Vector3(-0.05f, 0.05f), new Vector3(0.1f, -0.1f));
                        Debug.DrawRay((Vector3)raycastHit.point + new Vector3(0.05f, 0.05f), new Vector3(-0.1f, -0.1f));


                        // Draw partial raycast
                        Debug.DrawRay(nextRaycastStartPoint, newHeading * raycastHit.distance, bounceColors[(i % bounceColors.Length)]);
                        newHeading = newHeading - 2 * (Vector2.Dot(newHeading, raycastHit.normal) * raycastHit.normal);

                        //Debug.Log("new distance to travel: " + raycastHit.distance);
                        //Debug.Log("Bounce: " + (i+1) + ", distance to go: " + postBounceDistance);
                    }
                    else
                    {
                        // Draw full raycast
                        Debug.DrawRay(nextRaycastStartPoint, newHeading * postBounceDistance, bounceColors[(i % bounceColors.Length)]);
                        Debug.Log("No collision.");
                        postBounceDistance = 0;
                        newPosition = nextRaycastStartPoint + (newHeading * postBounceDistance);
                    }
                    postBounceDistance = postBounceDistance - raycastHit.distance;
                }
            }

            //Debug.DrawRay((Vector3)raycastHit.point, (heading - 2 * (Vector2.Dot(heading, raycastHit.normal) * raycastHit.normal)) * postBounceDistance);
        }
        else
        {
            Debug.DrawRay(gameObject.transform.position, heading * fullraycastDistance, Color.blue);
            newPosition = gameObject.transform.position + (Vector3)(heading * speed * Time.fixedDeltaTime);
            newHeading = heading;
        }

        //gameObject.transform.Translate(heading * speed * Time.deltaTime);
        gameObject.transform.position = newPosition;

    }

    private void OnDrawGizmos()
    {
        heading = gameObject.transform.up;
        Debug.DrawRay(new Vector2(-4, 4), heading, Color.yellow);
        Debug.DrawRay(new Vector2(-4, 3), newHeading, Color.yellow);

        float distanceToMove = speed * Time.fixedDeltaTime;
        float distanceToEdgeOfSelf = radius * gameObject.transform.localScale.x;
        float fullraycastDistance = distanceToEdgeOfSelf + distanceToMove;

        Vector3 raycastPointOnSurface = gameObject.transform.position + (Vector3)(distanceToEdgeOfSelf * heading);

        RaycastHit2D temp = Physics2D.CircleCast(gameObject.transform.position, radius * gameObject.transform.localScale.x, heading, fullraycastDistance, LayerMask.GetMask("Ball") ^ 0xFFFF);
        if (temp.collider != null)
        {
            Debug.DrawRay(temp.point, new Vector3(-1, 1).normalized * 0.05f, Color.black);
            Debug.DrawRay(temp.point, new Vector3(-1, -1).normalized * 0.05f, Color.black);
            Debug.DrawRay(temp.point, new Vector3(1, 1).normalized * 0.05f, Color.black);
            Debug.DrawRay(temp.point, new Vector3(1, -1).normalized * 0.05f, Color.black);
            Debug.DrawRay(temp.point, temp.normal, Color.cyan);
            gameObject.GetComponents<CircleCollider2D>()[1].radius = radius * gameObject.transform.localScale.x;
        }

        //Debug.Log("Distance to move: " + distanceToMove);

        // Mark object's center
        Debug.DrawRay(gameObject.transform.position + new Vector3(-0.05f, 0.05f), new Vector3(0.1f, -0.1f));
        Debug.DrawRay(gameObject.transform.position + new Vector3(0.05f, 0.05f), new Vector3(-0.1f, -0.1f));

        // Mark object's new center
        Debug.DrawRay(gameObject.transform.position + (Vector3)(distanceToMove * heading) + new Vector3(-0.01f, 0.01f), new Vector3(0.02f, -0.02f), Color.yellow);
        Debug.DrawRay(gameObject.transform.position + (Vector3)(distanceToMove * heading) + new Vector3(0.01f, 0.01f), new Vector3(-0.02f, -0.02f), Color.yellow);

        // Mark object's edge of self along the heading
        Debug.DrawRay(raycastPointOnSurface + new Vector3(1, 1) * 0.01f, new Vector3(1, 1) * -0.02f, Color.cyan);
        Debug.DrawRay(raycastPointOnSurface + new Vector3(-1, 1) * 0.01f, new Vector3(1, -1) * 0.02f, Color.cyan);

        // Mark object's new edge of self along the heading
        Debug.DrawRay(raycastPointOnSurface + (Vector3)(distanceToMove * heading) + new Vector3(-0.01f, 0.01f), new Vector3(0.02f, -0.02f), Color.yellow);
        Debug.DrawRay(raycastPointOnSurface + (Vector3)(distanceToMove * heading) + new Vector3(0.01f, 0.01f), new Vector3(-0.02f, -0.02f), Color.yellow);

        Color[] bounceColors = { Color.white, Color.green, Color.yellow, Color.magenta };
        int bounces = 0;

        RaycastHit2D raycastHit = Physics2D.Raycast(raycastPointOnSurface, heading, fullraycastDistance, LayerMask.GetMask("Ball") ^ 0xFFFF);
        if (raycastHit.collider != null)
        {
            bounces++;
            Debug.DrawRay(raycastPointOnSurface, heading * fullraycastDistance, Color.red);
            Debug.DrawRay((Vector3)raycastHit.point + new Vector3(-0.05f, 0.05f), new Vector3(0.1f, -0.1f));
            Debug.DrawRay((Vector3)raycastHit.point + new Vector3(0.05f, 0.05f), new Vector3(-0.1f, -0.1f));

            float postBounceDistance = distanceToMove - raycastHit.distance;
            //Debug.Log("Post-bounce distance: " + postBounceDistance);
            newHeading = heading - 2 * (Vector2.Dot(heading, raycastHit.normal) * raycastHit.normal);

            int gizmoRaycastBounceLimit = 20;
            for (int i = bounces-1; i < gizmoRaycastBounceLimit; i++)
            {
                if (postBounceDistance > 0)
                {
                    Vector2 nextRaycastStartPoint = raycastHit.point + (newHeading * 0.01f);

                    raycastHit = Physics2D.Raycast(nextRaycastStartPoint, newHeading, postBounceDistance, LayerMask.GetMask("Ball") ^ 0xFFFF);
                    // Draw full raycast
                    //Debug.DrawRay(nextRaycastStartPoint, newHeading * postBounceDistance, bounceColors[(i % bounceColors.Length)]);
                    if (raycastHit.collider != null)
                    {
                        bounces++;
                        Debug.DrawRay((Vector3)raycastHit.point + new Vector3(-0.05f, 0.05f), new Vector3(0.1f, -0.1f));
                        Debug.DrawRay((Vector3)raycastHit.point + new Vector3(0.05f, 0.05f), new Vector3(-0.1f, -0.1f));


                        // Draw partial raycast
                        Debug.DrawRay(nextRaycastStartPoint, newHeading * raycastHit.distance, bounceColors[(i % bounceColors.Length)]);
                        newHeading = newHeading - 2 * (Vector2.Dot(newHeading, raycastHit.normal) * raycastHit.normal);

                        //Debug.Log("new distance to travel: " + raycastHit.distance);
                        //Debug.Log("Bounce: " + (i+1) + ", distance to go: " + postBounceDistance);
                    }
                    else
                    {
                        // Draw full raycast
                        Debug.DrawRay(nextRaycastStartPoint, newHeading * postBounceDistance, bounceColors[(i % bounceColors.Length)]);
                        //Debug.Log("No collision.");
                        postBounceDistance = 0;
                    }
                    postBounceDistance = postBounceDistance - raycastHit.distance;
                }
            }

            //Debug.DrawRay((Vector3)raycastHit.point, (heading - 2 * (Vector2.Dot(heading, raycastHit.normal) * raycastHit.normal)) * postBounceDistance);
        }
        else
        {
            Debug.DrawRay(raycastPointOnSurface, heading * fullraycastDistance, Color.blue);
            newHeading = heading;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal * 2.0f, Color.red);
            heading = (heading - 2 * (Vector2.Dot(heading, contact.normal) * contact.normal)).normalized;
        }

        speed += acceleration * Time.deltaTime;*/
    }
}
