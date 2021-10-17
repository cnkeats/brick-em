public class Level_Basics : Level
{
    public Level_Basics()
    {
        levelMetadata = new LevelMetadata();
        levelMetadata.name = "The Basics";
        levelMetadata.prefabName = "Level_Basics";
        levelMetadata.blurb = "Gotta start somewere";
        levelMetadata.description = "DESCRIPTION TEST";
        levelMetadata.baseScore = 0;
        levelMetadata.scoreMultiplier = 0;
        levelMetadata.shotsAllowed = 999;

        SetLevelContentAsPrefabByName();
    }
}
