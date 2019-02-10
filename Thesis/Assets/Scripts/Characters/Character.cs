using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : SJMonoBehaviourSaveable, IControllable<Character.Trigger>, IMortal
{
    public enum State : byte
    {
        Base,
        Alive,
        Dead,
        Idle,
        Trotting,
        SlowingDown,
        Grounded,
        OnAir,
        Jumping,
        Falling,
        Hidden,
        Attacking,
        Pushing,
        Grappling,
        Standing,
        Ducking,
        ChoiceIdleOrMoving,
        ChoiceJumpingOrFalling,
        Walking,
        Running,
        Moving,
        ChoiceWalkingOrTrottingOrRunning,
        CheckingForPushables,
        ChoiceMovingByWillOrBraking,
        Braking,
    }

    public enum Trigger : byte
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
        Run
    }

    public class Blackboard
    {
        public MovableObject toPushMovableObject;
        public Hide toHidePlace;
    }

    public event Action<Collision2D> onCollisionEnter2D
    {
        add
        {
            Collider.onEnteredCollision += value;
        }

        remove
        {
            Collider.onEnteredCollision -= value;
        }
    }

    public event Action<Collision2D> onCollisionStay2D
    {
        add
        {
            Collider.onStayCollision += value;
        }

        remove
        {
            Collider.onStayCollision -= value;
        }
    }

    public event Action<Collider2D> onTriggerEnter2D
    {
        add
        {
            Collider.onEnteredTrigger += value;
        }

        remove
        {
            Collider.onEnteredTrigger -= value;
        }
    }

    public event Action<Collider2D> onTriggerExit2D
    {
        add
        {
            Collider.onExitedTrigger += value;
        }

        remove
        {
            Collider.onExitedTrigger -= value;
        }
    }

    public event Action onFixedUpdate;

	public event Action<Trigger> onOrderReceived;
    public event Action onDetected;
    public event Action onDead;

    protected Blackboard blackboard;
    
    public bool IsFacingLeft { get; private set; }

	public Eyes Eyes
    {
        get { return eyes; }
    }

    public Transform HandPoint
    {
        get { return handPoint; }
    }

    public virtual bool CanMove { get; }

    [SerializeField]
    private float maxMovementVelocity, acceleration;

    private PercentageReversibleNumber velocity;

    public double MaxMovementVelocity
    {
        get
        {
            return velocity.CurrentValue;
        }
    }

    public float Acceleration
    {
        get
        {
            return acceleration;
        }
    }

    [SerializeField]
    protected Eyes eyes;

    [SerializeField]
    protected Transform handPoint;
    
    public bool blockFacing;
    

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

    protected Queue<Trigger> orders;

    public SJCollider2D Collider { get; protected set; }


    [SerializeField]
    private CharacterHSMStateAsset hsmAsset;

    protected CharacterHSMState hsm;

    protected override void Awake()
    {
        base.Awake();

        Animator = GetComponent<Animator>();
        RigidBody2D = GetComponent<Rigidbody2D>();

        blackboard = new Blackboard();

        orders = new Queue<Trigger>();

        velocity = new PercentageReversibleNumber(maxMovementVelocity);

        Collider = GetComponent<SJCollider2D>();

        collisionCheckDeadlyDelegate = CheckDeadly;
        triggerCheckDeadlyDelegate = CheckDeadly;

        onCollisionEnter2D += collisionCheckDeadlyDelegate;
        onTriggerEnter2D += triggerCheckDeadlyDelegate;

        hsm = (CharacterHSMState)CharacterHSMStateAsset.BuildFromAsset(hsmAsset);

        hsm.PropagateOwnerReference(this);
        hsm.PropagateBlackboardReference(blackboard);

        hsm.Enter();
    }

    protected virtual void Update()
    {
        SendOrdersToStates();

        hsm.Update();
    }

    protected virtual void FixedUpdate()
    {
        if(onFixedUpdate != null)
        {
            onFixedUpdate();
        }
    }

    public bool IsOnState(State state)
    {
        return hsm.IsOnState(state);
    }

    private void SendOrdersToStates()
    {
        while(orders.Count > 0)
        {
            hsm.SendEvent(orders.Dequeue());
        }
    }



    public virtual bool Die(DeadlyType deadly)
    {
        if(IsOnState(State.Alive))
        {
            Collider.enabled = false;

            Die();

            return true;
        }

        return false;
    }

    protected bool Die()
    {

        if (onDead != null)
        {
            onDead();
        }

        return true;
    }

    public abstract void GetEnslaved();

    public virtual void SetOrder(Trigger order)
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

            IsFacingLeft = left;

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

    public int AddVelocityConstraintByPercentageAndGetConstraintId(float percentage)
    {
        return velocity.AddPercentageConstraint(percentage);
    }

    public void RemoveVelocityConstraintById(int id)
    {
        velocity.ReversePercentageConstraint(id);
    }

}
