using SJ.GameEntities;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : SaveableGameEntity, IControllable<Character.OrderEvent>
{
    public readonly struct OrderEvent
    {
        public readonly Order Order;
        public readonly float RelativeFloatValue;

        public OrderEvent(Order order, float relativeFloatValue)
        {
            Order = order;
            RelativeFloatValue = relativeFloatValue;
        }
    }

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
        HangLadder,
        StopHanging,
        Activate,
        Throw,
        Shock,
        Drop,
        SwitchActivables,
        Collect,
        FinishAction,
        HangWall,
    }

    public event Action onFixedUpdate;

    public event Action<OrderEvent> OnOrderReceived;
    public event Action onDetected;


    public bool IsFacingLeft
    {
        get
        {
            return transform.right.x < 0;
        }
    }

    public int FacingDirection
    {
        get
        {
            if (transform.right.x > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
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

    [SerializeField]
    private SJHSMStateAsset hsmAsset;

    private CharacterHSMState hsm;

    public Blackboard Blackboard { get; private set; }

    protected override void SJAwake()
    {
        orders = new Queue<Order>();

        Blackboard = new Blackboard();

        hsm = SJHSMStateAsset.BuildFromAsset<CharacterHSMState>(hsmAsset, this);

        base.SJAwake();
    }

    protected override void SJStart()
    {
        base.SJStart();

        hsm.Enter();
    }

    protected override void SJUpdate()
    {
        while (orders.Count > 0)
        {
            hsm.SendEvent(orders.Dequeue());
        }

        hsm.Update();
    }

    protected override void SJFixedUpdate()
    {
        if (onFixedUpdate != null)
        {
            onFixedUpdate();
        }
    }

    public abstract void GetEnslaved();

    public virtual void SendOrder(OrderEvent order)
    {
        orders.Enqueue(order.Order);

        OnOrderReceived?.Invoke(order);
    }

    public void SendOrder(Order order)
    {
        SendOrder(new OrderEvent(order, 1));
    }

    public void Face(bool left)
    {
        if (!blockFacing && IsFacingLeft != left)
        {
            transform.Rotate(Vector3.up, 180);

            OnFacingChanged(IsFacingLeft);
        }
    }

    public void Face(int direction)
    {
        if (direction >= 0)
        {
            Face(false);
        }
        else
        {
            Face(true);
        }
    }

    protected virtual void OnFacingChanged(bool facingLeft)
    {
    }

    public void NotifyDetection()
    {
        OnDetected();

        if (onDetected != null)
        {
            onDetected();
        }
    }

    protected virtual void OnDetected()
    {

    }
}