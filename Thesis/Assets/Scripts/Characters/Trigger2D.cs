using System;
using UnityEngine;

public class Trigger2D : MonoBehaviour {

    public event Action<Collider2D> onEntered;
    public event Action<Collider2D> onExited;
    public event Action<Collider2D> onStay;

    public BoxCollider2D InnerCollider { get; private set; }

    void Awake()
    {
        InnerCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (onEntered != null)
        {
            onEntered(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (onExited != null)
        {
            onExited(collision);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(onStay != null)
        {
            onStay(collision);
        }
    }

    public void ChangeSize(Vector2 size)
    {
        ((BoxCollider2D)InnerCollider).size = size;
    }
}
