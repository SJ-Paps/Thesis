using UnityEngine;

public class Turret : Robot
{
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
    private float acceleration;

    public float Acceleration
    {
        get
        {
            return acceleration;
        }
    }

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

    protected override void Awake()
    {
        base.Awake();

        HeadCollider = HeadRigidBody.GetComponent<SJCollider2D>();
        BodyCollider = BodyRigidBody.GetComponent<SJCollider2D>();

        ChargeTime = new PercentageReversibleNumber(chargeTimeBase);
        ShootDamage = new PercentageReversibleNumber(shootDamageBase);
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
