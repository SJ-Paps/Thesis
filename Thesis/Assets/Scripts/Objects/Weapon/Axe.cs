using UnityEngine;

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
        Rigidbody2D.isKinematic = false;

        Activate(null);
    }

    protected override void OnUse()
    {
        sharpEdge.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable mortal = collision.GetComponent<IDamagable>();

        if(mortal != null)
        {
            mortal.TakeDamage(1, type);
        }
    }

    protected override void OnCollect()
    {
        Activate(null);

        Rigidbody2D.isKinematic = true;
    }

    public bool Throw()
    {
        return Drop();
    }

    protected override void OnFinishUse()
    {
        sharpEdge.enabled = false;
    }

    protected override bool ValidateActivation(Character user)
    {
        return Rigidbody2D.isKinematic == false;
    }
}
