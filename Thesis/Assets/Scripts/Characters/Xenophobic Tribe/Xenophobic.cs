using UnityEngine;

public class Xenophobic : Tribal, IAudibleListener
{
    [SerializeField]
    protected XenophobicAttackState attackState;

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

    protected override void Awake()
    {
        base.Awake();

        attackState.InitializeState(actionFSM, State.Attacking, this, orders, blackboard);

        actionFSM.AddState(attackState);

        actionFSM.MakeTransition(State.Idle, Trigger.Attack, State.Attacking);
        actionFSM.MakeTransition(State.Attacking, Trigger.StopAttacking, State.Idle);
        
    }

    public override void GetEnslaved()
    {

    }

    public void Listen(ref AudibleData data)
    {
        
    }
}
