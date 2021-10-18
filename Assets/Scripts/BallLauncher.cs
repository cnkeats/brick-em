using UnityEngine;
using UnityEngine.Assertions;

public class BallLauncher : MonoBehaviour
{

    public LineRenderer aimLine;
    public Vector2 aim;

    private InputManager inputManager;

    [SerializeField]
    public BallLauncherState state;
    [SerializeField]
    private BallLauncherState prelockState;

    private GameObject objectToLaunch;

    private GameObject aimIndicator;


    public enum BallLauncherState
    {
        INACTIVE,
        LOADED,
        AIMING
    }

    void Awake()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        aimLine = gameObject.GetComponent<LineRenderer>();
        aimIndicator = transform.Find("AimIndicator").gameObject;
    }

    private void OnEnable()
    {
        //inputManager.OnStartTouch += ActiveTouch;
        inputManager.OnStartTouch += StartTouch;
        inputManager.OnActiveTouch += ActiveTouch;
        inputManager.OnEndTouch += EndTouch;
    }

    private void OnDisable()
    {
        //inputManager.OnStartTouch -= ActiveTouch;
        inputManager.OnStartTouch -= StartTouch;
        inputManager.OnActiveTouch -= ActiveTouch;
        inputManager.OnEndTouch -= EndTouch;
    }

    private void ActiveTouch(Vector2 position, float time)
    {
        if (state == BallLauncherState.AIMING)
        {
            AimBall(position);
        }
    }

    private void StartTouch(Vector2 position, float time)
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

        if (state == BallLauncherState.INACTIVE && collider.bounds.Contains(position))
        {
            GameObject queuedObject = GameObject.Find("Player").GetComponent<PlayerController>().PopShotQueue();

            if (queuedObject == null)
            {
                return;
            }

            objectToLaunch = Instantiate(queuedObject);
            objectToLaunch.gameObject.transform.parent = GameObject.Find("DynamicContent").transform;
            objectToLaunch.name = objectToLaunch.name.Replace("(Clone)", "");
            objectToLaunch.gameObject.transform.position = new Vector3(0, transform.position.y, 0);

            state = BallLauncherState.AIMING;
        }
    }

    private void EndTouch(Vector2 position, float time)
    {
        if (state == BallLauncherState.AIMING)
        {
            AimBall(position);
            aimLine.positionCount = 0;

            objectToLaunch.GetComponent<Ball>().Launch(aim);
            objectToLaunch = null;

            state = BallLauncherState.INACTIVE;

            aimIndicator.transform.localPosition = Vector3.zero;

            gameObject.SetActive(false);
        }
    }

    private void AimBall(Vector2 touchedPosition)
    {
        if (state != BallLauncherState.AIMING)
        {
            throw new AssertionException("AIMING BUT NOT AIMING! SOMETHING WENT WRONG!", ":(");
        }
        else
        {
            aim = touchedPosition - (Vector2)transform.position;

            float angleDifference = Vector2.SignedAngle(aim, Vector2.left);
            float shotAngleRange = 120f;

            if (angleDifference < 90 - shotAngleRange / 2 && angleDifference > -90)
            {
                aim = Quaternion.Euler(0, 0, shotAngleRange / 2).normalized * Vector2.up;
            }
            else if (angleDifference > 90 + shotAngleRange / 2 || angleDifference < -90)
            {
                aim = Quaternion.Euler(0, 0, -shotAngleRange / 2).normalized * Vector2.up;
            }

            RaycastHit2D raycastHit = Physics2D.CircleCast(objectToLaunch.transform.position,
                (objectToLaunch.GetComponent<CircleCollider2D>().radius * objectToLaunch.GetComponent<CircleCollider2D>().transform.localScale.x) + Physics2D.defaultContactOffset * 2,
                aim, 10f, LayerMask.GetMask("Ball", "Shield", "Launcher") ^ 0xFFFF);

            if (raycastHit.collider != null)
            {
                aimLine.positionCount = 2;
                aimLine.SetPositions(new Vector3[] { objectToLaunch.transform.position, raycastHit.centroid });
            }

            aimIndicator.transform.position = touchedPosition;
        }
    }
}
