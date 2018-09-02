using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.FSM;

public class Xenophobic : Tribal, IAudibleListener
{
    [SerializeField]
    protected Weapon weapon;

    public Weapon Weapon
    {
        get
        {
            return weapon;
        }

        protected set
        {
            weapon = value;
        }
    }

    protected FSM<State, Trigger, ChangedStateEventArgs> attackFSM;

    protected override void Awake()
    {
        base.Awake();

        attackFSM = new FSM<State, Trigger, ChangedStateEventArgs>();

        attackFSM.AddState(State.Idle);
        attackFSM.AddState(new XenophobicAttackState(attackFSM, State.Attacking, this));

        attackFSM.MakeTransition(State.Idle, Trigger.Attack, State.Attacking);
        attackFSM.MakeTransition(State.Attacking, Trigger.StopAttack, State.Idle);

        attackFSM.StartBy(State.Idle);

        Weapon.SetUser(this);
    }

    protected override void Update()
    {
        base.Update();

        weapon.UseWeapon();
    }

    public override void GetEnslaved()
    {

    }

    public void Listen(ref AudibleData data)
    {
        
    }

    public override void SetOrder(Order order)
    {
        
    }
}
