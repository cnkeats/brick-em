using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public enum MinoState { HEALTHY, CRACKED, DESTROYED };
    
    public MinoState state = MinoState.HEALTHY;
    public int maxHits = 1;
    public int currentHits = 0;
    public float width = 0.24f;// + 0.03f;
    public float height = 0.24f + 0.03f;

    public void Hit()
    {
        currentHits++;
        
        if (currentHits >= maxHits)
        {
            state = MinoState.DESTROYED;
            //GameObject.Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
