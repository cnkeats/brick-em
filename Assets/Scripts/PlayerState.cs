using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerState
{
    public static int starShots = 0;

    public static void GetStarShot()
    {
        starShots++;
    }
}
