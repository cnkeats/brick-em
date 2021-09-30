using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Level level;

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadLevel()
    {
        level = new Level_0();
        level.bounds = GameObject.Find("GameArea").GetComponent<EdgeCollider2D>().bounds;
        level.GetMinos();
    }
}
