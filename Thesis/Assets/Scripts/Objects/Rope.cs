using UnityEngine;

public partial class Rope : ClimbableObject
{
    [SerializeField]
    private Rigidbody2D[] segments;

    public Rigidbody2D GetNearestSegment(Vector2 position)
    {
        Rigidbody2D nearest = segments[0];

        for(int i = 1; i < segments.Length; i++)
        {
            Rigidbody2D current = segments[i];

            if((position - current.position).sqrMagnitude < (position - nearest.position).sqrMagnitude)
            {
                nearest = current;
            }
        }

        return nearest;
    }
}