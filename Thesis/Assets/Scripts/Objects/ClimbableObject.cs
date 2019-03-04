using UnityEngine;

public abstract class ClimbableObject : ActivableObject<Character>, IClimbable
{
    [SerializeField]
    private Transform topPoint, bottomPoint;

    [SerializeField]
    private float climbDifficulty;

    public Vector3 Top
    {
        get
        {
            return topPoint.position;
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

    public float ClimbDifficulty()
    {
        return climbDifficulty;
    }
}
