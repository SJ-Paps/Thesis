using UnityEngine;
using System;

public abstract class Character : SJMonoBehaviour, IControllable
{
    public event Action<Collision2D> onCollisionEnter2D;
    public event Action onFixedUpdate;

    public enum State
    {
        Idle,
        Moving,
        Grounded,
        Jumping,
        Falling,
    }

    public enum Trigger
    {
        StopMoving,
        Move,
        Ground,
        Jump,
        Fall
    }

    public enum Order
    {
        OrderMoveLeft,
        OrderMoveRight,
        OrderJump
    }

    protected bool enslaved;

    public bool Enslaved
    {
        get
        {
            return enslaved;
        }
    }

    public abstract event ChangeControlDelegate onChangeControl;

    public abstract void GetEnslaved();

    public abstract void SetOrder(Order e);

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(onCollisionEnter2D != null)
        {
            onCollisionEnter2D(collision);
        }
    }

    void FixedUpdate()
    {
        if(onFixedUpdate != null)
        {
            onFixedUpdate();
        }
    }
}
