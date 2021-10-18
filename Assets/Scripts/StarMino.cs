using UnityEngine;

public class StarMino : Mino
{
    public override int baseScoreValue => 100;

    public override void Hit(Collision2D collision)
    {
        base.Hit(collision);

        PlayerState.GetStarShot();
        FindObjectOfType<PlayerController>().AddShotToQueue("StarShot");
    }
}
