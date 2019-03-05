using UnityEngine;

public interface IClimbable
{
    float GetClimbDifficulty();
    
    Vector3 Bottom { get; }
}
