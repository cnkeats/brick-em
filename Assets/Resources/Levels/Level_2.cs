using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_2 : Level
{
    public Level_2()
    {
        levelMetadata = new LevelMetadata();
        levelMetadata.name = "Level 2";
        levelMetadata.prefabName = "Level_2";
        levelMetadata.blurb = "BLURB TEST";
        levelMetadata.description = "DESCRIPTION TEST";
        levelMetadata.baseScore = 0;
        levelMetadata.shotsAllowed = 1;

        SetLevelContentAsPrefabByName();
    }
}
