using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SJColliderBase : MonoBehaviour
{
    public abstract void ChangeSize(Vector3 size);
}

public abstract class SJCollider2DBase : SJColliderBase
{
    public Collider2D InnerCollider { get; private set; }

    protected void Awake()
    {
        InnerCollider = GetComponent<Collider2D>();

        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}

public abstract class SJCollider3DBase : SJColliderBase
{
    public Collider InnerCollider { get; private set; }

    protected void Awake()
    {
        InnerCollider = GetComponent<Collider>();

        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}
