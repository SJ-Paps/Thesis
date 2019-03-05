using UnityEngine;

public interface IClimbable
{
    float GetClimbDifficulty();

    Vector3 Top { get; }
    Vector3 Bottom { get; }
}
