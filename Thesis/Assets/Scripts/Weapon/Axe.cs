using UnityEngine;
using SAM.Timers;
using System;

public class Axe : Weapon, IDeadly
{
    [SerializeField]
    new private Collider2D collider;

    private SyncTimer timer;
    private float attackInterval = 1f;
    private float recoilInterval = 0.5f;

    private Action<SyncTimer> onAttack;
    private Action<SyncTimer> onTerminate;

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

    protected override void OnUseWeapon()
    {
        BeingUsed = true;

        timer.Interval = attackInterval;
        timer.onTick += onAttack;
        timer.Start();
    }

    private void OnAttack(SyncTimer timer)
    {
        collider.enabled = true;

        timer.Interval = recoilInterval;
        timer.onTick -= onAttack;
        timer.onTick += onTerminate;
        timer.Start();
    }

    private void OnTerminate(SyncTimer timer)
    {
        timer.onTick -= onTerminate;
        collider.enabled = false;
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
