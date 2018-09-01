using UnityEngine;
using System;
using SAM.FSM;

public abstract class Character : SJMonoBehaviour, IControllable<Character.Order>
{
    public event Action<Collision2D> onCollisionEnter2D;
    public event Action<Character.Order> onReceiveOrder;
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
    }

    public enum Trigger
    {
        Die,
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

    public bool IsAlive { get; protected set; }

    private FSM<State, Trigger, ChangedStateEventArgs> aliveFSM;

    private CharacterContextState alive;
    private CharacterContextState dead;

    protected virtual void Awake()
    {
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

        if (Input.GetKey(KeyCode.Q))
        {
            aliveFSM.Trigger(Trigger.Die);
        }

        CheckIsAlive();
    }

    protected void AddStateMachineWhenAlive(FSM<State, Trigger, ChangedStateEventArgs> fsm)
    {
        alive.AddFSM(fsm);
    }

    protected void AddStateMachineWhenDead(FSM<State, Trigger, ChangedStateEventArgs> fsm)
    {
        dead.AddFSM(fsm);
    }

    public abstract void GetEnslaved();

    public virtual void SetOrder(Order order)
    {
        if(onReceiveOrder != null)
        {
            onReceiveOrder(order);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(onCollisionEnter2D != null)
        {
            onCollisionEnter2D(collision);
        }
    }

    private void CheckIsAlive()
    {
        if (aliveFSM.CurrentState == dead && IsAlive)
        {
            IsAlive = false;

            if (onDead != null)
            {
                onDead();
            }
        }
    }
}
