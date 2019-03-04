using UnityEngine;

public interface IClimbable
{
    float ClimbDifficulty();

    Vector3 Top { get; }
    Vector3 Bottom { get; }
}
