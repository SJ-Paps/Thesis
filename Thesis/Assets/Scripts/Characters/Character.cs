﻿using UnityEngine;
using System;
using SAM.FSM;
using System.Collections.Generic;

public abstract class Character : SJMonoBehaviour, IControllable<Character.Order>, IMortal
{
    [SerializeField]
    protected bool isPlayer;

    public bool IsPlayer
    {
        get
        {
            return isPlayer;
        }

        protected set
        {
            isPlayer = value;
        }
    }

    public event Action<Collision2D> onCollisionEnter2D;
	public event Action<Order> onOrderReceived;
    public event Action onDead;
    public bool isHiding = false;

    public enum State
    {
        Alive,
        Dead,
        Idle,
        Moving,
        Grounded,
        Jumping,
        Falling,
		Hidden,
        Attacking
    }

    public enum Trigger
    {
        Die,
        Move,
        Ground,
        Jump,
        Fall,
		Hide,
        Attack,
        StopAttacking,
        StopMoving,
        StopHiding
    }

    public enum Order
    {
        OrderMoveLeft,
        OrderMoveRight,
        OrderJump,
		OrderAttack,
        OrderHide
    }

    protected bool enslaved;

    public bool Enslaved
    {
        get
        {
            return enslaved;
        }
    }

    public bool IsAlive { get; protected set; }

    public Animator Animator { get; protected set; }

    private FSM<State, Trigger> aliveFSM;

    private CharacterAliveState alive;
    private CharacterDeadState dead;

    protected List<Order> orders;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();

        orders = new List<Order>();

        aliveFSM = new FSM<State, Trigger>();

        alive = new CharacterAliveState(aliveFSM, State.Alive, this, orders);
        dead = new CharacterDeadState(aliveFSM, State.Dead, this, orders);

        aliveFSM.AddState(alive);
        aliveFSM.AddState(dead);

        aliveFSM.MakeTransition(State.Alive, Trigger.Die, State.Dead);

        aliveFSM.StartBy(State.Alive);
    }

    protected virtual void Update()
    {
        aliveFSM.UpdateCurrentState();

        ClearOrders();
    }

    protected void AddStateMachineWhenAlive(FSM<State, Trigger> fsm)
    {
        alive.AddFSM(fsm);
    }

    protected void AddStateMachineWhenDead(FSM<State, Trigger> fsm)
    {
        dead.AddFSM(fsm);
    }

    public virtual bool Die(IDeadly deadly)
    {
        Die();

        return true;
    }

    protected bool Die()
    {
        aliveFSM.Trigger(Trigger.Die);

        IsAlive = false;

        if (onDead != null)
        {
            onDead();
        }

        return true;
    }

    public abstract void GetEnslaved();

    public virtual void SetOrder(Order order)
    {
        orders.Add(order);

        if(onOrderReceived != null)
        {
            onOrderReceived(order);
        }
    }

    protected void ClearOrders()
    {
        orders.Clear();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(onCollisionEnter2D != null)
        {
            onCollisionEnter2D(collision);
        }
    }

    void OnTriggerStay2D(Collider2D collider) 
    {
        if (collider.gameObject.layer == 8) 
        {
            isHiding = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 8)
        {
            isHiding = false;
        }
    }
}
