using UnityEngine;
using System;

public class Turret : Robot, ISeer, IDamagable
{
    public enum State
    {
        Base,
        Alive,
        Dead,
        Moving,
        Idle,
        Charging,
        Attacking
    }

    public class Blackboard : global::Blackboard
    {

    }

    public event Action onDead;

    [SerializeField]
    protected Rigidbody2D head, body;

    public Rigidbody2D HeadRigidBody
    {
        get
        {
            return head;
        }
    }

    public Rigidbody2D BodyRigidBody
    {
        get
        {
            return body;
        }
    }

    public SJCollider2D HeadCollider { get; private set; }
    public SJCollider2D BodyCollider { get; private set; }

    [SerializeField]
    protected float chargeTimeBase;

    public PercentageReversibleNumber ChargeTime { get; protected set; }

    [SerializeField]
    protected float shootDistance;

    [SerializeField]
    protected float shootDamageBase;

    public PercentageReversibleNumber ShootDamage { get; protected set; }

    [SerializeField]
    protected Transform gunPoint;

    [SerializeField]
    protected GameObject bulletPrefab;

    [SerializeField]
    private float accelerationBase;

    public PercentageReversibleNumber Acceleration { get; protected set; }

    public float ShootDistance
    {
        get
        {
            return shootDistance;
        }
    }

    public GameObject BulletPrefab
    {
        get
        {
            return bulletPrefab;
        }
    }

    public Transform GunPoint
    {
        get
        {
            return gunPoint;
        }
    }

    [SerializeField]
    private TurretHSMStateAsset hsmAsset;

    private TurretHSMState hsm;

    protected Blackboard blackboard;

    private EyeCollection eyes;

    protected override void Awake()
    {
        base.Awake();

        HeadCollider = HeadRigidBody.GetComponent<SJCollider2D>();
        BodyCollider = BodyRigidBody.GetComponent<SJCollider2D>();

        ChargeTime = new PercentageReversibleNumber(chargeTimeBase);
        ShootDamage = new PercentageReversibleNumber(shootDamageBase);
        Acceleration = new PercentageReversibleNumber(accelerationBase);

        blackboard = new Blackboard();

        hsm = TurretHSMStateAsset.BuildFromAsset<TurretHSMState>(hsmAsset, this, blackboard);

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

        hsm.Update();
    }

    protected override void ProcessOrder(Order order)
    {
        hsm.SendEvent(order);
    }

    public virtual void TakeDamage(float damage, DamageType type)
    {
        
    }

    public EyeCollection GetEyes()
    {
        return eyes;
    }


    public override void GetEnslaved()
    {

    }

    public override bool ShouldBeSaved()
    {
        return true;
    }

    protected override void OnSave(SaveData data)
    {
        /*data.AddValue("x", transform.parent.position.x);
        data.AddValue("y", transform.parent.position.y);
        data.AddValue("limitLeft", leftLimit);
        data.AddValue("limitRight", rightLimit);
        data.AddValue("rotation z", transform.parent.rotation.z);
        data.AddValue("rotation x", transform.parent.rotation.x);
        data.AddValue("rotation y", transform.parent.rotation.y);
        data.AddValue("rotation w", transform.parent.rotation.w);*/
    }

    protected override void OnLoad(SaveData data)
    {
        /*transform.parent.position = new Vector2(data.GetAs<float>("x"), data.GetAs<float>("y"));
        transform.parent.rotation = new Quaternion(data.GetAs<float>("rotation x"), data.GetAs<float>("rotation y"), data.GetAs<float>("rotation z"), data.GetAs<float>("rotation w"));
        leftLimit = data.GetAs<float>("limitLeft");
        rightLimit = data.GetAs<float>("limitRight");*/
    }
}
