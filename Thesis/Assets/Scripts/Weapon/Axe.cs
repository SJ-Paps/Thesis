using UnityEngine;
using SAM.Timers;

public class Axe : Weapon, IDeadly
{
    [SerializeField]
    new private Collider2D collider;

    private SyncTimer timer;
    private float attackInterval = 0.4f;
    private float recoilInterval = 0.2f;

    protected override void Awake()
    {
        base.Awake();

        timer = new SyncTimer();
        
    }

    protected override void Update()
    {
        timer.Update(Time.deltaTime);
    }

    public override void UseWeapon()
    {
        BeingUsed = true;

        timer.Interval = attackInterval;
        timer.onTick += OnAttack;
        timer.Start();
    }

    private void OnAttack(SyncTimer timer)
    {
        collider.enabled = true;

        timer.Interval = recoilInterval;
        timer.onTick += OnTerminate;
        timer.Start();
    }

    private void OnTerminate(SyncTimer timer)
    {
        collider.enabled = false;
        BeingUsed = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EditorDebug.Log("AAAA");

        IMortal mortal = collision.GetComponent<IMortal>();

        if(mortal != null)
        {
            mortal.Die(this);
        }
    }
}
