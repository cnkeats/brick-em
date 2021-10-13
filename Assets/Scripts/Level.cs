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
        SetLevelContentAsPrefabByName();
    }

    protected GameObject SetLevelContentAsPrefabByName()
    {
        levelContent = Resources.Load(string.Format("Levels/{0}", levelMetadata.name)) as GameObject;
        return levelContent;
    }
}
