using SAM.Timers;
using System;
using UnityEngine;
using UnityEngine.Animations;

public class Axe : Weapon, IThrowable
{
    private DamageType type;

    [SerializeField]
    private Collider2D sharpEdge;

    private SyncTimer timer;
    private float attackInterval = 0.1f;
    private float recoilInterval = 0.3f;

    private Action<SyncTimer> onAttack;
    private Action<SyncTimer> onTerminate;
    
    new private Rigidbody2D rigidbody2D;
    
    private ParentConstraint parentConstraint;

    protected override void Awake()
    {
        base.Awake();

        type = DamageType.Sharp;
        rigidbody2D = GetComponentInChildren<Rigidbody2D>();
        parentConstraint = GetComponentInChildren<ParentConstraint>();

        timer = new SyncTimer();

        onAttack = OnAttack;
        onTerminate = OnTerminate;
    }

    protected void Update()
    {
        timer.Update(Time.deltaTime);
    }

    protected override void OnDrop()
    {
        base.OnDrop();

        rigidbody2D.isKinematic = false;

        parentConstraint.RemoveSource(0);

        parentConstraint.constraintActive = false;
    }

    protected override void OnUse()
    {
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
        FinishUse();
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
        rigidbody2D.isKinematic = true;

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = user.GetHand().transform;
        source.weight = 1;

        parentConstraint.AddSource(source);

        parentConstraint.constraintActive = true;

        parentConstraint.SetRotationOffset(0, new Vector3(0, 180, 0));
    }

    public bool Throw()
    {
        return Drop();
    }
}
