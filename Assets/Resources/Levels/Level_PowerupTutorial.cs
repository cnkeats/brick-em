using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_PowerupTutorial : Level
{
    public Level_PowerupTutorial()
    {
        levelMetadata = new LevelMetadata();
        levelMetadata.name = "Powerups";
        levelMetadata.prefabName = "Level_PowerupTutorial";
        levelMetadata.blurb = "Buffs for the level!";
        levelMetadata.description = "DESCRIPTION TEST";
        levelMetadata.baseScore = 0;
        levelMetadata.scoreMultiplier = 0;
        levelMetadata.shotsAllowed = 999;

        SetLevelContentAsPrefabByName();
    }
}
