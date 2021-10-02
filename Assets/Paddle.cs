using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float keySpeed = 4.0f;

    public float leftBoundary = -5f;
    public float rightboundary = 5f;

    [SerializeField]
    private float leftEdge;
    [SerializeField]
    private float rightEdge;

    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate()
    {
        /*if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(Vector2.left * Time.deltaTime * keySpeed);
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(Vector2.right * Time.deltaTime * keySpeed);
        }*/

        Collider2D collider = gameObject.GetComponent<Collider2D>();
        Bounds bounds = collider.bounds;

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

        Collider2D collider = gameObject.GetComponent<Collider2D>();
        Bounds bounds = collider.bounds;

        Debug.DrawLine(
            new Vector2(leftEdge, gameObject.transform.position.y),
            new Vector2(rightEdge, gameObject.transform.position.y),
            Color.red);
    }
}
