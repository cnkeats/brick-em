using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_0 : Level
{
    public override string name { get { return "Level-0"; } }

    public override List<GameObject> GetMinos()
    {
        List<GameObject> minos = new List<GameObject>();
        Debug.Log(bounds);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        int minoCount = 20;

        for (float width = -bounds.extents.x; width < bounds.extents.x; width += 0.3f)
        {
            GameObject gameObject = (GameObject)GameObject.Instantiate(Resources.Load("Minos/Mino"));
            gameObject.name = "Mino";

            Mino mino = gameObject.GetComponent<Mino>();
            mino.transform.position = new Vector3(width, 1.0f, 0);

            minos.Add(gameObject);
        }

        return minos;
    }
}
