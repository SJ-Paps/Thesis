using UnityEngine;
using System;
using SAM.FSM;
using System.Collections.Generic;

public abstract class Character : SJMonoBehaviour, IControllable<Character.Order>, IMortal
{
    public event Action<Collision2D> onCollisionEnter2D;
    public event Action<Collision2D> onCollisionStay2D;
    public event Action<Collider2D> onTriggerEnter2D;
    public event Action<Collider2D> onTriggerExit2D;

	public event Action<Order> onOrderReceived;
    public event Action onDetected;
    public event Action onDead;

    protected Blackboard blackboard;
    
    public bool IsHidden
    {
        get { return blackboard.isHidden; }
    }
    public bool IsMovingHorizontal
    {
        get { return blackboard.movingHorizontal; }
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
        get { return transform.right.x < 0; }
    }
    public float MovementVelocity
    {
        get { return movementVelocity; }
        set
        {
            if(value >= 0)
            {
                movementVelocity = value;
            }
        }
    }
	public bool IsPushing
    {
        get { return blackboard.isPushing; }
    }

	public Eyes Eyes
    {
        get { return eyes; }
    }

    public Transform HandPoint
    {
        get { return handPoint; }
    }

    [SerializeField]
    protected float movementVelocity = 1;

    [SerializeField]
    protected Eyes eyes;

    [SerializeField]
    protected Transform handPoint;

    public bool blockFacing;

    public enum State : byte
    {
        Alive,
        Dead,
        Idle,
        Moving,
        SlowingDown,
        Grounded,
        Jumping,
        Falling,
        Hidden,
        Attacking,
        Pushing
    }

    public enum Trigger : byte
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

    public enum Order : byte
    {
        OrderMoveLeft,
        OrderMoveRight,
        OrderStopMoving,
        OrderJump,
        OrderAttack,
        OrderHide,
        OrderPush
    }

    public class Blackboard
    {
        public bool isAlive;
        public bool isHidden;
        public bool isGrounded;
        public bool isPushing;
        public bool movingHorizontal;
    }

    protected bool enslaved;

    public bool Enslaved
    {
        get
        {
            return enslaved;
        }
    }

    protected Action<Collision2D> collisionCheckDeadlyDelegate;
    protected Action<Collider2D> triggerCheckDeadlyDelegate;

    public Animator Animator { get; protected set; }
    public Rigidbody2D RigidBody2D { get; protected set; }

    private FSM<State, Trigger> aliveFSM;

    [SerializeField]
    private CharacterAliveState alive;
    [SerializeField]
    private CharacterDeadState dead;

    protected List<Order> orders;

    protected float groundDetectionDistance = 0.03f;

    public Collider2D Collider { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        Animator = GetComponent<Animator>();
        RigidBody2D = GetComponent<Rigidbody2D>();

        collisionCheckDeadlyDelegate = CheckDeadly;
        triggerCheckDeadlyDelegate = CheckDeadly;

        onCollisionEnter2D += collisionCheckDeadlyDelegate;
        onTriggerEnter2D += triggerCheckDeadlyDelegate;

        blackboard = new Blackboard();

        orders = new List<Order>();

        Collider = GetComponent<Collider2D>();

        aliveFSM = new FSM<State, Trigger>();

        alive.InitializeState(aliveFSM, State.Alive, this, orders, blackboard);
        dead.InitializeState(aliveFSM, State.Dead, this, orders, blackboard);

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

    public virtual bool Die(DeadlyType deadly)
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

    public void Face(bool left)
    {
        if(!blockFacing && FacingLeft != left)
        {
            transform.Rotate(Vector3.up, 180);

            OnFacingChanged(FacingLeft);
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
        if(IsHidden)
        {
            SetOrder(Order.OrderHide);
        }
    }

    private void CheckDeadly(Collision2D collision)
    {
        if(collision.gameObject.layer == Reg.hostileDeadlyLayer || collision.gameObject.layer == Reg.generalDeadlyLayer)
        {
            Deadly deadly = collision.gameObject.GetComponent<Deadly>();

            Die(deadly.Type);
        }
    }

    private void CheckDeadly(Collider2D collider)
    {
        if (collider.gameObject.layer == Reg.hostileDeadlyLayer || collider.gameObject.layer == Reg.generalDeadlyLayer)
        {
            Deadly deadly = collider.GetComponent<Deadly>();

            if(deadly != null)
            {
                Die(deadly.Type);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(onCollisionEnter2D != null)
        {
            onCollisionEnter2D(collision);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(onCollisionStay2D != null)
        {
            onCollisionStay2D(collision);
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

    public virtual bool IsOnFloor(int layerMask)
    {
        Bounds bounds = Collider.bounds;
        float height = 0.05f;

        Vector2 leftPoint = new Vector2(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y);
        Vector2 rightPoint = new Vector2(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y);

        EditorDebug.DrawLine(leftPoint, new Vector3(rightPoint.x, rightPoint.y - height), Color.green);
        EditorDebug.DrawLine(rightPoint, new Vector3(leftPoint.x, leftPoint.y - height), Color.green);

        return Physics2D.Linecast(leftPoint, new Vector2(rightPoint.x, rightPoint.y - height), layerMask) ||
            Physics2D.Linecast(rightPoint, new Vector2(leftPoint.x, leftPoint.y - height), layerMask);

    }

    public virtual bool CheckWall(int layers)
    {
        Bounds bounds = Collider.bounds;

        float separation = 0.15f;
        float xDir = transform.right.x;

        Vector2 beginPoint = new Vector2(bounds.center.x + (xDir * bounds.extents.x), bounds.center.y - bounds.extents.y);
        Vector2 endPoint = new Vector2(beginPoint.x + (xDir * separation), bounds.center.y + bounds.extents.y);

        EditorDebug.DrawLine(beginPoint, endPoint, Color.green);

        return Physics2D.Linecast(beginPoint, endPoint, layers);
    }

    public virtual bool CheckFloorAhead(int layers)
    {
        Bounds bounds = Collider.bounds;

        float separation = 1f;
        float yDistance = 0.5f;
        float xDir = transform.right.x;

        Vector2 beginPoint = new Vector2(bounds.center.x + (xDir * bounds.extents.x), bounds.center.y - (bounds.extents.y / 2));
        Vector2 endPoint = new Vector2(beginPoint.x + (xDir * separation), beginPoint.y - yDistance);

        EditorDebug.DrawLine(beginPoint, endPoint, Color.green);

        return Physics2D.Linecast(beginPoint, endPoint, layers);
    }
}
