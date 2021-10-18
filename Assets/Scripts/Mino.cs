using UnityEngine;

public class Mino : MonoBehaviour
{
    public enum MinoState { HEALTHY, CRACKED, DESTROYED };

    public MinoState state = MinoState.HEALTHY;
    public int maxHits = 1;
    public int currentHits = 0;

    private ParticleSystem pSystem;

    virtual public int baseScoreValue { get => 25; }

    public virtual void Hit(Collision2D collision)
    {
        currentHits++;

        if (currentHits >= maxHits)
        {
            state = MinoState.DESTROYED;
            gameObject.SetActive(false);

            PlayerController.AddToScore(baseScoreValue);
        }

        pSystem.Play();
        //particleSystem.transform.position = collision.GetContact(0).point;
        pSystem.transform.position = collision.transform.position;
        pSystem.transform.LookAt(collision.GetContact(0).point + collision.GetContact(0).normal);
        pSystem.textureSheetAnimation.SetSprite(0, gameObject.GetComponent<SpriteRenderer>().sprite);
        

        //particleSystem.get

        Utils.MarkPoint(collision.GetContact(0).point, Color.red, 5f);
    }

    private void OnEnable()
    {
        pSystem = FindObjectOfType<ParticleSystem>();

        if (maxHits > 0)
        {
            LevelManager.currentBreakableMinos.Add(this);
        }
    }

    private void OnDisable()
    {
        if (maxHits > 0)
        {
            LevelManager.currentBreakableMinos.Remove(this);
        }
    }
}
