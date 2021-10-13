using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1 : Level
{
    public Level_1() : base()
    {
        levelMetadata = new LevelMetadata();
        levelMetadata.name = "Level_1";
        levelMetadata.blurb = "BLURB TEST";
        levelMetadata.description = "DESCRIPTION TEST";
        levelMetadata.baseScore = 0;
        levelMetadata.shotsAllowed = 15;

        SetLevelContentAsPrefabByName();
    }
}
