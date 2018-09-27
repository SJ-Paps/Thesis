using System;
using UnityEngine;

public abstract class Trigger2D : MonoBehaviour {

    public event Action<Collider2D> onEntered;
    public event Action<Collider2D> onExited;
    public event Action<Collider2D> onStay;

    public Collider2D InnerCollider { get; protected set; }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (onEntered != null)
        {
            onEntered(collision);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (onExited != null)
        {
            onExited(collision);
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if(onStay != null)
        {
            onStay(collision);
        }
    }

    public abstract void ChangeSize(Vector2 size);
}
