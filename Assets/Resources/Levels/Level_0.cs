using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_0 : Level
{
    public Level_0()
    {
        levelMetadata = new LevelMetadata();
        levelMetadata.name = "Level 0";
        levelMetadata.prefabName = "Level_0";
        levelMetadata.blurb = "BLURB TEST";
        levelMetadata.description = "DESCRIPTION TEST";
        levelMetadata.baseScore = 0;
        levelMetadata.shotsAllowed = 15;

        SetLevelContentAsPrefabByName();
    }
}
