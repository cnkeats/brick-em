public class StarMino : Mino
{
    public override int baseScoreValue => 100;

    public override void Hit()
    {
        base.Hit();

        PlayerState.GetStarShot();
        FindObjectOfType<PlayerController>().AddShotToQueue("StarShot");
    }
}
