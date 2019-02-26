using SAM.Timers;
using System;
using UnityEngine;
using UnityEngine.Animations;

public class Axe : Weapon
{
    private DamageType type;

    [SerializeField]
    private Collider2D sharpEdge;

    private SyncTimer timer;
    private float attackInterval = 0.1f;
    private float recoilInterval = 0.3f;

    private Action<SyncTimer> onAttack;
    private Action<SyncTimer> onTerminate;

    [SerializeField]
    new private Rigidbody2D rigidbody2D;

    [SerializeField]
    private ParentConstraint parentConstraint;

    protected override void Awake()
    {
        base.Awake();

        type = DamageType.Sharp;

        timer = new SyncTimer();

        onAttack = OnAttack;
        onTerminate = OnTerminate;
    }

    public override bool Collect(IHandOwner user)
    {
        if(base.Collect(user))
        {
            rigidbody2D.isKinematic = true;

            ConstraintSource source = new ConstraintSource();
            //source.sourceTransform = Owner.HandPoint;
            source.weight = 1;

            parentConstraint.AddSource(source);

            parentConstraint.constraintActive = true;

            parentConstraint.SetRotationOffset(0, new Vector3(0, 180, 0));

            return true;
        }

        return false;
    }

    protected void Update()
    {
        timer.Update(Time.deltaTime);
    }

    public override bool Drop()
    {
        if(base.Drop())
        {
            rigidbody2D.isKinematic = false;

            parentConstraint.RemoveSource(0);

            parentConstraint.constraintActive = false;

            return true;
        }

        return false;
    }

    public override bool Throw()
    {
        return Drop();
    }

    protected override void OnUseWeapon()
    {
        BeingUsed = true;

        timer.Interval = attackInterval;
        timer.onTick += onAttack;
        timer.Start();
    }

    private void OnAttack(SyncTimer timer)
    {
        sharpEdge.enabled = true;

        timer.Interval = recoilInterval;
        timer.onTick -= onAttack;
        timer.onTick += onTerminate;
        timer.Start();
    }

    private void OnTerminate(SyncTimer timer)
    {
        timer.onTick -= onTerminate;
        sharpEdge.enabled = false;
        BeingUsed = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable mortal = collision.GetComponent<IDamagable>();

        if(mortal != null)
        {
            mortal.TakeDamage(1, type);
        }
    }
}
