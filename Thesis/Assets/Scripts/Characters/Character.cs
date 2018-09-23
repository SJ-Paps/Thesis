using UnityEngine;
using System;
using SAM.FSM;
using System.Collections.Generic;

public abstract class Character : SJMonoBehaviour, IControllable<Character.Order>, IMortal
{
    public event Action<Collision2D> onCollisionEnter2D;
    public event Action<Collider2D> onTriggerEnter2D;
    public event Action<Collider2D> onTriggerExit2D;
	public event Action<Order> onOrderReceived;
    public event Action onDead;
    protected Blackboard blackboard;

    
    public bool IsHidden
    {
        get { return blackboard.isHidden; }
    }
    
    public bool IsGrounded
    {
        get { return blackboard.isGrounded; }
    }
    public bool IsAlive
    {
        get { return blackboard.isAlive; }
    }
    public bool FacingLeft
    {
        get { return facingLeft; }
    }
    public float MovementVelocity
    {
        get { return movementVelocity; }
    }
	public bool IsPushing
    {
        get { return blackboard.isPushing; }
    }

    [SerializeField]
    private bool facingLeft;

    [SerializeField]
    protected float movementVelocity = 1;

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

        Attacking,
        Pushing
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

        StopHiding,
        StopPushing,
        Push
    }

    public enum Order
    {
        OrderMoveLeft,
        OrderMoveRight,
        OrderJump,
        OrderAttack,
        OrderAction
    }

    public class Blackboard
    {

        public bool isAlive;
        public bool isHidden;
        public bool isGrounded;
        public bool isPushing;
    }

    protected bool enslaved;

    public bool Enslaved
    {
        get
        {
            return enslaved;
        }
    }
    public Animator Animator { get; protected set; }

    private FSM<State, Trigger> aliveFSM;

    private CharacterAliveState alive;
    private CharacterDeadState dead;

    protected List<Order> orders;

    protected float groundDetectionDistance = 0.03f;
    protected float colliderVerticalDiameter;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();

        blackboard = new Blackboard();

        orders = new List<Order>();

        colliderVerticalDiameter = GetComponent<Collider2D>().bounds.size.y / 2;

        aliveFSM = new FSM<State, Trigger>();

        alive = new CharacterAliveState(aliveFSM, State.Alive, this, orders, blackboard);
        dead = new CharacterDeadState(aliveFSM, State.Dead, this, orders, blackboard);

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

        if (onOrderReceived != null)
        {
            onOrderReceived(order);
        }
    }

    protected void ClearOrders()
    {
        orders.Clear();
    }

    public bool CheckIsOnFloor(int layerMask)
    {
        float xRay = transform.position.x;
        float yRay = transform.position.y - colliderVerticalDiameter;

        Vector2 origin = new Vector2(xRay, yRay);

        RaycastHit2D groundDetection = Physics2D.Linecast(origin, origin + (Vector2.down * groundDetectionDistance), layerMask);

        EditorDebug.DrawLine(origin, origin + (Vector2.down * groundDetectionDistance), Color.green);

        return groundDetection.transform != null;
    }

    public void Face(bool left)
    {
        if(facingLeft != left)
        {
            facingLeft = left;

            OnFacingChanged(facingLeft);
        }
    }

    protected virtual void OnFacingChanged(bool facingLeft)
    {
        transform.Rotate(Vector3.up, 180);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(onCollisionEnter2D != null)
        {
            onCollisionEnter2D(collision);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (onTriggerEnter2D != null) 
        {
            onTriggerEnter2D(collider);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (onTriggerExit2D != null)
        {
            onTriggerExit2D(collider);
        }
    }
}
