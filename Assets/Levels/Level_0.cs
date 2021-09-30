using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Level_0 : Level
{
    public override string name { get { return "Level-0"; } }

    public override List<GameObject> GetMinos()
    {
        List<GameObject> minos = new List<GameObject>();

        Transform parent = GameObject.Find("Hittables").transform;

        float minoWidth = (Resources.Load("Minos/LargeMino") as GameObject).GetComponent<Mino>().width;
        float minoHeight = (Resources.Load("Minos/LargeMino") as GameObject).GetComponent<Mino>().height;

        float gameAreaTop = bounds.center.y + bounds.extents.y;
        float gameAreaBottom = bounds.center.y - bounds.extents.y;
        float gameAreaLeft = bounds.center.x - bounds.extents.x;
        float gameAreaRight = -gameAreaLeft;

        float gameAreaHeight = bounds.size.y;
        float gameAreaWidth = bounds.size.x;

        for (float height = gameAreaTop - minoHeight / 2; height > (gameAreaBottom + gameAreaTop) / 2; height -= minoHeight)
        {
            for (float width = -bounds.extents.x + minoWidth / 2; width < bounds.extents.x - minoWidth / 2; width += minoWidth)
            {
                GameObject gameObject = (GameObject)GameObject.Instantiate(Resources.Load("Minos/LargeMino"));
                gameObject.name = "LargeMino";
                gameObject.transform.parent = parent;

                Mino mino = gameObject.GetComponent<Mino>();
                mino.transform.position = new Vector3(width, height, 0);

                minos.Add(gameObject);
            }
        }

        return minos;
    }
}
