using UnityEngine;
using System;

public class Turret : Robot, ISeer, IDamagable
{
    public enum State : byte
    {
        Base,
        Alive,
        Dead,
        Moving,
        Idle,
        Charging,
        Attacking
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

    private EyeCollection eyes;

    protected override void Awake()
    {
        base.Awake();

        HeadCollider = HeadRigidBody.GetComponent<SJCollider2D>();
        BodyCollider = BodyRigidBody.GetComponent<SJCollider2D>();

        ChargeTime = new PercentageReversibleNumber(chargeTimeBase);
        ShootDamage = new PercentageReversibleNumber(shootDamageBase);
        Acceleration = new PercentageReversibleNumber(accelerationBase);

        eyes = new EyeCollection(GetComponentsInChildren<Eyes>());
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

    protected override object GetSaveData()
    {
        return null;
    }

    protected override void LoadSaveData(object data)
    {
        
    }

    public override void PostSaveCallback()
    {

    }

    public override void PostLoadCallback(object dataSave)
    {

    }
}
