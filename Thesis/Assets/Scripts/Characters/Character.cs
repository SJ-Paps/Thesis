using UnityEngine;
using System;
using SAM.FSM;

public abstract class Character : SJMonoBehaviour, IControllable<Character.Order>, IMortal
{
    public event Action<Collision2D> onCollisionEnter2D;
    public event Action<Character.Order> onProcessOrder;
    public event Action onDead;

    public enum State
    {
        Alive,
        Dead,
        Idle,
        Moving,
        Grounded,
        Jumping,
        Falling,
        Attacking
    }

    public enum Trigger
    {
        Die,
        StopMoving,
        Move,
        Ground,
        Jump,
        Fall,
        Attack,
        StopAttack
    }

    public enum Order
    {
        OrderMoveLeft,
        OrderMoveRight,
        OrderJump,
        OrderAttack
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

    public bool IsAlive { get; protected set; }

    public Animator Animator { get; protected set; }

    private FSM<State, Trigger, ChangedStateEventArgs> aliveFSM;

    private CharacterContextState alive;
    private CharacterContextState dead;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();

        aliveFSM = new FSM<State, Trigger, ChangedStateEventArgs>();

        alive = new CharacterContextState(aliveFSM, State.Alive, this);
        dead = new CharacterContextState(aliveFSM, State.Dead, this);

        aliveFSM.AddState(alive);
        aliveFSM.AddState(dead);

        aliveFSM.MakeTransition(State.Alive, Trigger.Die, State.Dead);

        aliveFSM.StartBy(State.Alive);
    }

    protected virtual void Update()
    {
        aliveFSM.UpdateCurrentState();
    }

    protected void AddStateMachineWhenAlive(FSM<State, Trigger, ChangedStateEventArgs> fsm)
    {
        alive.AddFSM(fsm);
    }

    protected void AddStateMachineWhenDead(FSM<State, Trigger, ChangedStateEventArgs> fsm)
    {
        dead.AddFSM(fsm);
    }

    public virtual bool Die(IDeadly deadly)
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
        if(onProcessOrder != null)
        {
            onProcessOrder(order);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(onCollisionEnter2D != null)
        {
            onCollisionEnter2D(collision);
        }
    }
}
