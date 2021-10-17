using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int maxHits = 10;
    public int currentHits = 0;

    [Space(10)]
    public bool unbreakable = false;

    [Space(10)]
    public List<Color> colors = new List<Color>();

    void Awake()
    {
        GetComponent<SpriteRenderer>().color = colors[0];
    }

    void Update()
    {
        Color c1 = Color.Lerp(colors[0], colors[1], (float)currentHits / (maxHits + 1));
        Color c2 = Color.Lerp(colors[1], colors[2], (float)currentHits / (maxHits + 1));
        GetComponent<SpriteRenderer>().color = Color.Lerp(c1, c2, (float)currentHits / (maxHits + 1));
    }

    public void Hit()
    {
        currentHits++;

        if (currentHits >= maxHits && !unbreakable)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
