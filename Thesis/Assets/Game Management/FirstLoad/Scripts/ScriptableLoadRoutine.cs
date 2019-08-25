using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableLoadRoutine : SJMonoBehaviour
{
    public abstract bool IsCompleted { get; }
    public abstract bool IsFaulted { get; }

    public abstract void Load();
    public abstract bool ShouldRetry();

    public virtual void ThrowExceptionIfNeeded()
    {

    }
}
