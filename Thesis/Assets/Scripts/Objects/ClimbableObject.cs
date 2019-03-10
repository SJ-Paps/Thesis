using UnityEngine;

public abstract class ClimbableObject : ActivableObject<Character>, IClimbable
{
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

    public override bool Activate(Character user)
    {
        return true;
    }

    public float GetClimbDifficulty()
    {
        return climbDifficulty;
    }
}
