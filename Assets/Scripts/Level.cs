using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level
{
    public LevelMetadata levelMetadata;

    public GameObject levelContent;

    public Level()
    {
        levelMetadata = new LevelMetadata();
    }

    protected GameObject SetLevelContentAsPrefabByName()
    {
        levelContent = Resources.Load(string.Format("Levels/Prefabs/{0}", levelMetadata.prefabName)) as GameObject;
        return levelContent;
    }
}
