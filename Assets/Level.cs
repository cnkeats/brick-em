using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level
{
    abstract public string name { get; }

    public abstract List<GameObject> GetMinos();

    public Bounds bounds;
}
