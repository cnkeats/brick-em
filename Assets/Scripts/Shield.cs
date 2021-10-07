using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int maxHits = 10;
    public int currentHits = 0;

    public List<Color> colors = new List<Color>();

    void Awake()
    {
        GetComponent<SpriteRenderer>().color = colors[0];
    }

    void Update()
    {
        GetComponent<SpriteRenderer>().color = colors[(int)((float)currentHits / maxHits * colors.Count)];
    }

    public void Hit()
    {
        currentHits++;

        if (currentHits >= maxHits)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
