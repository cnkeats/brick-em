using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Level_0_Generator_OLD : LevelGenerator
{
    public override string name { get { return "Level-0"; } }

    public override List<GameObject> GetMinos()
    {



        List<GameObject> minos = new List<GameObject>();

        Transform parent = GameObject.Find("Hitables").transform;

        float minoWidth = (Resources.Load("Minos/LargeMino") as GameObject).GetComponent<Mino>().width;
        float minoHeight = (Resources.Load("Minos/LargeMino") as GameObject).GetComponent<Mino>().height;

        float gameAreaTop = bounds.center.y + bounds.extents.y;
        float gameAreaBottom = bounds.center.y - bounds.extents.y;
        float gameAreaLeft = bounds.center.x - bounds.extents.x;
        float gameAreaRight = -gameAreaLeft;

        float gameAreaHeight = bounds.size.y;
        float gameAreaWidth = bounds.size.x;

        int gridWidth = Convert.ToInt32(gameAreaWidth / minoWidth);
        int gridHeight = Convert.ToInt32(gameAreaHeight / minoHeight);

        float gridLeftOffset = minoWidth / 2;
        float gridTopOffset = minoHeight / 2;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight / 2; y++)
            {
                if (x == gridWidth / 2 - 1 || x == gridWidth / 2)
                {
                    continue;
                }


                float posX = gameAreaLeft + gridLeftOffset + (x * minoWidth);
                float posY = gameAreaTop - gridTopOffset - (y * minoHeight);

                GameObject gameObject = (GameObject)GameObject.Instantiate(Resources.Load("Minos/LargeMino"));
                gameObject.name = "LargeMino";
                gameObject.transform.parent = parent;

                Mino mino = gameObject.GetComponent<Mino>();
                mino.transform.position = new Vector3(posX, posY, 0);

                minos.Add(gameObject);
            }
        }

        return minos;
    }
}
