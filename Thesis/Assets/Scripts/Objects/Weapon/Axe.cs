using UnityEngine;
using UnityEngine.Animations;

public class Axe : Weapon, IThrowable
{
    private DamageType type;

    [SerializeField]
    private Collider2D sharpEdge;

    protected override void Awake()
    {
        base.Awake();

        type = DamageType.Sharp;
    }

    protected override void OnDrop()
    {
        base.OnDrop();

        Rigidbody2D.isKinematic = false;
    }

    protected override void OnUse()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable mortal = collision.GetComponent<IDamagable>();

        if(mortal != null)
        {
            mortal.TakeDamage(1, type);
        }
    }

    protected override void OnCollect(IHandOwner user)
    {
        Rigidbody2D.isKinematic = true;
    }

    public bool Throw()
    {
        return Drop();
    }

    protected override bool ValidateDrop()
    {
        return State != ActivableState.On;
    }

    public override bool Activate(IHandOwner user)
    {
        sharpEdge.enabled = !sharpEdge.enabled;

        if (sharpEdge.enabled)
        {
            State = ActivableState.On;
        }
        else
        {
            State = ActivableState.Off;
        }

        return true;
    }
}
