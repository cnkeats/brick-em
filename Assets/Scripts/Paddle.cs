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

    private InputManager inputManager;

    private Camera cameraMain;

    public Vector3 touchedPosition;

    public float speedLimit = 1.0f;

    //public Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        cameraMain = Camera.main;
    }

    void FixedUpdate()
    {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        Bounds bounds = collider.bounds;

        //if (activeTouch)
        //{
        //    Vector3 targetPosition = new Vector3(touchedPosition.x, transform.position.y, transform.position.z);
        //    //velocity = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speedLimit / 5f) - gameObject.transform.position;
        //    //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speedLimit / 5f);
        //
        //    //RaycastHit2D[] raycastHits;
        //    //gameObject.GetComponent<Rigidbody2D>().Cast(velocity, raycastHit, 
        //}

        leftEdge = -bounds.extents.x + gameObject.transform.position.x;
        rightEdge = bounds.extents.x + gameObject.transform.position.x;

        if (leftEdge - leftBoundary < 0)
        {
            gameObject.transform.Translate(-(leftEdge - leftBoundary), 0, 0);
            //velocity = Vector3.zero;
        }
        else if (rightEdge - rightboundary > 0)
        {
            gameObject.transform.Translate(-(rightEdge - rightboundary), 0, 0);
            //velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        //Debug.DrawLine(new Vector2(leftBoundary, gameObject.transform.position.y+1), new Vector2(rightboundary, gameObject.transform.position.y+1),Color.magenta);

        //Debug.DrawLine(
        //new Vector2(leftEdge, gameObject.transform.position.y),
        //new Vector2(rightEdge, gameObject.transform.position.y),
        //Color.red);

        //Debug.DrawRay(new Vector2(-5, 2), velocity, Color.yellow);
    }
}
