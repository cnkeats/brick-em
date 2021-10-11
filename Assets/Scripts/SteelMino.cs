using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelMino : Mino
{
    public SteelMino()
    {
        maxHits = -1;
        currentHits = 0;
    }

    public override void Hit()
    {
        currentHits++;
    }
}
