﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SJColliderBase : MonoBehaviour
{
    public abstract void ChangeSize(Vector3 size);
    public abstract Vector3 GetSize();
}

public abstract class SJCollider2DBase : SJColliderBase
{
    public Collider2D InnerCollider { get; private set; }

    public Bounds bounds
    {
        get
        {
            return InnerCollider.bounds;
        }
    }

    public Vector2 offset
    {
        get
        {
            return InnerCollider.offset;
        }

        set
        {
            InnerCollider.offset = value;
        }
    }



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

    public Bounds bounds
    {
        get
        {
            return InnerCollider.bounds;
        }
    }

    protected void Awake()
    {
        InnerCollider = GetComponent<Collider>();

        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}