using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour {
    private TouchControls touchControls;

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void ActiveTouchEvent(Vector2 vector2, float time);
    public event ActiveTouchEvent OnActiveTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    private void Awake()
    {
        touchControls = new TouchControls();
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
        touchControls.Touch.TouchPress.started -= context => EndTouch(context);

        touchControls.Touch.TouchPosition.performed += context => ActiveTouch(context);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null) {
            OnStartTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null) {
            OnEndTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
        }
    }

    private void ActiveTouch(InputAction.CallbackContext context)
    {
        if (OnActiveTouch != null)
        {
            OnActiveTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
    }
}
