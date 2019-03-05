using UnityEngine;

public abstract class ClimbableObject : ActivableObject<Character>, IClimbable
{
    [SerializeField]
    private Transform bottomPoint;

    [SerializeField]
    private float climbDifficulty;

    private new SJCollider2D collider;

    public SJCollider2D Collider
    {
        get
        {
            if(collider == null)
            {
                collider = GetComponent<SJCollider2D>();
            }

            return collider;
        }
    }

    public Vector3 Bottom
    {
        get
        {
            return bottomPoint.position;
        }
    }

    public override bool Activate(Character user)
    {
        return true;
    }

    public float GetClimbDifficulty()
    {
        return climbDifficulty;
    }
}
