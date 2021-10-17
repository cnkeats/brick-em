public class PointItem : ConsumableItem
{
    public int value = 500;

    public override void Consume()
    {
        PlayerController.AddToScore(value);
        base.Consume();
    }
}
