using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float launchSpeed = 5.0f;

    [SerializeField]
    private float speed;
    public float speedLimit = 10f;

    [Space(20)]
    [Range(-1, 10)]
    public int maxBounces = -1;
    public int bounces = 0;

    [HideInInspector]
    public BallState ballState = BallState.LAUNCHING;
    private BallGizmoDisplayData ballData;

    public enum BallState
    {
        INACTIVE = 0,
        LAUNCHING = 1,
        ACTIVE = 2
    }

    private void OnEnable()
    {
        LevelManager.currentProjectiles.Add(this);
    }

    private void OnDisable()
    {
        LevelManager.currentProjectiles.Remove(this);
    }

    [ExecuteInEditMode]
    void Awake()
    {
        ballData = new BallGizmoDisplayData();

        if (ballState == BallState.ACTIVE)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * launchSpeed;
            transform.up = gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
        }
    }

    public void Launch(Vector2 aim)
    {
        ballState = BallState.ACTIVE;
        gameObject.GetComponent<Rigidbody2D>().velocity = aim.normalized * launchSpeed;
        transform.up = gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mino"))
        {
            collision.gameObject.GetComponent<Mino>().Hit(collision);
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            collision.gameObject.GetComponent<Shield>().Hit();
        }
        else if (!collision.collider.isTrigger)
        {
            bounces++;
            if (maxBounces > 0 && bounces >= maxBounces)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        transform.up = gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
    }

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
            RaycastHit2D raycastHit = Physics2D.CircleCast(startingPoint, radius + Physics2D.defaultContactOffset * 2, velocity, 10f, LayerMask.GetMask("Ball") ^ 0xFFFF);

            Utils.MarkPoint(startingPoint, Color.magenta);

            if (raycastHit.collider != null)
            {
                Utils.MarkPoint(raycastHit.centroid, Color.red, .5f);
                Debug.DrawRay(raycastHit.point, raycastHit.normal / 4, Color.cyan);
            }
            else
            {
                Debug.DrawRay(startingPoint, velocity, Color.blue);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 velocityPosition = new Vector2(-4, 4);
        Handles.Label(velocityPosition + Vector2.left * 2, "Velocity");

        Vector2 velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        Handles.Label(velocityPosition + Vector2.left * 2 + Vector2.down, string.Format("Speed: {0}", velocity.magnitude));
        Debug.DrawRay(velocityPosition, velocity.normalized / 3, Color.yellow);

        if (ballData.previousPosition != null)
        {
            Debug.DrawLine((Vector3)ballData.previousPosition, transform.position, Color.yellow, 2f);
        }

        ballData.previousPosition = gameObject.transform.position;

        DrawPrediction();
    }
#endif

    public struct BallGizmoDisplayData
    {
        public Vector3? previousPosition;
        public Vector3 startPoint;
        public List<RaycastHit2D> raycastHits;
    }
}