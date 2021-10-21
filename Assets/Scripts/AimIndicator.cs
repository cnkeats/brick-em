using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    private BallLauncher ballLauncher;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        ballLauncher = FindObjectOfType<BallLauncher>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = !(PlayerController.shotQueue.Count == 0 && ballLauncher.state == BallLauncher.BallLauncherState.INACTIVE);
    }

    private void Update()
    {
        if (PlayerController.shotQueue.Count == 0 &&  ballLauncher.state == BallLauncher.BallLauncherState.INACTIVE)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }
}
