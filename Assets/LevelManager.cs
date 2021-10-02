using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelGenerator levelGenerator;

    public void LoadLevelFromGenerator(LevelGenerator levelGenerator)
    {
        this.levelGenerator = levelGenerator;
        //this.levelGenerator.bounds = GameObject.Find("GameArea").GetComponent<EdgeCollider2D>().bounds;
        levelGenerator.Generate();
        //this.levelGenerator.GetMinos();
    }
}