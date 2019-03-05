using System;
using UnityEngine;

public abstract class SJCollider2D : SJCollider2DBase {

    public event Action<Collider2D> onEnteredTrigger;
    public event Action<Collider2D> onExitedTrigger;
    public event Action<Collider2D> onStayTrigger;

    public event Action<Collision2D> onEnteredCollision;
    public event Action<Collision2D> onExitedCollision;
    public event Action<Collision2D> onStayCollision;

    public bool IsTrigger
    {
        get
        {
            return InnerCollider.isTrigger;
        }

        set
        {
            InnerCollider.isTrigger = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onEnteredTrigger != null)
        {
            onEnteredTrigger(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (onExitedTrigger != null)
        {
            onExitedTrigger(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(onStayTrigger != null)
        {
            onStayTrigger(collision);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onEnteredCollision != null)
        {
            onEnteredCollision(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (onExitedCollision != null)
        {
            onExitedCollision(collision);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (onStayCollision != null)
        {
            onStayCollision(collision);
        }
    }

    public bool OverlapPoint(Vector2 point)
    {
        return InnerCollider.OverlapPoint(point);
    }

    public bool IsTouching(Collider2D collider)
    {
        return InnerCollider.IsTouching(collider);
    }

    public ColliderDistance2D Distance(SJCollider2D collider)
    {
        return InnerCollider.Distance(collider.InnerCollider);
    }
}
