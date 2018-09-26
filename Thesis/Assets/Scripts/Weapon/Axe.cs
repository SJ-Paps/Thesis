using UnityEngine;
using SAM.Timers;
using System;

public class Axe : Weapon, IDeadly
{
    [SerializeField]
    private Collider2D sharpEdge;

    private SyncTimer timer;
    private float attackInterval = 1f;
    private float recoilInterval = 0.5f;

    private Action<SyncTimer> onAttack;
    private Action<SyncTimer> onTerminate;

    [SerializeField]
    private FixedJoint2D joint2D;

    protected override void Awake()
    {
        base.Awake();

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

        joint2D.enabled = true;
        joint2D.connectedBody = character.RigidBody2D;
    }

    public override void Drop()
    {
        base.Drop();

        joint2D.enabled = false;
        joint2D.connectedBody = null;
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
            mortal.Die(this);
        }
    }
}
