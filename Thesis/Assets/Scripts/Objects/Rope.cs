using UnityEngine;

public partial class Rope : ClimbableObject
{
    [SerializeField]
    private Rigidbody2D[] segments;

    [SerializeField]
    private Transform topPoint, bottomPoint;
    

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

    public bool IsNearTop(Vector2 position, float sqrDistance)
    {
        return ((Vector2)topPoint.position - position).sqrMagnitude <= sqrDistance;
    }

    public bool IsNearBottom(Vector2 position, float sqrDistance)
    {
        return ((Vector2)bottomPoint.position - position).sqrMagnitude <= sqrDistance;
    }

}