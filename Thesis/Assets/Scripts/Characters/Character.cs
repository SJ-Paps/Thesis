using UnityEngine;
using System;

public abstract class Character : SJMonoBehaviour, IControllable<Character.Order>
{
    public event Action<Collision2D> onCollisionEnter2D;

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

    public struct ChangedStateEventArgs
    {

    }

    protected bool enslaved;

    public bool Enslaved
    {
        get
        {
            return enslaved;
        }
    }

    public abstract void GetEnslaved();

    public abstract void SetOrder(Order order);

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(onCollisionEnter2D != null)
        {
            onCollisionEnter2D(collision);
        }
    }
}
