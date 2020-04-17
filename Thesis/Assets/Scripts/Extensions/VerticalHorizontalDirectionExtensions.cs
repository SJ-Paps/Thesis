using SJ.Tools;

public static class VerticalHorizontalDirectionExtensions
{
    public static HorizontalDirection Inverse(this HorizontalDirection direction)
    {
        if (direction == HorizontalDirection.Left)
            return HorizontalDirection.Right;
        else
            return HorizontalDirection.Left;
    }

    public static VerticalDirection Inverse(this VerticalDirection direction)
    {
        if (direction == VerticalDirection.Down)
            return VerticalDirection.Up;
        else
            return VerticalDirection.Down;
    }
}
