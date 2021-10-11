using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    InputManager inputManager;

    public Vector2 aim;

    private float width;

    public GameObject loadedAmmo;
    public GameObject shotAmmo;

    public LineRenderer aimLine;

    public bool lockedAndLoaded;

    void Awake()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        width = gameObject.GetComponent<BoxCollider2D>().bounds.size.x;
        aimLine = gameObject.GetComponent<LineRenderer>();
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
        AimBall(position);
    }

    private void StartTouch(Vector2 position, float time)
    {
        //Utils.MarkPoint(position, Color.green, 5f);

        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

        if (shotAmmo == null && collider.bounds.Contains(position))
        {
            if (loadedAmmo == null)
            {
                shotAmmo = Instantiate(Resources.Load("Prefabs/Ball")) as GameObject;
            }
            else
            {
                shotAmmo = Instantiate(loadedAmmo);
                shotAmmo.name = "TEST";
            }
            shotAmmo.name = shotAmmo.name.Replace("(Clone)", "");
            shotAmmo.transform.parent = GameObject.Find("DynamicContent").transform;

        }
        else if (shotAmmo != null && collider.bounds.Contains(position))
        {

        }

        if (shotAmmo != null && shotAmmo.GetComponent<Ball>().ballState == Ball.BallState.LAUNCHING)
        {
            shotAmmo.transform.position = transform.position;
        }
    }

    private void EndTouch(Vector2 position, float time)
    {
        //Utils.MarkPoint(position, Color.red, 5f);
        if (shotAmmo != null && shotAmmo.GetComponent<Ball>().ballState == Ball.BallState.LAUNCHING)
        {
            shotAmmo.GetComponent<Ball>().Launch(aim);
            shotAmmo = null;
        }
        
        if (aimLine != null)
        {
            aimLine.positionCount = 0;
        }
    }

    private void AimBall(Vector2 touchedPosition)
    {
        if (shotAmmo != null)
        {
            float clampedTouchedPosition = Mathf.Clamp(touchedPosition.x, -width / 2, width / 2);
            float percentageAcrossBoundingBox = (clampedTouchedPosition + width / 2) / width;
            float angle = Mathf.Lerp(-60, 60, percentageAcrossBoundingBox);
            aim = Quaternion.Euler(0, 0, -angle) * Vector2.up;

            RaycastHit2D raycastHit = Physics2D.CircleCast(shotAmmo.transform.position, shotAmmo.GetComponent<CircleCollider2D>().radius * shotAmmo.GetComponent<CircleCollider2D>().transform.localScale.x, aim, 10f, LayerMask.GetMask("Ball", "Shield", "Launcher") ^ 0xFFFF);

            if (raycastHit.collider != null)
            {
                aimLine.positionCount = 2;
                aimLine.SetPositions(new Vector3[] { shotAmmo.transform.position, raycastHit.centroid });
            }
        }
        else if (aimLine != null)
        {
            aimLine.positionCount = 0;
        }
    }

    public void LockAndLoad(Object ammo)
    {
        loadedAmmo = ammo as GameObject;
        lockedAndLoaded = true;

        Debug.Log(string.Format("Loading {0}", loadedAmmo));
    }
}
