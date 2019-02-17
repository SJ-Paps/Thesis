using UnityEngine;

public class Turret : Robot, ISeer
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
    protected EyeCollection eyes;

    [SerializeField]
    private float acceleration;

    public float Acceleration
    {
        get
        {
            return acceleration;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        HeadCollider = HeadRigidBody.GetComponent<SJCollider2D>();
        BodyCollider = BodyRigidBody.GetComponent<SJCollider2D>();
    }


    public override void GetEnslaved()
    {

    }

    public override bool ShouldBeSaved()
    {
        return true;
    }

    public EyeCollection GetEyes()
    {
        return eyes;
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
