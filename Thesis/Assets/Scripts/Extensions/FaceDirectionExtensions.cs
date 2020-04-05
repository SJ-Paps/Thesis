using SJ.GameEntities.Characters;

public static class FaceDirectionExtensions
{
    public static HorizontalDirection Inverse(this HorizontalDirection direction)
    {
        if (direction == HorizontalDirection.Left)
            return HorizontalDirection.Right;
        else
            return HorizontalDirection.Left;
    }
}
