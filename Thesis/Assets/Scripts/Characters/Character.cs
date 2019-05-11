using System;
using System.Collections.Generic;
using UnityEngine;
using Paps.StateMachines;

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

    public class Blackboard : global::Blackboard
    {

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

    public int FacingDirection
    {
        get
        {
            if(transform.right.x > 0)
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

    protected Blackboard blackboard;

    [SerializeField]
    private SJHSMStateAsset hsmAsset;

    private CharacterHSMState hsm;

    protected override void Awake()
    {
        base.Awake();

        orders = new Queue<Order>();

        hsm = SJHSMStateAsset.BuildFromAsset<CharacterHSMState>(hsmAsset, this, blackboard);

    }

    protected override void Start()
    {
        base.Start();

        hsm.Enter();
    }

    protected virtual void Update()
    {
        while(orders.Count > 0)
        {
            hsm.SendEvent(orders.Dequeue());
        }

        hsm.Update();
    }

    protected virtual void FixedUpdate()
    {
        if(onFixedUpdate != null)
        {
            onFixedUpdate();
        }
    }
    
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

    public void Face(int direction)
    {
        if(direction >= 0)
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

        if(onDetected != null)
        {
            onDetected();
        }
    }

    protected virtual void OnDetected()
    {
        
    }

}