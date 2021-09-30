using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float radius;

    public float speedLimit = 100f;
    public float speed = 1.0f;
    public float acceleration = 0.0f;
    public Vector2 heading = new Vector2(0, -1).normalized;
    public Vector2 newHeading;

    public bool drawDebug = false;
    public bool drawGizmo = true;

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
        // Speed limit
        speed = speed > speedLimit ? speedLimit : speed;

        float distanceToMove = speed * Time.fixedDeltaTime;

        RaycastHit2D raycastHit;
        Vector3 startPoint = gameObject.transform.position;

        int bounceLimit = 2000;
        for (int bounces = 0; bounces < bounceLimit; bounces++)
        {
            // If we have bounces remaining, do a circle cast from the start point towards our current heading for the distance we need to move
            raycastHit = Physics2D.CircleCast(startPoint, radius * gameObject.transform.localScale.x, heading, distanceToMove, LayerMask.GetMask("Ball") ^ 0xFFFF);

            if (raycastHit.collider != null)
            {
                // Bump our speed
                speed += acceleration * Time.fixedDeltaTime;

                // Reflect heading about the noirmal
                heading = heading - 2 * (Vector2.Dot(heading, raycastHit.normal) * raycastHit.normal);

                // Subtract the distance of the hit from the distance to move
                distanceToMove -= raycastHit.distance;

                // Set the new start point to the centroid of the hit, plus a little bit to move it out of the normal
                startPoint = raycastHit.centroid + raycastHit.normal * 0.002f;

                // Mark the target as hit
                if (raycastHit.collider.gameObject.GetComponent<Mino>())
                {
                    raycastHit.collider.gameObject.GetComponent<Mino>().Hit();
                }
            }
            else
            {
                // If we don't hit anything, we are done and have our final position
                break;
            }
        }

        Vector3 endPoint = startPoint + (Vector3)(heading * distanceToMove);

        gameObject.transform.position = endPoint;
        gameObject.transform.rotation.SetLookRotation(heading);

        if (drawDebug)
        {
            DrawMovementProjection();
        }
    }

    private void DrawMovementProjection()
    {
        Debug.DrawRay(new Vector2(-4, 4), heading, Color.yellow);

        float distanceToMove = speed * Time.fixedDeltaTime;

        // Mark object's center
        MarkPoint(gameObject.transform.position);

        Color[] bounceColors = { Color.white, Color.green, Color.yellow, Color.magenta };

        RaycastHit2D raycastHit;
        Vector3 startPoint = gameObject.transform.position;
        Vector2 nextHeading = heading;

        int bounceLimit = 2000;
        for (int bounces = 0; bounces < bounceLimit; bounces++)
        {
            // If we have bounces remaining, do a circle cast from the start point towards our current heading for the distance we need to move
            raycastHit = Physics2D.CircleCast(startPoint, radius * gameObject.transform.localScale.x, nextHeading, distanceToMove, LayerMask.GetMask("Ball") ^ 0xFFFF);

            if (raycastHit.collider != null)
            {
                // Bump our speed
                speed += acceleration * Time.fixedDeltaTime;

                // Draw the raycast until the hit
                Debug.DrawRay(startPoint, nextHeading * raycastHit.distance, bounceColors[bounces % bounceColors.Length]);

                // Draw original heading red
                //Debug.DrawRay(startPoint, heading * gameObject.transform.localScale.x, Color.red);

                // Mark the hit point and draw the normal
                MarkPoint(raycastHit.point, "", Color.black, 0.05f);
                Debug.DrawRay(raycastHit.point, raycastHit.normal / 4, Color.cyan);

                // Reflect heading about the noirmal
                nextHeading = nextHeading - 2 * (Vector2.Dot(nextHeading, raycastHit.normal) * raycastHit.normal);

                // Subtract the distance of the hit from the distance to move
                distanceToMove -= raycastHit.distance;

                // Set the new start point to the centroid of the hit, plus a little bit to move it out of the normal
                startPoint = raycastHit.centroid + raycastHit.normal * 0.002f;

                // Mark the centroid
                //MarkPoint(raycastHit.centroid, "", Color.magenta, 0.05f);
            }
            else
            {
                // If we don't hit anything, we are done and have our final position
                break;
            }
        }

        Vector3 endPoint = startPoint + (Vector3)(nextHeading * distanceToMove);

        Debug.DrawRay(startPoint, nextHeading * distanceToMove, Color.blue);
        MarkPoint(endPoint, "end position", Color.green, 0.01f);
        Debug.DrawRay(new Vector2(-4, 2), nextHeading, Color.yellow);
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmo)
        {
            return;
        }

        Debug.DrawRay(new Vector2(-4, 4), heading, Color.yellow);

        float distanceToMove = speed * Time.fixedDeltaTime;

        // Mark object's center
        MarkPoint(gameObject.transform.position);

        Color[] bounceColors = { Color.white, Color.green, Color.yellow, Color.magenta };

        RaycastHit2D raycastHit;
        Vector3 startPoint = gameObject.transform.position;

        int bounceLimit = 20;
        for (int bounces = 0; bounces < bounceLimit; bounces++)
        {
            // If we have bounces remaining, do a circle cast from the start point towards our current heading for the distance we need to move
            raycastHit = Physics2D.CircleCast(startPoint, radius * gameObject.transform.localScale.x, heading, distanceToMove, LayerMask.GetMask("Ball") ^ 0xFFFF);
            //Debug.Log("CAST" + bounces);
            //Debug.Log(distanceToMove);
            //Debug.Log(raycastHit.collider);

            if (raycastHit.collider != null)
            {
                // Draw the raycast until the hit
                Debug.DrawRay(startPoint, heading * raycastHit.distance, bounceColors[bounces % bounceColors.Length]);

                // Draw original heading red
                //Debug.DrawRay(startPoint, heading * gameObject.transform.localScale.x, Color.red);

                // Mark the hit point and draw the normal
                MarkPoint(raycastHit.point, "", Color.black, 0.05f);
                Debug.DrawRay(raycastHit.point, raycastHit.normal/4, Color.cyan);

                // Reflect heading about the noirmal
                heading = heading - 2 * (Vector2.Dot(heading, raycastHit.normal) * raycastHit.normal);

                // Subtract the distance of the hit from the distance to move
                distanceToMove -= raycastHit.distance;

                // Set the new start point to the centroid of the hit, plus a little bit to move it out of the normal
                startPoint = raycastHit.centroid + raycastHit.normal * 0.002f;

                // Mark the centroid
                //MarkPoint(raycastHit.centroid, "", Color.magenta, 0.05f);
            }
            else
            {
                // If we don't hit anything, we are done and have our final position
                //startPoint = startPoint + (Vector3)(heading * distanceToMove);
                break;
            }
        }
        Debug.DrawRay(startPoint, heading * distanceToMove, Color.blue);
        MarkPoint(startPoint + (Vector3)(heading * distanceToMove), "end position", Color.green, 0.01f);
        Debug.DrawRay(new Vector2(-4, 2), heading, Color.yellow);
    }

    private void MarkPoint(Vector2 point, string label = "", Color? color = null, float size = 0.1f)
    {
        Debug.DrawRay(point + new Vector2(-1, 1).normalized * size, new Vector2(1, -1).normalized * size * 2, color ?? Color.white);
        Debug.DrawRay(point + new Vector2(1, 1).normalized * size, new Vector2(-1, -1).normalized * size * 2, color ?? Color.white);
        //Handles.Label(point + new Vector2(1, 0) * 0.05f, label);
    }
}
