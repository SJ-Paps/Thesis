using SAM.FSM;
using UnityEngine;

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

    protected FSM<State, Trigger> attackFSM;

    protected override void Awake()
    {
        base.Awake();

        attackFSM = new FSM<State, Trigger>();

        attackFSM.AddState(State.Idle);
        attackFSM.AddState(new XenophobicAttackState(attackFSM, State.Attacking, this, orders));

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
