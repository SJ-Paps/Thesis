using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Tribal : Character, IHandOwner, IDamagable, ISeer
{
    public enum State
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
        CheckingForLedges,
        Hanging,
        Climbing,
        HangingLedge,
        HangingRope,
        HangingStair,
        SwitchingActivables,
        Waiting,
        UsingWeapon,
        Throwing,
        Shocking,
        Collecting,
        Droping,
        Activating,
    }

    public class Blackboard
    {
        public List<IActivable> CurrentFrameActivables { get; private set; }

        public RaycastHit2D ledgeCheckHit;

        public Blackboard()
        {
            CurrentFrameActivables = new List<IActivable>();
        }
    }

    public static readonly AnimatorParameterId TrotAnimatorTrigger = new AnimatorParameterId("Move");
    public static readonly AnimatorParameterId RunAnimatorTrigger = new AnimatorParameterId("Move");
    public static readonly AnimatorParameterId WalkAnimatorTrigger = new AnimatorParameterId("Move");
    public static readonly AnimatorParameterId IdleAnimatorTrigger = new AnimatorParameterId("Idle");
    public static readonly AnimatorParameterId GroundAnimatorTrigger = new AnimatorParameterId("Ground");
    public static readonly AnimatorParameterId FallAnimatorTrigger = new AnimatorParameterId("Fall");
    public static readonly AnimatorParameterId JumpAnimatorTrigger = new AnimatorParameterId("Jump");
    public static readonly AnimatorParameterId HideAnimatorTrigger = new AnimatorParameterId("Idle");
    public static readonly AnimatorParameterId ClimbLedgeAnimatorTrigger = new AnimatorParameterId("ClimbLedge");

    public event Action onDead;

    public const float activableDetectionOffset = 0.2f;

    public Animator Animator { get; protected set; }
    public Rigidbody2D RigidBody2D { get; protected set; }

    public SJCollider2D Collider { get; protected set; }
    
    protected Hand hand;

    [SerializeField]
    private float maxMovementVelocity, acceleration;

    public PercentageReversibleNumber MaxVelocity { get; protected set; }

    public float Acceleration
    {
        get
        {
            return acceleration;
        }
    }

    [SerializeField]
    private float jumpMaxHeight, jumpAcceleration;

    public float JumpMaxHeight
    {
        get
        {
            return jumpMaxHeight;
        }
    }

    public float JumpAcceleration
    {
        get
        {
            return jumpAcceleration;
        }
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

    [SerializeField]
    private TribalHSMStateAsset hsmAsset;

    protected TribalHSMState hsm;

    protected Blackboard blackboard;

    private EyeCollection eyes;

    protected override void Awake()
    {
        Animator = GetComponent<Animator>();
        RigidBody2D = GetComponent<Rigidbody2D>();
        Collider = GetComponent<SJCollider2D>();

        hand = GetComponentInChildren<Hand>();
        hand.PropagateOwnerReference(this);

        base.Awake();
        
        blackboard = new Blackboard();

        hsm = TribalHSMStateAsset.BuildFromAsset<TribalHSMState>(hsmAsset, this, blackboard);

        MaxVelocity = new PercentageReversibleNumber(maxMovementVelocity);

        eyes = new EyeCollection(GetComponentsInChildren<Eyes>());
    }

    protected override void Start()
    {
        base.Start();

        hsm.Enter();
    }

    protected override void Update()
    {
        base.Update();

        blackboard.CurrentFrameActivables.Clear();

        hsm.Update();
    }

    public Hand GetHand()
    {
        return hand;
    }

    public EyeCollection GetEyes()
    {
        return eyes;
    }

    public virtual void TakeDamage(float damage, DamageType damageType)
    {
        SendOrder(Order.Die);

        if (onDead != null)
        {
            onDead();
        }
    }

    protected override void ProcessOrder(Character.Order order)
    {
        hsm.SendEvent(order);
    }

    public override bool ShouldBeSaved()
    {
        return true;
    }

    protected override void OnSave(SaveData data)
    {
        data.AddValue("x", transform.position.x);
        data.AddValue("y", transform.position.y);
        
    }

    protected override void OnLoad(SaveData data)
    {
        /*transform.position = new Vector2(data.GetAs<float>("x"), data.GetAs<float>("y"));

        if(data.ContainsValue("collectableGUID"))
        {
            Guid objGuid = new Guid(data.GetAs<string>("collectableGUID"));

            CurrentCollectableObject = GetSJMonobehaviourSaveableBySaveGUID<CollectableObject>(objGuid);

            CurrentCollectableObject.Collect(this);
        }*/
    }

    public override void PostLoadCallback(SaveData data)
    {
        transform.position = new Vector2(data.GetAs<float>("x"), data.GetAs<float>("y"));

        if(data.ContainsValue("collectableGUID"))
        {
            Guid objGuid = new Guid(data.GetAs<string>("collectableGUID"));
        }
    }

    
}

public static class TribalExtensions
{
    public static MovableObject CheckForMovableObject(this Tribal tribal)
    {
        float checkMovableObjectDistanceX = 0.2f;

        float xDirection;

        if (tribal.transform.right.x >= 0)
        {
            xDirection = 1;
        }
        else
        {
            xDirection = -1;
        }

        Bounds tribalBounds = tribal.Collider.bounds;

        return SJUtil.FindActivable<MovableObject, Tribal>(new Vector2(tribalBounds.center.x + (tribalBounds.extents.x * xDirection),
                                                                  tribalBounds.center.y - tribalBounds.extents.y / 3),
                                                     new Vector2(checkMovableObjectDistanceX, tribalBounds.extents.y), tribal.transform.eulerAngles.z);
    }

    public static bool CheckWall(this Tribal tribal)
    {
        Bounds bounds = tribal.Collider.bounds;

        float separation = 0.15f;
        float xDir = tribal.transform.right.x;

        Vector2 beginPoint = new Vector2(bounds.center.x + (xDir * bounds.extents.x), bounds.center.y - bounds.extents.y);
        Vector2 endPoint = new Vector2(beginPoint.x + (xDir * separation), bounds.center.y + bounds.extents.y);

        EditorDebug.DrawLine(beginPoint, endPoint, Color.green);

        return Physics2D.Linecast(beginPoint, endPoint, Reg.walkableLayerMask);
    }

    public static bool CheckFloorAhead(this Tribal tribal)
    {
        Bounds bounds = tribal.Collider.bounds;

        float separation = 1f;
        float yDistance = 0.5f;
        float xDir = tribal.transform.right.x;

        Vector2 beginPoint = new Vector2(bounds.center.x + (xDir * bounds.extents.x), bounds.center.y - (bounds.extents.y / 2));
        Vector2 endPoint = new Vector2(beginPoint.x + (xDir * separation), beginPoint.y - yDistance);

        EditorDebug.DrawLine(beginPoint, endPoint, Color.green);

        return Physics2D.Linecast(beginPoint, endPoint, Reg.walkableLayerMask);
    }
}
