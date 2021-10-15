using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointItem : ConsumableItem
{
    public int value = 500;

    public override void Consume()
    {
        PlayerController.AddToScore(value);
        base.Consume();
    }
}
