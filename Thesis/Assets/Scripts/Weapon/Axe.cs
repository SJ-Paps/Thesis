using UnityEngine;
using UnityEngine.Animations;
using SAM.Timers;
using System;
using System.Collections;

public class Axe : Weapon
{
    private DeadlyType type;

    [SerializeField]
    private Collider2D sharpEdge;

    private SyncTimer timer;
    private float attackInterval = 1.2f;
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

        type = DeadlyType.Sharp;

        timer = new SyncTimer();

        onAttack = OnAttack;
        onTerminate = OnTerminate;
    }

    protected void Update()
    {
        timer.Update(Time.deltaTime);
    }

    public override void SetUser(Character character)
    {
        base.SetUser(character);
        
        rigidbody2D.isKinematic = true;

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = character.HandPoint;
        source.weight = 1;

        parentConstraint.AddSource(source);

        parentConstraint.constraintActive = true;

        Debug.Log(character.transform.rotation.eulerAngles);

        parentConstraint.SetRotationOffset(0, new Vector3(0, 180, 0));

        //transform.rotation = character.transform.rotation;
    }

    public override void Drop()
    {
        base.Drop();
        
        rigidbody2D.isKinematic = false;

        parentConstraint.RemoveSource(0);

        parentConstraint.constraintActive = false;
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
        IMortal mortal = collision.GetComponent<IMortal>();

        if(mortal != null)
        {
            mortal.Die(type);
        }
    }
}
