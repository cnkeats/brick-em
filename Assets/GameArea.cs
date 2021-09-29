using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{

    public float leftEdge = -1.0f;
    public float rightEdge = 1.0f;
    public float topEdge = 1.0f;
    public float bottomEdge = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {

        Vector2 topLeftCorner = new Vector2(leftEdge, topEdge);
        Vector2 bottomRightCorner = new Vector2(rightEdge, bottomEdge);
        Vector2 topRightCorner = new Vector2(bottomRightCorner.x, topLeftCorner.y);
        Vector2 bottomLeftCorner = new Vector2(topLeftCorner.x, bottomRightCorner.y);

        Debug.DrawLine(topLeftCorner, topRightCorner, Color.cyan);
        Debug.DrawLine(topRightCorner, bottomRightCorner, Color.cyan);
        Debug.DrawLine(bottomRightCorner, bottomLeftCorner, Color.cyan);
        Debug.DrawLine(bottomLeftCorner, topLeftCorner, Color.cyan);
    }
}
