using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BallLauncher launcher;

    private void Awake()
    {
         launcher = GameObject.Find("BallLauncher").GetComponent<BallLauncher>();
    }

    [ContextMenu("Get Star Shot")]
    public void PowerUp()
    {
        PlayerState.GetStarShot();
        LockAndLoad();
    }

    [ContextMenu("Lock and Load")]
    public void LockAndLoad()
    {
        Debug.Log(PlayerState.starShots);
        if (PlayerState.starShots > 0)
        {
            PlayerState.starShots--;
            launcher.LockAndLoad(Resources.Load("Prefabs/StarShot"));
        }
        else
        {
            launcher.LockAndLoad(Resources.Load("Prefabs/Ball"));
        }
    }
}
