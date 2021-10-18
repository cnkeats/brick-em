public class Level_SingleMino : Level
{
    public Level_SingleMino()
    {
        levelMetadata = new LevelMetadata();
        levelMetadata.name = "Single Mino";
        levelMetadata.prefabName = "Level_SingleMino";
        levelMetadata.blurb = "Gotta start somewere";
        levelMetadata.description = "DESCRIPTION TEST";
        levelMetadata.baseScore = 0;
        levelMetadata.scoreMultiplier = 0;
        levelMetadata.shotsAllowed = 2;

        SetLevelContentAsPrefabByName();
    }
}
