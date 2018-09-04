using SAM.FSM;
using System.Collections.Generic;

public class XenophobicAttackState : CharacterAttackState
{
    new private Xenophobic character;
    private Weapon weapon;

    public XenophobicAttackState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Xenophobic controller, List<Character.Order> orderList) : base(fsm, state, controller, orderList)
    {
        character = controller;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

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
