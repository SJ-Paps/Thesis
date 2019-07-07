using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations;

public abstract class Tribal : Character, IDamagable, ISeer
{
    public enum State : byte
    {
        Base,
        Alive,
        Dead,
        Idle,
        Trotting,
        HangingWall,
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
        HangingLadder,
        SwitchingActivables,
        Waiting,
        UsingWeapon,
        Throwing,
        Shocking,
        Collecting,
        Droping,
        Activating,
        ChoiceCollectingOrDropingOrThrowingOrActivatingOrAttacking,
        ChoiceHangingLadderOrRopeOrWallOrLedge,
    }
    
    public class TribalSaveData
    {
        public int unNumero;
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

    public static readonly string rightHandEquipmentSlotIdentifier = "right_hand";

    public event Action onDead;

    public event Action<TribalSaveData> onSaving;
    public event Action<TribalSaveData> onLoading;

    #region CONFIGURATION

    [SerializeField]
    private float movementVelocity, movementAcceleration, jumpMaxHeight, jumpAcceleration, jumpForceFromLadder, climbForce;

    #endregion

    public PercentageReversibleNumber MovementVelocity { get; protected set; }
    public PercentageReversibleNumber MovementAcceleration { get; protected set; }
    public PercentageReversibleNumber JumpMaxHeight { get; protected set; }
    public PercentageReversibleNumber JumpAcceleration { get; protected set; }
    public PercentageReversibleNumber JumpForceFromLadder { get; protected set; }
    public PercentageReversibleNumber ClimbForce { get; protected set; }

    public Animator Animator { get; protected set; }
    public Rigidbody2D RigidBody2D { get; protected set; }

    public SJCapsuleCollider2D Collider { get; protected set; }

    private EyeCollection eyes;

    protected override void Awake()
    {
        Animator = GetComponent<Animator>();
        RigidBody2D = GetComponent<Rigidbody2D>();
        Collider = GetComponent<SJCapsuleCollider2D>();

        MovementVelocity = new PercentageReversibleNumber(movementVelocity);
        MovementAcceleration = new PercentageReversibleNumber(movementAcceleration);
        JumpMaxHeight = new PercentageReversibleNumber(jumpMaxHeight);
        JumpAcceleration = new PercentageReversibleNumber(jumpAcceleration);
        JumpForceFromLadder = new PercentageReversibleNumber(jumpForceFromLadder);
        ClimbForce = new PercentageReversibleNumber(climbForce);

        eyes = new EyeCollection(GetComponentsInChildren<Eyes>());

        base.Awake();
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

    protected override object GetSaveData()
    {
        TribalSaveData saveData = new TribalSaveData() { unNumero = UnityEngine.Random.Range(0, 100) };

        saveData.unNumero = 3;

        if(onSaving != null)
        {
            onSaving(saveData);
        }

        return saveData;
    }

    protected override void LoadSaveData(object data)
    {
        TribalSaveData saveData = (TribalSaveData)data;

        Debug.Log(saveData.unNumero);

        if(onLoading != null)
        {
            onLoading(saveData);
        }
    }

    public override void PostLoadCallback(object dataSave)
    {
        
    }

    public override void PostSaveCallback()
    {
        
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

    public static void FindActivables(this Tribal tribal, List<IActivable> activables)
    {
        Bounds ownerBounds = tribal.Collider.bounds;

        int xDirection;

        if (tribal.IsFacingLeft)
        {
            xDirection = -1;
        }
        else
        {
            xDirection = 1;
        }

        //guardo los activables en la lista del blackboard
        SJUtil.FindActivables(new Vector2(ownerBounds.center.x + (ownerBounds.extents.x * xDirection), ownerBounds.center.y),
                                    new Vector2(ownerBounds.extents.x * 2, ownerBounds.size.y * 2), tribal.transform.eulerAngles.z, activables);
    }

    public static void DisplayCollectObject(this Tribal tribal, CollectableObject collectableObject)
    {

    }

    public static void DisplayEquipObject(this Tribal tribal, Transform hand, EquipableObject equipableObject)
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = hand;
        source.weight = 1;

        equipableObject.ParentConstraint.AddSource(source);

        equipableObject.ParentConstraint.constraintActive = true;

        Vector3 offset = new Vector3(equipableObject.HandlePoint.localPosition.x * equipableObject.transform.localScale.x,
                                     equipableObject.HandlePoint.localPosition.y * equipableObject.transform.localScale.y);

        equipableObject.ParentConstraint.SetTranslationOffset(0, -offset);
    }

    public static void DisplayDropObject(this Tribal tribal, CollectableObject collectableObject)
    {
        if(collectableObject is EquipableObject equipable)
        {
            equipable.ParentConstraint.RemoveSource(0);

            equipable.ParentConstraint.constraintActive = false;
        }
        else
        {
            //si no es equipable
        }
    }

    public static void DisplayThrowObject(this Tribal tribal, EquipableObject throwableObject)
    {
        //temporal
        if(throwableObject is IThrowable)
        {
            DisplayDropObject(tribal, throwableObject);
        }
    }
}
