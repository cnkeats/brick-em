using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMino : Mino
{
    public override void Hit()
    {
        base.Hit();

        Debug.Log(PlayerState.starShots);
        PlayerState.GetStarShot();
        Debug.Log(PlayerState.starShots);
        GameObject.Find("Player").GetComponent<PlayerController>().LockAndLoad();
        Debug.Log(PlayerState.starShots);
    }
}
