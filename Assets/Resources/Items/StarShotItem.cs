using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShotItem : ConsumableItem
{
    public override void Consume()
    {
        player.GainNextShot(Resources.Load("Prefabs/StarShot") as GameObject);
        base.Consume();
    }
}
