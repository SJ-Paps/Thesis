using SJ.GameEntities.Characters;

public static class FaceDirectionExtensions
{
    public static FaceDirection Inverse(this FaceDirection direction)
    {
        if (direction == FaceDirection.Left)
            return FaceDirection.Right;
        else
            return FaceDirection.Left;
    }
}
