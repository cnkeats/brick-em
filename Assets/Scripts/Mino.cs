using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public enum MinoState { HEALTHY, CRACKED, DESTROYED };
    
    public MinoState state = MinoState.HEALTHY;
    public int maxHits = 1;
    public int currentHits = 0;

    virtual public int baseScoreValue { get => 25; }

    public virtual void Hit()
    {
        currentHits++;
        
        if (currentHits >= maxHits)
        {
            state = MinoState.DESTROYED;
            gameObject.SetActive(false);

            PlayerController.AddToScore(baseScoreValue);
        }
    }
}
