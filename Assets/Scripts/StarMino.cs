using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMino : Mino
{
    public override void Hit()
    {
        base.Hit();

        PlayerState.GetStarShot();
        GameObject.Find("Player").GetComponent<PlayerController>().LockAndLoad();
    }
}
