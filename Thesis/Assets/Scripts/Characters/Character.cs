using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : SJMonoBehaviourSaveable, IControllable<Character.Order>
{
    public enum Order : byte
    {
        Die,
        MoveLeft,
        MoveRight,
        Ground,
        Jump,
        Fall,
        Hide,
        Attack,
        StopAttacking,
        StopMoving,
        StopHiding,
        StopPushing,
        Push,
        Grapple,
        StandUp,
        Duck,
        Move,
        Walk,
        Trot,
        Run,
        ClimbUp,
        ClimbDown,
        HangLedge,
        HangRope,
        HangStair,
        StopHanging,
        Activate,
        Throw,
        Shock,
        Drop,
        SwitchActivables,
        Collect,
    }

    public event Action onFixedUpdate;

	public event Action<Order> onOrderReceived;
    public event Action onDetected;
    
    
    public bool IsFacingLeft
    {
        get
        {
            return transform.right.x < 0;
        }
    }

    
    
    [HideInInspector]
    public bool blockFacing;
    

    protected bool enslaved;

    public bool Enslaved
    {
        get
        {
            return enslaved;
        }
    }

    protected Queue<Order> orders;
    

    protected override void Awake()
    {
        base.Awake();

        orders = new Queue<Order>();

        
        
    }

    protected virtual void Update()
    {
        while(orders.Count > 0)
        {
            ProcessOrder(orders.Dequeue());
        }
    }

    protected virtual void FixedUpdate()
    {
        if(onFixedUpdate != null)
        {
            onFixedUpdate();
        }
    }

    protected abstract void ProcessOrder(Character.Order order);
    
    public abstract void GetEnslaved();

    public virtual void SendOrder(Order order)
    {
        orders.Enqueue(order);

        if (onOrderReceived != null)
        {
            onOrderReceived(order);
        }
    }

    public void Face(bool left)
    {
        if(!blockFacing && IsFacingLeft != left)
        {
            transform.Rotate(Vector3.up, 180);

            OnFacingChanged(IsFacingLeft);
        }
    }

    protected virtual void OnFacingChanged(bool facingLeft)
    {
    }

    public void NotifyDetection()
    {
        OnDetected();

        if(onDetected != null)
        {
            onDetected();
        }
    }

    protected virtual void OnDetected()
    {
        
    }

}