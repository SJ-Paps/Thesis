using SAM.FSM;

public class XenophobicAttackState : CharacterAttackState
{
    new private Xenophobic character;
    private Weapon weapon;

    public XenophobicAttackState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Xenophobic controller) : base(fsm, state, controller)
    {
        character = controller;
    }

    protected override void OnEnter(ref Character.ChangedStateEventArgs e)
    {
        base.OnEnter(ref e);

        weapon = character.Weapon;
        weapon.UseWeapon();
    }

    protected override void OnUpdate()
    {
        if(weapon.BeingUsed == false)
        {
            stateMachine.Trigger(Character.Trigger.StopAttack);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
