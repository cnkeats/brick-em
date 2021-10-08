using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour {

    private TouchControls touchControls;

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void ActiveTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnActiveTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    private Camera mainCamera;

    private void Awake()
    {
        touchControls = new TouchControls();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void Start()
    {
        touchControls.Touch.TouchPress.started += context => StartTouch(context);
        touchControls.Touch.TouchPress.canceled += context => EndTouch(context);

        //touchControls.Touch.TouchPosition.started += context => StartTouch(context);
        touchControls.Touch.TouchPosition.performed += context => ActiveTouch(context);
        //touchControls.Touch.TouchPosition.started -= context => EndTouch(context);
        //touchControls.Touch.TouchPosition.canceled -= context => ActiveTouch(context);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("START");
        if (OnStartTouch != null) {
            OnStartTouch(Utils.ScreenToWorld(mainCamera, touchControls.Touch.TouchPosition.ReadValue<Vector2>()), (float)context.startTime);
        }
    }

    private void ActiveTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("ACTIVE");
        if (OnActiveTouch != null)
        {
            OnActiveTouch(Utils.ScreenToWorld(mainCamera, touchControls.Touch.TouchPosition.ReadValue<Vector2>()), (float)context.time);
        }
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("END");
        if (OnEndTouch != null) {
            OnEndTouch(Utils.ScreenToWorld(mainCamera, touchControls.Touch.TouchPosition.ReadValue<Vector2>()), (float)context.time);
        }
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, touchControls.Touch.TouchPosition.ReadValue<Vector2>());
    }

    private void DrawTouch (Vector2 touchPosition)
    {
        Debug.DrawRay(touchPosition + new Vector2(-.1f, .1f), new Vector2(.2f, -.2f), Color.cyan);
        Debug.DrawRay(touchPosition + new Vector2(.1f, .1f), new Vector2(-.2f, -.2f), Color.cyan);
    }
}
