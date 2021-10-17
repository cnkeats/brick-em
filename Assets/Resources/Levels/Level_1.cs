public class Level_1 : Level
{
    public Level_1()
    {
        levelMetadata = new LevelMetadata();
        levelMetadata.name = "Level 1";
        levelMetadata.prefabName = "Level_1";
        levelMetadata.blurb = "BLURB TEST";
        levelMetadata.description = "DESCRIPTION TEST";
        levelMetadata.baseScore = 0;
        levelMetadata.shotsAllowed = 10;

        SetLevelContentAsPrefabByName();
    }
}
