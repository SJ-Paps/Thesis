using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : SJMonoBehaviourSaveable, IControllable<Character.Trigger>, IDamagable, ISeer
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
        CheckingForLedges,
        Hanging,
        Climbing,
        HangingLedge,
        HangingRope,
        HangingStair,
        CheckActivables,
        Waiting,
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
        Run,
        ClimbUp,
        ClimbDown,
        HangLedge,
        HangRope,
        HangStair,
        StopHanging,
        Activate
    }

    public class Blackboard
    {
        public MovableObject toPushMovableObject;
        public Hide toHidePlace;
        public RaycastHit2D ledgeCheckHit;
    }

    public event Action onFixedUpdate;

	public event Action<Trigger> onOrderReceived;
    public event Action onDetected;
    public event Action onDead;

    protected Blackboard blackboard;
    
    public bool IsFacingLeft { get; private set; }

    [SerializeField]
    protected EyeCollection eyes;
    
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

    protected Queue<Trigger> orders;


    [SerializeField]
    private CharacterHSMStateAsset hsmAsset;

    protected CharacterHSMState hsm;

    protected override void Awake()
    {
        base.Awake();

        

        blackboard = new Blackboard();

        orders = new Queue<Trigger>();
        

        hsm = CharacterHSMStateAsset.BuildFromAsset<CharacterHSMState>(hsmAsset, this, blackboard);

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

    public EyeCollection GetEyes()
    {
        return eyes;
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



    public virtual void TakeDamage(float damage, DamageType damageType)
    {
        hsm.SendEvent(Trigger.Die);

        if(onDead != null)
        {
            onDead();
        }
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

    /*private void CheckDeadly(Collision2D collision)
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
    }*/

}
