using System;
using UnityEngine;

public abstract class Tribal : Character, ISeer
{
    public static readonly AnimatorParameterId TrotAnimatorTrigger = new AnimatorParameterId("Move");
    public static readonly AnimatorParameterId RunAnimatorTrigger = new AnimatorParameterId("Move");
    public static readonly AnimatorParameterId WalkAnimatorTrigger = new AnimatorParameterId("Move");
    public static readonly AnimatorParameterId IdleAnimatorTrigger = new AnimatorParameterId("Idle");
    public static readonly AnimatorParameterId GroundAnimatorTrigger = new AnimatorParameterId("Ground");
    public static readonly AnimatorParameterId FallAnimatorTrigger = new AnimatorParameterId("Fall");
    public static readonly AnimatorParameterId JumpAnimatorTrigger = new AnimatorParameterId("Jump");
    public static readonly AnimatorParameterId HideAnimatorTrigger = new AnimatorParameterId("Idle");
    public static readonly AnimatorParameterId ClimbLedgeAnimatorTrigger = new AnimatorParameterId("ClimbLedge");

    public const float activableDetectionOffset = 0.2f;

    [SerializeField]
    protected CollectableObject currentCollectableObject;

    public CollectableObject CurrentCollectableObject
    {
        get
        {
            return currentCollectableObject;
        }

        protected set
        {
            currentCollectableObject = value;
        }
    }

    public Animator Animator { get; protected set; }
    public Rigidbody2D RigidBody2D { get; protected set; }

    public SJCollider2D Collider { get; protected set; }

    public Transform HandPoint
    {
        get { return handPoint; }
    }

    [SerializeField]
    protected Eyes eyes;

    [SerializeField]
    protected Transform handPoint;

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


    protected override void Awake()
    {
        Animator = GetComponent<Animator>();
        RigidBody2D = GetComponent<Rigidbody2D>();
        Collider = GetComponent<SJCollider2D>();

        base.Awake();

        if(CurrentCollectableObject != null)
        {
            CurrentCollectableObject.Collect(this);
        }

        MaxVelocity = new PercentageReversibleNumber(maxMovementVelocity);
    }

    public Eyes GetEyes()
    {
        return eyes;
    }

    public override bool ShouldBeSaved()
    {
        return true;
    }

    protected override void OnSave(SaveData data)
    {
        data.AddValue("x", transform.position.x);
        data.AddValue("y", transform.position.y);
        

        if(CurrentCollectableObject != null)
        {
            data.AddValue("collectableGUID", CurrentCollectableObject.saveGUID);
        }
        
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

            CurrentCollectableObject = GetSJMonobehaviourSaveableBySaveGUID<CollectableObject>(objGuid);

            CurrentCollectableObject.Collect(this);
        }
    }


    public T CheckForActivableObject<T>() where T : class, IActivable<Tribal>
    {
        float xDirection;

        if (transform.right.x >= 0)
        {
            xDirection = 1;
        }
        else
        {
            xDirection = -1;
        }

        return SJUtil.FindActivable<T, Tribal>(new Vector2(Collider.bounds.center.x + (Collider.bounds.extents.x * xDirection), 0),
                                        new Vector2(Collider.bounds.extents.x / 2, Collider.bounds.extents.y / 2), transform.eulerAngles.z);
    }

    public T CheckForActivableObject<T>(Vector2 center, Vector2 detectionBoxSize) where T : class, IActivable<Tribal>
    {
        T activable = SJUtil.FindActivable<T, Tribal>(center, detectionBoxSize, transform.eulerAngles.z);

        return activable;
    }

    public MovableObject CheckForMovableObject()
    {
        float checkMovableObjectDistanceX = 0.2f;

        float xDirection;

        if (transform.right.x >= 0)
        {
            xDirection = 1;
        }
        else
        {
            xDirection = -1;
        }

        return CheckForActivableObject<MovableObject>(new Vector2(Collider.bounds.center.x + (Collider.bounds.extents.x * xDirection),
                                                                  Collider.bounds.center.y - Collider.bounds.extents.y / 3),
                                                     new Vector2(checkMovableObjectDistanceX, Collider.bounds.extents.y));
    }
}
