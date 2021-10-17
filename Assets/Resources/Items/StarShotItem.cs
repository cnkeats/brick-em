using UnityEngine;

public class StarShotItem : ConsumableItem
{
    public override void Consume()
    {
        player.GainNextShot(Resources.Load("Projectiles/StarShot") as GameObject);
        base.Consume();
    }
}
